using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour {

	public float throwSpeed;
	public float stunnedDuration;
	public float rotateSpeed;

	private float totalRotation = 0;
	private float timeHit;
	private float initialFlickDownTime;
	private int playerNumber;
	
	public bool controlBall = false;
	public bool throwing = false; 	//used and set by ball and powerupcontrolball. throwing is
									// true when the player is activley using the powerup. 
									// throwing has no relevance to actual throwing
	public bool timeSlowPowerUp = false;
	public float timeScale = 1f;

	private bool jump = false;
	private bool jumpCancel = false;
	private bool speedBoost = false;
	private bool justHit = false;
	private bool playerStunned = false;
	private bool barrelRoll = false;
	public bool justThrown = false;
	private bool lockPosition = false;
	private bool dropThroughPlatform = false;

	public InputDevice playerControl;
	private PlayerMove playerMovement;
	private PlayerHealth playerHealth;

	public Ball possessedBall;
	public bool isBallPossessed = false;
	private PlayerAim playerAim;
	
	private Color playerColor;
	public Color beginColor;
	public Color endColor;
	
	// Use this for initialization
	void Start () {
		playerControl = InputManager.ActiveDevice;
		playerMovement = GetComponent<PlayerMove> ();
		playerHealth = GetComponent<PlayerHealth> ();
		int temp = InputManager.Devices.Count;
		if (name == "Player1") {
			if(temp < 1) return;
			playerControl = InputManager.Devices [0];
			playerAim = GameObject.Find ("Guide1").GetComponent<PlayerAim> ();
			playerNumber = 1;
		}

		if (name == "Player2") {
			if(temp < 2) return;
			playerControl = InputManager.Devices [1];
			playerAim = GameObject.Find ("Guide2").GetComponent<PlayerAim> ();
			playerNumber = 2;
		}

		if (name == "Player3") {
			if(temp < 3) return;
			playerControl = InputManager.Devices [2];
			playerAim = GameObject.Find ("Guide3").GetComponent<PlayerAim> ();
			playerNumber = 3;
		}

		if (name == "Player4") {
			if(temp < 4) return;
			playerControl = InputManager.Devices[3];
			playerAim = GameObject.Find ("Guide4").GetComponent<PlayerAim>();
			playerNumber = 4;
		}


		possessedBall.transform.position = this.transform.position;
		this.rigidbody.velocity = Vector3.zero;
		isBallPossessed = true;
		playerColor = this.renderer.material.color;

	}
	
	// Update is called once per frame
	void Update () {

		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;

		//Prevents player from sucking up ball that was just thrown
		if (isBallPossessed) {
			if (Vector3.Distance (possessedBall.transform.position, this.transform.position) > 15f) {
				justThrown = false;
			}
		}
		if (possessedBall != null && Vector3.Distance (possessedBall.transform.position, this.transform.position) > 15f) {
			justThrown = false;
		}

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
				CancelBlink();
				this.renderer.material.color = playerColor;
				justHit = false;
			}
		}

		//Change Z Axis of ball when possessed to appear in front of you
		if (isBallPossessed) {
			possessedBall.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,5f);
		}

		//Check if player flicked down on joystick so they can drop through platform
		if (playerMovement.isPlayerOnPlatform()) {
			if (verticalMovement < 0 && horizontalMovement < .2 && horizontalMovement > -.2)
				initialFlickDownTime = Time.time;
			else if (verticalMovement == 0 && horizontalMovement == 0) {
				if (Time.time - initialFlickDownTime <= .2){ 
					dropThroughPlatform = true;
				}
			}
		}
			
		if (playerControl.Action1.WasPressed)
			jump = true;
		else if (playerControl.Action3.WasPressed) {
			if (isBallPossessed) {
				throwing = true;
				ThrowBall ();
			}
		} 
		else if (playerControl.RightTrigger.WasPressed) {
			print ("Right Trigger Pressed");
			speedBoost = true;
		}
		else if (playerControl.Action4.WasPressed)
			BarrelRoll ();

		if (playerControl.LeftTrigger.IsPressed && isBallPossessed) {
			Vector3 shotDirection3D = new Vector3 ( playerControl.LeftStickX,  playerControl.LeftStickY, 1f);
			playerAim.UpdateGuidePosition (shotDirection3D);
			lockPosition = true;
		}

		if (playerControl.Action1.WasReleased)
			jumpCancel = true;
		if (playerControl.LeftTrigger.WasReleased) {
			lockPosition = false;
			playerAim.RemoveGuide();
		}
	}

	void FixedUpdate(){
		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;

		// has controlBall power up
		if(controlBall){
			possessedBall.rigidbody.velocity = new Vector2 (throwSpeed * horizontalMovement/1.8f, verticalMovement*throwSpeed/1.8f);
			return;
		}

		if (horizontalMovement > 0 && transform.localScale.x < 0)
			Flip ();
		if(horizontalMovement<0 && transform.localScale.x > 0)
			Flip();

		playerMovement.Movement (horizontalMovement, verticalMovement, jump, jumpCancel,speedBoost,lockPosition,dropThroughPlatform);

		jump = false;
		jumpCancel = false;
		speedBoost = false;
		dropThroughPlatform = false;
	}

	void Flip(){
		Vector3 local = transform.localScale;
		local.x *= -1;
		transform.localScale = local;
	}

	void stunPlayer(){
		timeHit = Time.time;
		this.renderer.material.color = beginColor;
		InvokeRepeating ("Blink",.1f, .25f);
	}

	void Blink(){
		if(this.renderer.material.color == beginColor)
			this.renderer.material.color = endColor;
		else
			this.renderer.material.color = beginColor;
	}

	void CancelBlink(){
		CancelInvoke ();
	}
	
	//Throw ball in direction of left stick
	void ThrowBall(){

		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;

		isBallPossessed = false;
		justThrown = true;
		//possessedBall.transform.position += new Vector3(horizontalMovement, verticalMovement, 10);

		Vector2 throwMovement = new Vector2 (horizontalMovement, verticalMovement);
		throwMovement.Normalize ();

		possessedBall.rigidbody.collider.isTrigger = false;
		possessedBall.ballThrown ();
		if (controlBall) return;
		possessedBall.rigidbody.velocity = new Vector2 (throwSpeed * throwMovement.x, throwMovement.y*throwSpeed);

		if ((unlimitedBallPowerUp.access.currentPlayer != null) && unlimitedBallPowerUp.access.currentPlayer.Equals (this.gameObject)) {
			if (unlimitedBallPowerUp.access.numThrows == 0) {
				BallContainer.BallContainerSingleton.ballContainer.Remove(possessedBall);
			}
			else {
				unlimitedBallPowerUp.access.numThrows++;
			}
			Invoke ("callMakeNewBall", 0.1f);
		}
	}

	public void PickUpBall(GameObject ball){
		ball.transform.position = this.transform.position;
		this.rigidbody.velocity = Vector3.zero;
		isBallPossessed = true;
		ball.rigidbody.collider.isTrigger = true;
	}

	void BarrelRoll(){
		barrelRoll = true;
	}

	//React to getting hit by a ball
	public void HitByBall(){
//		possessedBall = null;
//		possession = false;
		playerHealth.HitByEnemyBall ();
		justHit = true;
		if (playerHealth.getCurrentLife() == 0)
			Destroy (this.gameObject);
		stunPlayer ();
	}

	void callMakeNewBall() {
		unlimitedBallPowerUp.access.makeNewBall ();
	}

	//Returns whether the player is blinking meaning they were just hit and invulnerable
	public bool isPlayerBlinking(){
		return justHit;
	}


}

