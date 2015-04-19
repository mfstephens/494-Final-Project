using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class PlayerController : MonoBehaviour {
	
	public float throwSpeed;
	public float stunnedDuration;
	public float rotateSpeed;
	public Vector3 startPos;
	public GameObject bubbleShield;
	
	private float totalRotation = 0;
	private float timeHit;
	private float initialFlickDownTime = -5f;
	private int playerNumber;
	public float throwCoolDown;
	private float lastThrow = 0;
	Animator anim;
	public GameObject bounty;
	public GameObject ballHoldPosition;	

	public bool invincible = false;
	private bool isSuspended = false;
	private bool jump = false;
	private bool jumpCancel = false;
	private bool speedBoost = false;
	private bool justHit = false;
	private bool playerStunned = false;
	private bool barrelRoll = false;
	public bool lockPosition = false;
	private bool dropThroughPlatform = false;
	public bool isStunned = false;
	public bool shieldOn = false;
	
	public InputDevice playerControl;
	private PlayerMove playerMovement;

	public Ball possessedBall;
	public bool isBallPossessed = false;

	public Color playerColor;
	SkinnedMeshRenderer myMesh;
	private int lerpDirection;
	private float curLerp = 0;

	// player aim stuff
	public PlayerAim playerAim;
	Vector3 lastAimPosition;
	Vector3 lastShotDirection;
	public Ball ballTarget;

	public List<GameObject> coins;

	void Awake() {
		playerColor = GetComponentInChildren<SkinnedMeshRenderer> ().material.color;
	}

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		myMesh = GetComponentInChildren<SkinnedMeshRenderer> ();
		playerColor = GetComponentInChildren<SkinnedMeshRenderer> ().material.color;

		playerMovement = GetComponent<PlayerMove> ();

		//Set the font color to correspond to player color
		if (Application.loadedLevelName.Equals ("_ThreeToFour")) {
			ScoreBoard.scoreBoard.setPlayerColor (playerMovement.playerColor - 1, playerColor);
		}

		int temp = InputManager.Devices.Count;
		ballTarget = null;
		if (gameObject.name == "Player1") {
			if(temp < 1) return;
			playerControl = InputManager.Devices [0];
			playerNumber = 1;
			print ("1 player!!!");
		}
		
		if (name == "Player2") {
			if(temp < 2) return;
			playerControl = InputManager.Devices [1];
			playerNumber = 2;
			print ("2 player!!!");

		}
		
		if (name == "Player3") {
			if(temp < 3) return;
			playerControl = InputManager.Devices [2];
			playerNumber = 3;
			print ("3 player!!!");

		}
		
		if (name == "Player4") {
			if(temp < 4) return;
			playerControl = InputManager.Devices[3];
			playerNumber = 4;
			print ("4 player!!!");

		}

		/* Moves ball to player, sets balls velocity to 0, tells the player he has a ball and their player color*/
//		possessedBall.transform.position = this.transform.position;
//		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
//		isBallPossessed = true;
		Physics.IgnoreCollision (this.gameObject.GetComponent<Collider> (), possessedBall.gameObject.GetComponent<Collider> ());
		PickUpBall (possessedBall.gameObject);		
	}
	
	// Update is called once per frame
	void Update () {

		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;
		
		//Perform a 360 degree flip if player is not on ground
		if (barrelRoll) {
			if(!playerMovement.isPlayerOnGround()){
				float rotationAmount = rotateSpeed * Time.deltaTime;
				transform.Rotate(new Vector3(0,0,rotationAmount));
				totalRotation+=rotationAmount;
				if(totalRotation>=360){
					transform.rotation = Quaternion.identity;
					barrelRoll = false;
					totalRotation = 0;
				}
			}
			else{
				transform.rotation = Quaternion.identity;
				barrelRoll = false;
				totalRotation = 0;
			}
		}
		
		//Characters blinks when they were hit for stunnedDuration (can customize to stun, add more effects)
		if (justHit) {
			if(timeHit + stunnedDuration <= Time.time){
				justHit = false;
			}
		}
		
		//Change Z Axis of ball when possessed to appear in front of you
		if (isBallPossessed) {
			possessedBall.transform.position = new Vector3(this.transform.position.x + 9f,this.transform.position.y + 6f,this.transform.position.z);
			possessedBall.transform.position = ballHoldPosition.transform.position;
		}
		
		//Check if player flicked down on joystick so they can drop through platform
//		if (playerMovement.isPlayerOnPlatform()) {
//			if (verticalMovement < -.5 && horizontalMovement < .2 && horizontalMovement > -.2)
//				initialFlickDownTime = Time.time;
//			else if (verticalMovement == 0 && horizontalMovement == 0) {
//				if (Time.time - initialFlickDownTime <= .2){ 
//					dropThroughPlatform = true;
//				}
//			}
//		}

		Vector2 throwMovement = new Vector2 (horizontalMovement, verticalMovement);
		Vector3 shotDirection3D = new Vector3 (throwMovement.x, throwMovement.y, 1);
		lastShotDirection = shotDirection3D;
		if (shotDirection3D.x == 0 && shotDirection3D.y == 0) {
			playerAim.UpdateGuidePosition (lastAimPosition);
		} else {
			playerAim.UpdateGuidePosition (shotDirection3D);
			lastAimPosition = shotDirection3D;
		}
		
		if (playerControl.Action1.WasPressed) {
			jump = true;
		} 
		else if (playerControl.RightTrigger.WasPressed) {
			if (isBallPossessed && !shieldOn && !lockPosition && (Time.time - lastThrow > throwCoolDown)) {
				ThrowBall();
				lastThrow = Time.time;
				anim.SetTrigger("Throw");

			} 
			else if (!shieldOn && !lockPosition) {
				possessedBall.returnBall();
			}

		} else if (!isBallPossessed && ballTarget == null) {
			Vector3 forceVector = new Vector3(horizontalMovement * throwSpeed, verticalMovement * throwSpeed, 1);
			possessedBall.applyExtraControl(forceVector); 
		}
		else if (playerControl.Action3.WasPressed) {
			speedBoost = true;
		}
		else if (playerControl.Action4.WasPressed)
			BarrelRoll ();
		else if (playerControl.MenuWasPressed)
			PauseMenu.access.PlayerPausedGame (playerNumber - 1);
		
		if (playerControl.LeftTrigger.IsPressed && !lockPosition) {
			//lockPosition = true;
			bubbleShield.GetComponent<BubbleShield>().startShield();
		}
		
		if (playerControl.Action1.WasReleased)
			jumpCancel = true;
		if (playerControl.LeftTrigger.WasReleased && !lockPosition) {
			//lockPosition = false;
			bubbleShield.GetComponent<BubbleShield>().endShield();
		}


		if (horizontalMovement > .05 && transform.eulerAngles.y != 120f) {
			transform.eulerAngles = new Vector3 (0, 120f, 0);
			//bounty.transform.localEulerAngles = new Vector3(0, -120f, 0);
		}
			
		if (horizontalMovement < -.05 && transform.eulerAngles.y != -120f) {
			transform.eulerAngles = new Vector3 (0, -120f, 0);
			//bounty.transform.Rotate (new Vector3(0, 120f, 0));
		}


		if (isSuspended) {
			
		}



		playerMovement.Movement (horizontalMovement, verticalMovement, jump, jumpCancel,speedBoost,lockPosition,dropThroughPlatform);
		
		jump = false;
		jumpCancel = false;
		speedBoost = false;
		dropThroughPlatform = false;
	}

	void FixedUpdate() {
		if (invincible) {
			if (curLerp >= 0.8f) {
				lerpDirection = -1;
			}
			else if (curLerp <= 0) {
				lerpDirection = 1;
			}

			curLerp += 0.03f * lerpDirection;
			myMesh.material.color = Color.Lerp(playerColor, myMesh.material.color = Color.black, curLerp);
		} 
	}
	
				
	//Throw ball in direction of left stick
	void ThrowBall(){
		if (playerMovement.isPlayerFalling) {
			return;
		}

		if (invincible)
			endInvincible ();

		FinalStatistics.finalStatistics.PlayerThrewBall (playerNumber);

		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;


		isBallPossessed = false;

		Vector2 throwMovement = new Vector2 (horizontalMovement, verticalMovement);
		if (ballTarget != null) {
			Vector3 distance3D = ballTarget.transform.position - transform.position;
			throwMovement = new Vector2 (distance3D.x, distance3D.y);
		}

		if (horizontalMovement == 0 && verticalMovement == 0) {
			throwMovement = lastAimPosition;
		}


		throwMovement.Normalize ();


		//possessedBall.gameObject.GetComponent<Collider> ().enabled = true;
		Physics.IgnoreCollision (this.gameObject.GetComponent<Collider> (), possessedBall.gameObject.GetComponent<Collider> ());
		possessedBall.ballCanBeControlled = true;

		possessedBall.gameObject.GetComponent<TrailRenderer> ().enabled = true;
		possessedBall.GetComponent<Rigidbody>().velocity = new Vector2 (throwSpeed * throwMovement.x, throwMovement.y*throwSpeed);
		possessedBall.GetComponent<Ball> ().possesed = false;
	}
	
	public void PickUpBall(GameObject ball){
		ball.GetComponent<Ball> ().possesed = true;
		ball.transform.position = this.transform.position;
		//ball.gameObject.GetComponent<Collider> ().enabled = false;
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
		isBallPossessed = true;
		possessedBall.gameObject.GetComponent<TrailRenderer> ().enabled = false;
	}
	
	void BarrelRoll(){
		barrelRoll = true;
	}
	
	//React to getting hit by a ball
	public void HitByBall() {
		anim.SetTrigger ("Hit");
		this.transform.position -= new Vector3 (0, 0, 40f);

		if (Application.loadedLevelName.Equals ("_OneToTwo")) {
			CaptureTheFlagMode.access.playerScore(this.gameObject);
			justHit = true;
			CrowdBehavior.access.crowdFlash();
//			if (!isBallPossessed) {
//				PickUpBall(possessedBall.gameObject);
//			}
			//possessedBall.gameObject.SetActive(false);
			playerMovement.isOnMovingPlatform = false;
			MainCamera.access.players.Remove (this.gameObject);
			MainCamera.access.players.Remove (possessedBall.gameObject);
			Invoke("returnToStart",2f);
		}
		else if (Application.loadedLevelName.Equals("_ThreeToFour") || Application.loadedLevelName.Equals("_ThreeToFour_small")) {
//			if (KingOfTheHill.access != null && KingOfTheHill.access.isKing(playerMovement.playerColor)) {
//				this.transform.localScale -= new Vector3(1f, 3f, 0.625f);
//				possessedBall.transform.localScale -= new Vector3(1.125f, 1.125f, 1.125f);
//			}
//			else {

			if ((FlagRotate.access.possessingPlayer != null) && FlagRotate.access.possessingPlayer.name.Equals(this.gameObject.name)) {
				//FinalStatistics.finalStatistics.CrownLeaderKilledBy(
				FlagRotate.access.dropFlag();
				FlagRotate.access.currentPlayer = -1;
			}
	
				justHit = true;
				CrowdBehavior.access.crowdFlash();

//				if (!isBallPossessed) {
//					PickUpBall(possessedBall.gameObject);
//				}
				//possessedBall.gameObject.SetActive(false);
				playerMovement.isOnMovingPlatform = false;
				MainCamera.access.players.Remove (this.gameObject);
				MainCamera.access.players.Remove (possessedBall.gameObject);
				Invoke("returnToStart",2f);
			}
//		}
	}
	
		
	//Returns whether the player is blinking meaning they were just hit and invulnerable
	public bool isPlayerBlinking(){
		return justHit;
	}

	public void returnToStart() {
		Rigidbody rigid = this.gameObject.GetComponent<Rigidbody> ();
		rigid.useGravity = false;
		invincible = true;

		this.transform.position = RespawnPositionTwo.access.generateRespawnPoint ();

		//possessedBall.gameObject.SetActive(true);
		//possessedBall.transform.position = this.transform.position;
		possessedBall.ballShouldReturn = true;
		rigid.constraints = RigidbodyConstraints.FreezeRotation ^ RigidbodyConstraints.FreezePositionZ;
		rigid.rotation = Quaternion.identity;
		rigid.velocity = Vector3.zero;

		possessedBall.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		Physics.IgnoreCollision (this.gameObject.GetComponent<Collider> (), possessedBall.gameObject.GetComponent<Collider> ());
		MainCamera.access.players.Add (this.gameObject);
		MainCamera.access.players.Add (possessedBall.gameObject);
		playerMovement.isPlayerFalling = false;
		if (Application.loadedLevelName.Equals ("_ThreeToFour") || Application.loadedLevelName.Equals("_ThreeToFour_small")) {
			Physics.IgnoreCollision (this.gameObject.GetComponent<Collider> (), FlagRotate.access.gameObject.GetComponent<Collider> (), false);
		}
		Invoke ("dropPlayer", 0.5f);
		Invoke ("endInvincible", 2f);
		//MainCamera.access.players.Add (possessedBall.gameObject);
	}

	public void dropPlayer() {
		GetComponent<Rigidbody> ().useGravity = true;
		isSuspended = false;
		transform.eulerAngles = new Vector3 (0, 180f, 0);
	}

	public void endInvincible() {
		invincible = false;
		GetComponentInChildren<SkinnedMeshRenderer> ().material.color = playerColor;
		curLerp = 0;
		anim.SetTrigger ("EndInvincible");
	}

}


