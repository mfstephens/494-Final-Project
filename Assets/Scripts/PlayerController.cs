using UnityEngine;
using System.Collections;
using InControl;

public class PlayerController : MonoBehaviour {

	public float throwSpeed;
	public float stunnedDuration;
	public float rotateSpeed;

	private float timeHit;
	private float rotateStart;
	private bool jump = false;
	private bool jumpCancel = false;
	private bool possession = false;
	private bool speedBoost = false;
	private bool justHit = false;
	private bool playerStunned = false;
	private bool barrelRoll = false;

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
		if (name == "Player1")
			playerControl = InputManager.Devices [0];
		if (name == "Player2")
			playerControl = InputManager.Devices [1];

		playerColor = this.renderer.material.color;

		PickUpZone = transform.Find ("PickUpBallZone").gameObject;
		PickUpZone.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (barrelRoll) {

			if(!playerMovement.isPlayerOnGround())
				transform.Rotate(Vector3.forward,360f);
			//barrelRoll = false;
		}

		//Characters blinks when they were hit for stunnedDuration (can customize to stun, add more effects)
		if (justHit) {
			if(timeHit + stunnedDuration <= Time.time){
				CancelBlink();
				this.renderer.material.color = playerColor;
				justHit = false;
			}
		}

		if (CanBallBePickedUp () && !possession)
			PickUpZone.SetActive (true);
		else
			PickUpZone.SetActive (false);


		if (possession)
			possessedBall.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,-5f);

		if (playerControl.Action1.WasPressed)
			jump = true;

		else if (playerControl.Action1.WasReleased)
			jumpCancel = true;

		else if (playerControl.Action3.WasPressed) {
			if (!possession)
				PickUpBall ();
			else
				ThrowBall ();
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

	bool CanBallBePickedUp(){
		Ball closestBall = BallContainer.BallContainerSingleton.closestBallToPosition (this.transform.position);
		float closestBallDistance = Vector3.Distance (this.transform.position, closestBall.transform.position);

		if (closestBall != null && closestBallDistance < 15f)
			return true;
		return false;
	}
	
	void PickUpBall(){
		Ball closestBall = BallContainer.BallContainerSingleton.closestBallToPosition (this.transform.position);
		float closestBallDistance = Vector3.Distance (this.transform.position, closestBall.transform.position);

		//If the ball (if any) is close, pick up
		if (closestBall != null && closestBallDistance < 15f) {
			closestBall.rigidbody.collider.isTrigger = true;
			closestBall.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,-5f);
			closestBall.ballPickedUpBy(gameObject.name);
			possession = true;
			possessedBall = closestBall;
		}
	}

	//Throw ball in direction of left stick
	void ThrowBall(){
		float horizontalMovement = playerControl.LeftStickX;
		float verticalMovement = playerControl.LeftStickY;

		possession = false;

		possessedBall.transform.position += new Vector3(horizontalMovement, verticalMovement, 10);
		
		possessedBall.rigidbody.velocity = new Vector2 (throwSpeed * horizontalMovement, verticalMovement*throwSpeed);
		possessedBall.rigidbody.collider.isTrigger = false;

		possessedBall.ballThrown ();
	}

	void BarrelRoll(){
		rotateStart = Time.time;
		barrelRoll = true;
	}

	//returns the ball in possession of (could be null)
	public Ball BallPossessed(){
		return possessedBall;
	}

	//React to getting hit by a ball
	public void HitByBall(){
		possessedBall = null;
		possession = false;
		justHit = true;
		stunPlayer ();
	}
}

