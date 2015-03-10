using UnityEngine;
using System.Collections;
using InControl;
using System;

namespace CustomProfileExample{
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
				this.collider.isTrigger = true;
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

			if (playerControl.Action2.WasPressed && !groundPound)
				animator.SetTrigger("Punch");

		}
		
		void FixedUpdate() {

			float move = playerControl.LeftStickX;
			float verticalMove = playerControl.LeftStickY;

			if (groundPound) {
				inGroundPound = true;
				rigidbody.velocity=new Vector2(0,0);
				rigidbody.useGravity=false;
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
				rigidbody.velocity = new Vector2 (move * speed, rigidbody.velocity.y);
			}
			else {
				rigidbody.velocity = new Vector2 (0, rigidbody.velocity.y);
			}

			if (fallDown) {
				rigidbody.AddForce(new Vector3(0,dropSpeed,0));
				fallDown = false;
			}


			if (jump) {

				if(isOnGround){
					rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed);
					doubleJump = true;
				}
				else if (doubleJump){
					doubleJump=false;
					rigidbody.velocity=new Vector2(rigidbody.velocity.x,0);
					rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpSpeed);
				}
				jump = false;
			}

			if (jumpCancel) {
				if(rigidbody.velocity.y > jumpShortSpeed)
					rigidbody.velocity = new Vector2(rigidbody.velocity.x,jumpShortSpeed);
				jumpCancel = false;
			}

		}

		void GroundPound(){
			rigidbody.useGravity = true;
			rigidbody.velocity = new Vector2 (0, -200f);
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
				PlayerMovement opponent = collision.gameObject.GetComponent<PlayerMovement>();

				//Check if you're on ground and opponent is in act of ground pounding
				if(isOnGround && opponent.groundPound){
					animator.SetBool("GroundPounded",true);
					Invoke ("StunnedFromGroundPound",2f);
				}
			}

		}

		void OnCollisionStay(Collision collision){

			if (collision.gameObject.CompareTag ("LeftWall") || collision.gameObject.CompareTag("RightWall")) {
				rigidbody.velocity=new Vector2(0,rigidbody.velocity.y);
			}

			if (collision.gameObject.CompareTag ("Platform")) {
				if(flickDown){
					this.collider.isTrigger = true;
					rigidbody.velocity=new Vector2(rigidbody.velocity.x,-100f);
				}
			}

			if (collision.gameObject.CompareTag ("Base")) {
				flickDown=false;
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

		void OnTriggerExit(Collider colliderObject){
			if (colliderObject.gameObject.CompareTag ("Platform")) {
				this.collider.isTrigger = false;
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
	}
}