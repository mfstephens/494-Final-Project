﻿using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float speed;
	public float jumpSpeed;
	public float jumpShortSpeed;
	public float dropSpeed;
	public float wallJump;
	public float speedBoostForce;
	public float speedBoostDuration;
	public float forcePadForce;
	public int playerColor;
	public int hitSpeed;


	private bool inForcePad = false;
	private bool barrelRoll = false;
	private bool isOnGround = false;
	private bool doubleJump = false;
	private bool isOnLeftWall = false;
	private bool isOnRightWall = false;
	private bool isOnPlatform = false;
	private bool dropThroughPlatform = false; 

	private PlayerController playerController;
	private PlayerHealth playerHealth;


	// Use this for initializationx
	void Start () {
		playerController = this.GetComponent<PlayerController> ();
		playerHealth = this.GetComponent<PlayerHealth> ();
	}

	public void Movement(float moveX, float moveY, bool jump, bool cancelJump, bool speedBoost,bool lockPosition,bool drop){

		//Player must stand still while in lock position
		if (lockPosition) {
			Vector3 temp = rigidbody.velocity;
			temp.x = 0;
			rigidbody.velocity = temp;
			return;
		}


		//OnCollisionStay will read this and drop through the platform
		dropThroughPlatform = drop;

		//After player hits a force pad, they are subject to force pads velocity until they hit something
		if (!inForcePad) {
			rigidbody.velocity = new Vector2 (moveX * speed, rigidbody.velocity.y);
			if(moveY < 0)
				rigidbody.AddForce(new Vector3 (0, dropSpeed,0));
		}

		if (jump) {
			if (isOnGround || isOnPlatform)
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
			else if (doubleJump) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
				doubleJump = false;
			}
		} 

		else if (cancelJump) {
			if (rigidbody.velocity.y > jumpShortSpeed)
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpShortSpeed);
		} 

		//Player can use speed boost if they have enough energy
		else if (speedBoost) {
			print("Speed Boost");
			if(playerHealth.UseSpeedBoost())
				StartCoroutine("SpeedBoost");
		}


	}

	//Coroutine adds force over a period of speed boost duration (Maybe Add In Direction of Stick)
	IEnumerator SpeedBoost()
	{
		float startSpeedBoost = Time.time;
		print ("Speed Boost");
		while (Time.time < startSpeedBoost + speedBoostDuration) {
			//rigidbody.AddForce(transform.localScale.x*speedBoostForce,0,0);
			rigidbody.velocity = new Vector2(transform.localScale.x * speedBoostForce,0);
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
			// to handle the unlimited ball power up
//			if ((collision.gameObject.GetComponent<Ball>().playerColor != playerColor) && (unlimitedBallPowerUp.access.currentPlayer != null) && (unlimitedBallPowerUp.access.currentPlayer = this.gameObject)) {
//				return;
//			}

			if((!playerController.isPlayerBlinking() &&  collision.gameObject.GetComponent<Ball>().playerColor != playerColor) && (collision.relativeVelocity.magnitude > hitSpeed)){
				playerController.HitByBall();
				//TODO:fix disssssss
				if (collision.gameObject.GetComponent<Ball>().playerColor != -1)  {
					collision.gameObject.GetComponent<Ball>().findPlayerAndReturn();
				}
			}
		}

//		//If Hit by ball that is not in your possession
//		if (playerController.BallPossessed()!= null && playerController.BallPossessed() != collision.gameObject) {
//			if(collision.gameObject.CompareTag("Ball")){
//				if(!collision.gameObject.GetComponent<Ball>().thrownByPlayer(name)){
////					BallContainer.BallContainerSingleton.destroyBall(playerController.BallPossessed());
//					playerController.HitByBall();
//				}
//			}
//		}
	}

	void OnCollisionStay (Collision collision){

		//Drop Through Platform
		if (collision.gameObject.CompareTag ("Platform")) {
			if(dropThroughPlatform){
				Physics.IgnoreCollision(collision.collider,this.gameObject.collider,true);
				dropThroughPlatform = false;
			}
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
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("ForcePad")) {
			this.rigidbody.velocity = forcePadForce * other.gameObject.transform.up;
			inForcePad = true;
		}
		if (other.gameObject.CompareTag ("Ball")) {
			playerController.PickUpBall(other.gameObject);
		}
		
	}

	void OnTriggerExit(Collider other){
		if (other.gameObject.CompareTag ("ForcePad")) {

		}

	}
}
