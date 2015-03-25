using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour {
	
	public float throwSpeed;
	public float stunnedDuration;
	public float rotateSpeed;
	
	private float totalRotation = 0;
	private float timeHit;
	private float initialFlickDownTime = -5f;
	private int playerNumber;
			
	private bool jump = false;
	private bool jumpCancel = false;
	private bool speedBoost = false;
	private bool justHit = false;
	private bool playerStunned = false;
	private bool barrelRoll = false;
	private bool lockPosition = false;
	private bool dropThroughPlatform = false;
	
	public InputDevice playerControl;
	private PlayerMove playerMovement;

	public Ball possessedBall;
	public bool isBallPossessed = false;

	private Color playerColor;
	public Color beginColor;
	public Color endColor;
	
	// Use this for initialization
	void Start () {
		playerMovement = GetComponent<PlayerMove> ();
		int temp = InputManager.Devices.Count;
		print (temp);

		print (InputManager.Devices [0].Name);
		//playerControl = InputManager.Devices [1];

		//playerControl = InputManager.ActiveDevice;


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

		PickUpBall (possessedBall.gameObject);
		Physics.IgnoreCollision (this.gameObject.GetComponent<Collider> (), possessedBall.gameObject.GetComponent<Collider> ());
		playerColor = this.GetComponent<Renderer>().material.color;
		
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
				CancelBlink();
				this.GetComponent<Renderer>().material.color = playerColor;
				justHit = false;
			}
		}
		
		//Change Z Axis of ball when possessed to appear in front of you
		if (isBallPossessed) {
			possessedBall.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
		}
		
		//Check if player flicked down on joystick so they can drop through platform
		if (playerMovement.isPlayerOnPlatform()) {
			if (verticalMovement < -.5 && horizontalMovement < .2 && horizontalMovement > -.2)
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
				ThrowBall ();
			}
			else {
				possessedBall.findPlayerAndReturn();
			}
		} 
		else if (playerControl.RightTrigger.WasPressed) {
			speedBoost = true;
		}
		else if (playerControl.Action4.WasPressed)
			BarrelRoll ();
		
		if (playerControl.LeftTrigger.IsPressed && isBallPossessed) {
			Vector3 shotDirection3D = new Vector3 ( playerControl.LeftStickX,  playerControl.LeftStickY, 1f);
			lockPosition = true;
		}
		
		if (playerControl.Action1.WasReleased)
			jumpCancel = true;
		if (playerControl.LeftTrigger.WasReleased) {
			lockPosition = false;
		}
	}
	
	void FixedUpdate(){
		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;

		//TODO: person in charge of animations 
		if (horizontalMovement > 0 && transform.eulerAngles.y == 180)
			Flip();
		if(horizontalMovement < 0 && transform.eulerAngles.y == 0)
			Flip();
		
		playerMovement.Movement (horizontalMovement, verticalMovement, jump, jumpCancel,speedBoost,lockPosition,dropThroughPlatform);
		
		jump = false;
		jumpCancel = false;
		speedBoost = false;
		dropThroughPlatform = false;
	}
	
	void Flip(){
		transform.eulerAngles += new Vector3 (0, 180f, 0);
	}
	
	void stunPlayer(){
		timeHit = Time.time;
		this.GetComponent<Renderer>().material.color = beginColor;
		InvokeRepeating ("Blink",.1f, .25f);
	}
	
	void Blink(){
		if(this.GetComponent<Renderer>().material.color == beginColor)
			this.GetComponent<Renderer>().material.color = endColor;
		else
			this.GetComponent<Renderer>().material.color = beginColor;
	}
	
	void CancelBlink(){
		CancelInvoke ();
	}
	
	//Throw ball in direction of left stick
	void ThrowBall(){
		
		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;
		
		isBallPossessed = false;

		Vector2 throwMovement = new Vector2 (horizontalMovement, verticalMovement);
		throwMovement.Normalize ();
	
		possessedBall.gameObject.GetComponent<TrailRenderer> ().enabled = true;
		possessedBall.GetComponent<Rigidbody>().velocity = new Vector2 (throwSpeed * throwMovement.x, throwMovement.y*throwSpeed);
	}
	
	public void PickUpBall(GameObject ball){
		ball.transform.position = this.transform.position;
		this.GetComponent<Rigidbody>().velocity = Vector3.zero;
		isBallPossessed = true;
		possessedBall.gameObject.GetComponent<TrailRenderer> ().enabled = false;

	}
	
	void BarrelRoll(){
		barrelRoll = true;
	}
	
	//React to getting hit by a ball
	public void HitByBall() {
		justHit = true;

		// for when you are playing capture the flag
		if(Application.loadedLevelName.Equals("CaptureTheFlag")) {
			CaptureTheFlagMode.access.returnPlayer(this.gameObject);
		}
	}
	
		
	//Returns whether the player is blinking meaning they were just hit and invulnerable
	public bool isPlayerBlinking(){
		return justHit;
	}
	
	
}

