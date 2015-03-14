using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour {

	public float throwSpeed;
	public float stunnedDuration;
	public float rotateSpeed;

	private float totalRotation = 0;
	private float timeHit;
	private int playerNumber;

	private bool jump = false;
	private bool jumpCancel = false;
	private bool possession = false;
	private bool speedBoost = false;
	private bool justHit = false;
	private bool playerStunned = false;
	private bool barrelRoll = false;
	private bool justThrown = false;

	private InputDevice playerControl;
	private PlayerMove playerMovement;
	private Ball possessedBall;
	private GameObject PickUpZone;

	private Color playerColor;
	public Color beginColor;
	public Color endColor;
	
	// Use this for initialization
	void Start () {
		playerControl = InputManager.ActiveDevice;
		playerMovement = GetComponent<PlayerMove> ();
		if (name == "Player1") {
			playerControl = InputManager.Devices [0];
			playerNumber = 1;
		}
		if (name == "Player2") {
			playerControl = InputManager.Devices [1];
			playerNumber = 2;
		}
		if (name == "Player3") {
			playerControl = InputManager.Devices [2];
			playerNumber = 3;
		}

		playerColor = this.renderer.material.color;

		PickUpZone = transform.Find ("PickUpBallZone").gameObject;
		PickUpZone.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {


		if (Vector3.Distance (possessedBall.transform.position, this.transform.position) > 15f) {
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

		//Turn on or off the halo to indicate if ball can be picked up
//		if (CanBallBePickedUp () && !possession) {
//			PickUpZone.SetActive (true);
//			PickUpBall ();
//		}
//		else
//			PickUpZone.SetActive (false);

		//Change Z Axis of ball when possessed to appear in front of you
		if (possession)
			possessedBall.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,5f);

		if (playerControl.Action1.WasPressed)
			jump = true;

		else if (playerControl.Action1.WasReleased)
			jumpCancel = true;

		else if (playerControl.Action3.WasPressed) {
//			if (!possession && CanBallBePickedUp()) {
//				PickUpBall ();
//			}
			if (possession) {
				ThrowBall ();
			}
		} 

		else if (playerControl.RightTrigger.WasPressed)
			speedBoost = true;

		else if (playerControl.Action4.WasPressed)
			BarrelRoll ();
	}

	void FixedUpdate(){
		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;

		if (horizontalMovement > 0 && transform.localScale.x < 0)
			Flip ();
		if(horizontalMovement<0 && transform.localScale.x > 0)
			Flip();

		playerMovement.Movement (horizontalMovement, verticalMovement, jump, jumpCancel,speedBoost);

		jump = false;
		jumpCancel = false;
		speedBoost = false;
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

	public bool CanBallBePickedUp(){
		Ball closestBall = BallContainer.BallContainerSingleton.closestBallToPosition (this.transform.position);
		float closestBallDistance = Vector3.Distance (this.transform.position, closestBall.transform.position);

		if (closestBall != null && closestBallDistance < 15f && closestBall.playerColor == playerNumber && !justThrown) {
			print ("Player number: " + playerNumber);
			print ("Player color: " + closestBall.playerColor);
			return true;
		}
		return false;
	}

	public void PickUpBall(){
		print ("attempting to pick up bal");
		Ball closestBall = BallContainer.BallContainerSingleton.closestBallToPosition (this.transform.position);

		closestBall.rigidbody.collider.isTrigger = true;
		//closestBall.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,-5f);
		closestBall.ballPickedUpBy(gameObject.name);
		possession = true;
		possessedBall = closestBall;
	}

	//Throw ball in direction of left stick
	void ThrowBall(){

		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;

		possession = false;
		justThrown = true;
		//possessedBall.transform.position += new Vector3(horizontalMovement, verticalMovement, 10);
		
		possessedBall.rigidbody.velocity = new Vector2 (throwSpeed * horizontalMovement, verticalMovement*throwSpeed);
		possessedBall.rigidbody.collider.isTrigger = false;

		possessedBall.ballThrown ();
	}

	void BarrelRoll(){
		barrelRoll = true;
	}

	//returns the ball in possession of (could be null)
	public Ball BallPossessed(){
		return possessedBall;
	}

	//React to getting hit by a ball
	public void HitByBall(){
//		possessedBall = null;
//		possession = false;
		justHit = true;
		stunPlayer ();
	}
}

