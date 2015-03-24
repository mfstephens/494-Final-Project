using UnityEngine;
using System.Collections;
using InControl;
using System;

	public class PlayerMovement : MonoBehaviour {

		public float speed = 10.0f;
		public float stunnedSpeed = 2.0f;
		public float jumpSpeed;
		public float jumpShortSpeed;
		public float dropSpeed = -150f;

		private Animator animator;
		private Transform ceilingCheck;
		[SerializeField] private LayerMask whatIsCeiling;
		private float ceilingRadius = 1.25f;

		private bool isHeadHittingCeiling = false;
		private bool doubleJump = false;
		private bool isOnGround = false;
		private bool isOnLeftWall = false;
		private bool isOnRightWall = false;
		private bool groundPound = false;
		private bool inGroundPound = false;
		private bool flickDown = false;
		private bool fallDown = false;
		private bool jump = false;
		private bool jumpCancel = false;
		private float flickDownTime = -5f;

		private InputDevice playerControl;
		private catchAndThrow throwControl;

		// Use this for initialization
		void Start () {
			animator = GetComponent<Animator> ();
			ceilingCheck = transform.FindChild ("Head");
			throwControl = GetComponent<catchAndThrow> ();
			gameObject.SetActive (false);
			speed = 0;
			jumpSpeed = 0;
		}
		
		// Update is called once per frame
		void Update () {

			if (animator.GetBool("GroundPounded")==true) {
				return;
			}

			float move = playerControl.LeftStickX;
			float verticalMove = playerControl.LeftStickY;

			//Record time user flicks the analog stick down ("make sure they're not trying to move horizontally and that they are on the ground)
			if (isOnGround) {
				if (verticalMove < 0 && move < .2 && move > -.2)
					flickDownTime = Time.time;
				else if (verticalMove == 0 && move == 0) {
					if (Time.time - flickDownTime <= .2) 
						flickDown = true;
				}
			}
			else{
				if(verticalMove<0 && !isOnGround)
					fallDown = true;
			}

			//Check if player is hitting ceiling
			isHeadHittingCeiling = Physics.CheckSphere (ceilingCheck.position, ceilingRadius, whatIsCeiling);
			if (isHeadHittingCeiling) {
				this.GetComponent<Collider>().isTrigger = true;
			}

			if (playerControl.Action3.WasPressed)
				throwControl.attemptThrow();

			if (playerControl.Action1.WasPressed)
				jump = true;
			if (playerControl.Action1.WasReleased)
				jumpCancel = true;

			//Ground Pound
			if (playerControl.LeftStickY < 0 && playerControl.Action2.WasPressed)
				groundPound = true;

			//Can not punch if you have the ball or in midst of ground pound
			if (playerControl.Action2.WasPressed && !groundPound && !throwControl.possesion) {
				animator.SetTrigger ("Punch");
				print ("Distance "+Vector3.Distance(ballBehavior.ball.gameObject.transform.position, this.gameObject.transform.position));
				if ((Vector3.Distance(ballBehavior.ball.gameObject.transform.position, this.gameObject.transform.position) < 10) && ballBehavior.ball.possessed && (ballBehavior.ball.owner != null) && (ballBehavior.ball.owner != this.gameObject)) {
					throwControl.dropBall();
				}
			}

		}
		
		void FixedUpdate() {

			float move = playerControl.LeftStickX;
			float verticalMove = playerControl.LeftStickY;

			if (groundPound) {
				inGroundPound = true;
				GetComponent<Rigidbody>().velocity=new Vector2(0,0);
				GetComponent<Rigidbody>().useGravity=false;
				Invoke ("GroundPound",.3f);
				return;
			}
			
			//Player cant keep running into the wall causing clipping and stuck in animation once you are in ground pound
			if ((move < 0 && isOnLeftWall || move>0 && isOnRightWall) || inGroundPound) {
				return;
			}

			//Flip animation if character turns
			if (move > 0 && transform.localScale.x < 0)
				Flip ();
			if(move<0 && transform.localScale.x > 0)
				Flip();

			//Apply horizontal velocity
			if (move != 0) {
				GetComponent<Rigidbody>().velocity = new Vector2 (move * speed, GetComponent<Rigidbody>().velocity.y);
			}
			else {
				GetComponent<Rigidbody>().velocity = new Vector2 (0, GetComponent<Rigidbody>().velocity.y);
			}

			if (fallDown) {
				GetComponent<Rigidbody>().AddForce(new Vector3(0,dropSpeed,0));
				fallDown = false;
			}


			if (jump) {

				if(isOnGround){
					GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x,jumpSpeed);
					doubleJump = true;
				}
				else if (doubleJump){
					doubleJump=false;
					GetComponent<Rigidbody>().velocity=new Vector2(GetComponent<Rigidbody>().velocity.x,0);
					GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x,jumpSpeed);
				}
				jump = false;
			}

			if (jumpCancel) {
				if(GetComponent<Rigidbody>().velocity.y > jumpShortSpeed)
					GetComponent<Rigidbody>().velocity = new Vector2(GetComponent<Rigidbody>().velocity.x,jumpShortSpeed);
				jumpCancel = false;
			}

		}

		void GroundPound(){
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<Rigidbody>().velocity = new Vector2 (0, -200f);
			groundPound = false;
		}

		void StunnedFromGroundPound(){
			animator.SetBool ("GroundPounded", false);
		}

		void Flip(){
			Vector3 local = transform.localScale;
			local.x *= -1;
			transform.localScale = local;
		}

		public void setController(int index){
			playerControl = InputManager.Devices [index];
		}

		void OnCollisionEnter(Collision collision){

			if (collision.gameObject.CompareTag ("Platform")) {
				if(!isHeadHittingCeiling){
					inGroundPound = false;
					isOnGround = true;
					doubleJump = true;
				}
			}

			if (collision.gameObject.CompareTag ("LeftWall")){
				doubleJump = true;
				isOnLeftWall = true;
			}

			if (collision.gameObject.CompareTag ("RightWall")) {
				doubleJump = true;
				isOnRightWall = true;
			}

			if (collision.gameObject.CompareTag ("Base")) {
				inGroundPound = false;
				isOnGround = true;
				doubleJump = true;
			}

			if (collision.gameObject.CompareTag ("Player")) {

				catchAndThrow opponent = collision.gameObject.GetComponent<catchAndThrow>();
				PlayerMovement opponentMovement = collision.gameObject.GetComponent<PlayerMovement>();

				//Teammate does nothing to you right know when ground pounded
				if(opponent.teamNumber == throwControl.teamNumber){
					return;
				}

				if(opponentMovement.animator.GetCurrentAnimatorStateInfo(0).IsName("Punch")){
					print ("Punched....IN THE FACE");

					/*if(throwControl.possesion){
						throwControl.dropBall();
					}*/

					/*
					if(opponentMovement.rigidbody.velocity.x > rigidbody.velocity.x)
						rigidbody.AddForce(new Vector3(transform.localPosition.x * 500,0,0));
					if(opponentMovement.rigidbody.velocity.x < rigidbody.velocity.x)
						opponentMovement.rigidbody.AddForce(new Vector3(transform.localPosition.x*500,0,0));
					*/
				}
				
				//Check if you're on ground and opponent is in act of ground pounding
				if(isOnGround && opponentMovement.inGroundPound){
					//If you have the ball, it gets released in a random direction
					if(throwControl.possesion){
						throwControl.randomBallVelocity();
					}

					//animator.SetBool("GroundPounded",true);
					//Invoke ("StunnedFromGroundPound",2f);
				}

			}

		}

		void OnCollisionStay(Collision collision){

			if (collision.gameObject.CompareTag ("LeftWall") || collision.gameObject.CompareTag("RightWall")) {
				GetComponent<Rigidbody>().velocity=new Vector2(0,GetComponent<Rigidbody>().velocity.y);
			}

			if (collision.gameObject.CompareTag ("Platform")) {
				if(flickDown){
					this.GetComponent<Collider>().isTrigger = true;
					GetComponent<Rigidbody>().velocity=new Vector2(GetComponent<Rigidbody>().velocity.x,-100f);
				}
			}

			if (collision.gameObject.CompareTag ("Base")) {
				flickDown=false;
			}

			if(collision.gameObject.CompareTag("Player")){
				print ("Get off me bro");	
			}
		}

		void OnCollisionExit(Collision collision){
			if (collision.gameObject.CompareTag ("Platform")) {
				isOnGround=false;
			}
			if(collision.gameObject.CompareTag("LeftWall")){
				isOnLeftWall=false;
			}
			if (collision.gameObject.CompareTag ("RightWall")) {
				isOnRightWall = false;
			}
			if (collision.gameObject.CompareTag ("Base")) {
				isOnGround = false;
			}
		}

		void OnTriggerEnter(Collider collider){
		if (collider.gameObject.CompareTag ("Base"))
			this.GetComponent<Rigidbody>().GetComponent<Collider>().isTrigger = false;

		}

		void OnTriggerExit(Collider colliderObject){
			if (colliderObject.gameObject.CompareTag ("Platform")) {
				this.GetComponent<Collider>().isTrigger = false;
				flickDown = false;
			}
		}

		//Just for testing to see the circle that detects ceiling hit
		void OnDrawGizmos(){
			if (ceilingCheck != null) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere (ceilingCheck.position, ceilingRadius);
			}
		}

		public float getLeftStickX() {
			return playerControl.LeftStickX;
		}

		public float getLeftStickY() {
			return playerControl.LeftStickY;
		}

		//Allows the players to move because the game is active now
		public void StartGame(){
			jumpSpeed = 125f;
			jumpShortSpeed = 62.5f;
			speed = 125f;
		}
	}