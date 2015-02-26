using UnityEngine;
using System.Collections;
using InControl;
using System;

namespace CustomProfileExample{
	public class PlayerMovement : MonoBehaviour {

		public float speed = 10.0f;
		public float jump = 2.0f;
		public float initialJump = 20f;
		public float maxJumpHeight = 20f;
		public float dropSpeed = -150f;

		private Animator animator;
		private Transform ceilingCheck;
		[SerializeField] private LayerMask whatIsCeiling;
		private float ceilingRadius = 1.25f;
		private float initialJumpPlayerYPosition;
		private bool isHeadHittingCeiling = false;
		private bool doubleJump = false;
		private bool isOnGround = false;
		private bool isOnWall = false;
		private bool inGroundPound = false;
		private bool flickDown = false;
		private float flickDownTime = -5f;
		private InputDevice playerControl;
		private catchAndThrow throwControl;

		// Use this for initialization
		void Start () {
			animator = GetComponent<Animator> ();
			ceilingCheck = transform.FindChild ("Head");
			throwControl = GetComponent<catchAndThrow> ();
			if (!ceilingCheck) {
				Debug.LogError("Head is null");
			}
			if (!throwControl) {
				Debug.LogError("Cant find catch script");
			}
			gameObject.SetActive (false);
		}
		
		// Update is called once per frame
		void Update () {

			//Check if player is hitting ceiling
			isHeadHittingCeiling = Physics.CheckSphere (ceilingCheck.position, ceilingRadius, whatIsCeiling);
			Collider[] colliders = Physics.OverlapSphere (ceilingCheck.position, ceilingRadius, whatIsCeiling);
			if (colliders.Length > 0) {
				colliders[0].isTrigger=true;
			}

			if (playerControl.Action3.WasPressed) {
				throwControl.attemptThrow();
			}

			if (playerControl.Action1.WasPressed) {
				if(isOnGround){
					initialJumpPlayerYPosition=transform.position.y;
					print (initialJumpPlayerYPosition);
					rigidbody.AddForce(new Vector3(0,initialJump,0));
					doubleJump = true;
					return;
				}
				else if(doubleJump){
					doubleJump=false;
					initialJumpPlayerYPosition=transform.position.y;
					rigidbody.velocity=new Vector2(rigidbody.velocity.x,0);
					rigidbody.AddForce(new Vector3(0,initialJump,0));
					return;
				}

			}
			else if (playerControl.Action1.IsPressed) {
				if(rigidbody.velocity.y<=0 || isOnGround){
					return;
				}
				if(transform.position.y<initialJumpPlayerYPosition+maxJumpHeight){
					rigidbody.AddForce(new Vector3(0,jump,0));
				}
			}
			else if (playerControl.Action1.WasReleased) {

			}

			//Ground Pound
			if (playerControl.LeftStickY < 0 && playerControl.Action2.WasPressed) {
				inGroundPound=true;
				rigidbody.velocity=new Vector2(0,0);
				rigidbody.useGravity=false;
				Invoke ("GroundPound",.3f);
				return;
			}
			if (playerControl.Action2.WasPressed) {
				animator.SetTrigger("Punch");
			}

		}
		
		void FixedUpdate() {

			//Player cannot move if on the wall
			float move = playerControl.LeftStickX;
			float verticalMove = playerControl.LeftStickY;

			if (move < 0 && isOnWall || inGroundPound) {
				return;
			}
			if (move != 0) {
				rigidbody.velocity = new Vector2 (move * speed, rigidbody.velocity.y);
			}
			else {
				rigidbody.velocity = new Vector2 (0, rigidbody.velocity.y);
			}

			if (move > 0 && transform.localScale.x < 0)
				Flip ();
			if(move<0 && transform.localScale.x > 0)
		   		Flip();

			//Record time user flicks the analog stick down ("make sure they're not trying to move horizontally and that they are on the ground)
			if (isOnGround) {
				if (verticalMove < 0 && move < .2 && move > -.2) {
					flickDownTime = Time.time;
				}
				if (verticalMove == 0 && move == 0) {
					if (Time.time - flickDownTime <= .2) {
						flickDown = true;
					}
				}
			}
			else {
				if(verticalMove<0 && !isOnGround){
					rigidbody.AddForce(new Vector3(0,dropSpeed,0));
				}
			}
		}

		void GroundPound(){
			rigidbody.useGravity = true;
			rigidbody.velocity = new Vector2 (0, -200f);
		}

		void Flip(){
			Vector3 local = transform.localScale;
			local.x *= -1;
			transform.localScale = local;
		}

		public float movementX(){
			return playerControl.LeftStickX;
		}

		public float movementY(){
			return playerControl.LeftStickX;
		}

		public void setController(int index){
			print (index);
			playerControl = InputManager.Devices [index];
		}

		void OnCollisionEnter(Collision collision){
			if (collision.gameObject.CompareTag ("Platform")) {
				if(isHeadHittingCeiling){
					collision.gameObject.collider.isTrigger = true;
				}
				else{
					inGroundPound = false;
					isOnGround = true;
					doubleJump = true;
				}
			}

			if (collision.gameObject.CompareTag ("Wall")) {
				doubleJump = true;
				isOnWall = true;
			}

			if (collision.gameObject.CompareTag ("Base")) {
				inGroundPound = false;
				isOnGround = true;
				doubleJump = true;
			}

		}

		void OnCollisionStay(Collision collision){

			if (collision.gameObject.CompareTag ("Wall")) {
				rigidbody.velocity=new Vector2(0,rigidbody.velocity.y);
			}

			if (collision.gameObject.CompareTag ("Platform")) {
				if(flickDown){
					collision.gameObject.collider.isTrigger = true;
					rigidbody.velocity=new Vector2(rigidbody.velocity.x,-100f);
					return;
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
			if(collision.gameObject.CompareTag("Wall")){
				isOnWall=false;
			}

			if (collision.gameObject.CompareTag ("Base")) {
				isOnGround = false;
			}
		}

		void OnTriggerExit(Collider collider){
			if (collider.gameObject.CompareTag ("Platform")) {
				collider.isTrigger = false;
				flickDown = false;
			}
		}

		void OnDrawGizmos(){
			if (ceilingCheck != null) {
				Gizmos.color = Color.yellow;
				Gizmos.DrawSphere (ceilingCheck.position, ceilingRadius);
			}
		}
	}
}