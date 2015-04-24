using UnityEngine;
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

	Animator anim;
	float lastTimeGrounded = 0f;
	float okToJumpWhenInAir = .3f;
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
		anim = GetComponent<Animator> ();
		playerFall = GetComponent<PlayerFallOff> ();
		playerController = this.GetComponent<PlayerController> ();
	}

	public void Movement(float moveX, float moveY, bool jump, bool cancelJump, bool speedBoost,bool lockPosition,bool drop){

		if (isPlayerFalling) {
			if (transform.position.y > 350f) {
				transform.position += new Vector3(-1000f, 0, 0);
			}
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
			if (isOnGround || isOnPlatform || ((Time.time - lastTimeGrounded) < okToJumpWhenInAir)){
				GetComponent<Rigidbody>().velocity = new Vector2 (GetComponent<Rigidbody>().velocity.x, jumpSpeed);
				anim.SetTrigger("Jump");
				lastTimeGrounded = 0f;
			}
			else if (doubleJump) {
				GetComponent<Rigidbody>().velocity = new Vector2 (GetComponent<Rigidbody>().velocity.x, jumpSpeed);
				anim.SetTrigger("DoubleJump");
				doubleJump = false;
				EllipsoidParticleEmitter[] e = GetComponentsInChildren<EllipsoidParticleEmitter>();
				for(int i = 0; i < e.Length; i++){
					e[i].emit = true;
				}
				Invoke("stopEmission",.3f);
			}
		} 
		
		else if (cancelJump) {
			if (GetComponent<Rigidbody>().velocity.y > jumpShortSpeed)
				GetComponent<Rigidbody>().velocity = new Vector2 (GetComponent<Rigidbody>().velocity.x, jumpShortSpeed);
				lastTimeGrounded = 0f;

		} 
		
		//Player can use speed boost if they have enough energy
//		else if (speedBoost) {
//	
//			if(playerHealth.UseSpeedBoost())
//				StartCoroutine("SpeedBoost");
//		}

		// if you're on a moving platform
		if (isOnMovingPlatform) {
			GetComponent<Rigidbody>().velocity += movingPlatform.GetComponent<Rigidbody>().velocity;
		}

		if (!isOnGround && !isOnPlatform) {
			anim.SetBool ("InAir", true);
			anim.SetFloat ("Running", 0f);
		}
		else {
			anim.SetBool ("InAir", false);
			anim.SetFloat ("Running", Mathf.Abs (moveX));	
		}
	}

	void stopEmission(){
		EllipsoidParticleEmitter[] e = GetComponentsInChildren<EllipsoidParticleEmitter>();
		for(int i = 0; i < e.Length; i++){
			e[i].emit = false;
		}
	}
	
	//Coroutine adds force over a period of speed boost duration (Maybe Add In Direction of Stick)
	IEnumerator SpeedBoost()
	{
		float startSpeedBoost = Time.time;
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
			if(!isCollisionAboveMe(collision)){
				isOnGround = true;
				doubleJump = true;
				inForcePad = false;
			}
		}
		
		if (collision.gameObject.CompareTag ("Platform")) {
			if(!isCollisionAboveMe(collision)){
				isOnPlatform = true;
				doubleJump = true;
				inForcePad = false;
			}
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

				//if (Application.loadedLevelName.Equals("_ThreeToFour")) {
					int temp = 100;

					if (FlagRotate.access.currentPlayer == (this.playerColor - 1)) {
						foreach (GameObject cur in this.playerController.coins) {
							cur.GetComponent<CoinBehavior>().playerHit(collision.gameObject.GetComponent<Ball>().ballOwner);
						}
						this.playerController.coins.Clear();
						temp = FlagRotate.access.currentBounty;
					}

					FlagRotate.access.playerScores[collision.gameObject.GetComponent<Ball>().playerColor - 1] += temp;
					FlagRotate.access.MovePlayerCursor(collision.gameObject.GetComponent<Ball>().playerColor - 1);
//					FlagRotate.access.playerScoreTexts[collision.gameObject.GetComponent<Ball>().playerColor - 1].text = FlagRotate.access.playerScores[collision.gameObject.GetComponent<Ball>().playerColor - 1].ToString();

					//Pass array of player scores to be sorted to print out their rank
					//ScoreBoard.scoreBoard.setPlayerRank(FlagRotate.access.playerScores);
					
					//Set the player score on the scoreboard
					//ScoreBoard.scoreBoard.setPlayerScore(collision.gameObject.GetComponent<Ball>().playerColor - 1,FlagRotate.access.playerScores[collision.gameObject.GetComponent<Ball>().playerColor - 1].ToString());


					//collision.gameObject.GetComponent<Ball>().ballOwner.GetComponent<PlayerMove>().showScore.addScore(temp);
				//}

				//if (Application.loadedLevelName.Equals("_ThreeToFour")) {
					if ((FlagRotate.access.possessingPlayer != null) && FlagRotate.access.possessingPlayer.name.Equals(this.gameObject.name)) {
						FinalStatistics.finalStatistics.CrownLeaderKilledBy(collision.gameObject.GetComponent<Ball>().playerColor);
					}
				//}
				playerController.HitByBall(collision);
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
			if(!isCollisionAboveMe(collision)){
				isOnMovingPlatform = true;
				movingPlatform = collision.gameObject;
			}
		}

		if (collision.gameObject.CompareTag("Ground")) {
			if(!isCollisionAboveMe(collision)){
				isOnGround = true;
				doubleJump = true;
				inForcePad = false;
			}
		}
		
		if (collision.gameObject.CompareTag ("Platform")) {
			if(!isCollisionAboveMe(collision)){
				isOnPlatform = true;
				doubleJump = true;
				inForcePad = false;
			}
		}
		
	}
	
	void OnCollisionExit(Collision collision){
		if (collision.gameObject.CompareTag ("Ground")) {
			isOnGround = false;
			if(!isCollisionAboveMe(collision))
				lastTimeGrounded = Time.time;
		}
		
		if (collision.gameObject.CompareTag ("Platform")){
			isOnPlatform = false;
			if(!isCollisionAboveMe(collision))
				lastTimeGrounded = Time.time;
		}
		
		if (collision.gameObject.CompareTag ("LeftWall"))
			isOnLeftWall = false;
		
		if (collision.gameObject.CompareTag ("RightWall"))
			isOnRightWall = false;
		if (collision.gameObject.name == "Moving Platform") {
			isOnMovingPlatform = false;
			if(!isCollisionAboveMe(collision))
				lastTimeGrounded = Time.time;
		}
	}
	

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("ForcePad")) {
			this.GetComponent<Rigidbody>().velocity = forcePadForce * other.gameObject.transform.up;
			inForcePad = true;
		}
		if (!playerController.isBallPossessed && playerController.possessedBall.Equals(other.gameObject.GetComponent<Ball>())) {
			playerController.PickUpBall(other.gameObject);
		}
	}

	public bool canShield() {
		if (isOnPlatform || isOnGround) 
			return true;

		return false;
	}

	bool isCollisionAboveMe(Collision other){
		return(other.transform.position.y > transform.position.y);

	}

}
