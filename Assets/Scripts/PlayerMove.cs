﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {
	
	public float speed;
	public float jumpSpeed;
	public float jumpShortSpeed;
	public float wallJump;
	public float speedBoostForce;
	public float speedBoostDuration;
	public float forcePadForce;
	public int playerColor;
	public int hitSpeed;
	public float reducedAirFactor;

	private bool inForcePad = false;
	private bool barrelRoll = false;
	private bool isOnGround = false;
	public bool doubleJump = false;
	private bool isOnLeftWall = false;
	private bool isOnRightWall = false;
	private bool isOnPlatform = false;
	private bool dropThroughPlatform = false; 
	public bool isOnMovingPlatform = false;
	private GameObject movingPlatform = null;
	private PlayerFallOff playerFall;
	public bool isPlayerFalling = false;

	public GameObject fireeeeee;

	public BountyDisplay showScore;
	private PlayerController playerController;

	
	// Use this for initializationx
	void Start () {
		playerFall = GetComponent<PlayerFallOff> ();
		playerController = this.GetComponent<PlayerController> ();
	}

	public void Movement(float moveX, float moveY, bool jump, bool cancelJump, bool speedBoost,bool lockPosition,bool drop){

		if (isPlayerFalling) {
			return;
		}

		//Player must stand still while in lock position
		if (lockPosition && (isOnGround || isOnPlatform)) {
			Vector3 temp = GetComponent<Rigidbody>().velocity;
			temp.x = 0;
			GetComponent<Rigidbody>().velocity = temp;
			// if you're locked on a moving platform
			if (isOnMovingPlatform) {
				GetComponent<Rigidbody>().velocity += movingPlatform.GetComponent<Rigidbody>().velocity;
			}
			return;
		}

		//OnCollisionStay will read this and drop through the platform
		dropThroughPlatform = drop;
		
		//After player hits a force pad, they are subject to force pads velocity until they hit something
		if (!inForcePad) {
			GetComponent<Rigidbody>().velocity = new Vector2 (moveX * speed, GetComponent<Rigidbody>().velocity.y);
			if (!isOnGround && !isOnPlatform) {
				GetComponent<Rigidbody>().velocity = new Vector2 (GetComponent<Rigidbody>().velocity.x * reducedAirFactor, GetComponent<Rigidbody>().velocity.y);
			}
		}
		
		if (jump) {
			if (isOnGround || isOnPlatform)
				GetComponent<Rigidbody>().velocity = new Vector2 (GetComponent<Rigidbody>().velocity.x, jumpSpeed);
			else if (doubleJump) {
				GetComponent<Rigidbody>().velocity = new Vector2 (GetComponent<Rigidbody>().velocity.x, jumpSpeed);
				doubleJump = false;
			}
		} 
		
		else if (cancelJump) {
			if (GetComponent<Rigidbody>().velocity.y > jumpShortSpeed)
				GetComponent<Rigidbody>().velocity = new Vector2 (GetComponent<Rigidbody>().velocity.x, jumpShortSpeed);
		} 
		
		//Player can use speed boost if they have enough energy
//		else if (speedBoost) {
//			print("Speed Boost");
//			if(playerHealth.UseSpeedBoost())
//				StartCoroutine("SpeedBoost");
//		}

		// if you're on a moving platform
		if (isOnMovingPlatform) {
			GetComponent<Rigidbody>().velocity += movingPlatform.GetComponent<Rigidbody>().velocity;
		}
		
	}
	
	//Coroutine adds force over a period of speed boost duration (Maybe Add In Direction of Stick)
	IEnumerator SpeedBoost()
	{
		float startSpeedBoost = Time.time;
		print ("Speed Boost");
		while (Time.time < startSpeedBoost + speedBoostDuration) {
			//rigidbody.AddForce(transform.localScale.x*speedBoostForce,0,0);
			GetComponent<Rigidbody>().velocity = new Vector2(transform.localScale.x * speedBoostForce,0);
			yield return null;
		}
	}
	
	public bool isPlayerOnGround(){
		return isOnGround;
	}
	
	public bool isPlayerOnPlatform(){
		return isOnPlatform;
	}
	
	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.CompareTag("Ground")) {
			isOnGround = true;
			doubleJump = true;
			inForcePad = false;
		}
		
		if (collision.gameObject.CompareTag ("Platform")) {
			isOnPlatform = true;
			doubleJump = true;
			inForcePad = false;
		}
		
		if (collision.gameObject.CompareTag ("LeftWall")) {
			isOnLeftWall = true;
			inForcePad = false;
		}
		
		if (collision.gameObject.CompareTag ("RightWall")) {
			inForcePad = false;
			isOnRightWall = true;
		}
		
		if(collision.gameObject.CompareTag("Ball")){

			if (playerController.invincible) {
				return;
			}

			if ((collision.gameObject.GetComponent<Ball>().playerColor != playerColor) && (collision.gameObject.GetComponent<Ball>().possesed == false)) {
				//Add Stats for player hit and player who threw ball
				FinalStatistics.finalStatistics.PlayerHitByBall(playerColor,collision.gameObject.GetComponent<Ball>().playerColor);

				if (Application.loadedLevelName.Equals("_ThreeToFour")) {
					int temp = 100;

					if (FlagRotate.access.currentPlayer == (this.playerColor - 1)) {
						foreach (GameObject cur in this.playerController.coins) {
							cur.GetComponent<CoinBehavior>().playerHit(collision.gameObject.GetComponent<Ball>().ballOwner);
						}
						this.playerController.coins.Clear();
						temp = FlagRotate.access.currentBounty;
					}

					FlagRotate.access.playerScores[collision.gameObject.GetComponent<Ball>().playerColor - 1] += temp;
					FlagRotate.access.playerScoreTexts[collision.gameObject.GetComponent<Ball>().playerColor - 1].text = FlagRotate.access.playerScores[collision.gameObject.GetComponent<Ball>().playerColor - 1].ToString();

					//Pass array of player scores to be sorted to print out their rank
					ScoreBoard.scoreBoard.setPlayerRank(FlagRotate.access.playerScores);
					
					//Set the player score on the scoreboard
					ScoreBoard.scoreBoard.setPlayerScore(collision.gameObject.GetComponent<Ball>().playerColor - 1,FlagRotate.access.playerScores[collision.gameObject.GetComponent<Ball>().playerColor - 1].ToString());

					collision.gameObject.GetComponent<Ball>().ballOwner.GetComponent<PlayerMove>().showScore.addScore(temp);
				}

				if ((FlagRotate.access.possessingPlayer != null) && FlagRotate.access.possessingPlayer.name.Equals(this.gameObject.name)) {
					FinalStatistics.finalStatistics.CrownLeaderKilledBy(collision.gameObject.GetComponent<Ball>().playerColor);
				}

				playerController.HitByBall();
				isPlayerFalling = true;
				playerFall.fallOff(collision.gameObject);
				
			}
		}
	}
	
	void OnCollisionStay (Collision collision){
		
		//Drop Through Platform
		if (collision.gameObject.CompareTag ("Platform")) {
			if(dropThroughPlatform){
				Physics.IgnoreCollision(collision.collider,this.gameObject.GetComponent<Collider>(),true);
				dropThroughPlatform = false;
			}
		}
		
		if (collision.gameObject.name == "Moving Platform") {
			isOnMovingPlatform = true;
			movingPlatform = collision.gameObject;
		}

		if (collision.gameObject.CompareTag("Ground")) {
			isOnGround = true;
			doubleJump = true;
			inForcePad = false;
		}
		
		if (collision.gameObject.CompareTag ("Platform")) {
			isOnPlatform = true;
			doubleJump = true;
			inForcePad = false;
		}
		
	}
	
	void OnCollisionExit(Collision collision){
		if (collision.gameObject.CompareTag ("Ground"))
			isOnGround = false;
		
		if (collision.gameObject.CompareTag ("Platform"))
			isOnPlatform = false;
		
		if (collision.gameObject.CompareTag ("LeftWall"))
			isOnLeftWall = false;
		
		if (collision.gameObject.CompareTag ("RightWall"))
			isOnRightWall = false;
		if (collision.gameObject.name == "Moving Platform") 
			isOnMovingPlatform = false;
	}
	
	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("ForcePad")) {
			this.GetComponent<Rigidbody>().velocity = forcePadForce * other.gameObject.transform.up;
			inForcePad = true;
		}
		if (playerController.possessedBall.Equals(other.gameObject.GetComponent<Ball>())) {
			playerController.PickUpBall(other.gameObject);
		}
	}

	public bool canShield() {
		print ("calling canshield");
		if (isOnPlatform || isOnGround) 
			return true;

		return false;
	}


}
