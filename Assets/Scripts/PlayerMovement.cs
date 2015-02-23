using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 10.0f;
	public float jump = 2.0f;
	public float initialJump = 20f;
	public float maxJumpHeight = 20f;

	private Animator animator;
	private float initialJumpPlayerYPosition;
	private bool doubleJump = false;
	private bool isOnGround = false;
	private bool isOnWall = false;
	private bool inGroundPound = false;
	private bool flickDown = false;
	private float flickDownTime = -5f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
			if(isOnGround){
				initialJumpPlayerYPosition=transform.position.y;
				rigidbody.AddForce(new Vector3(0,initialJump,0));
				doubleJump = true;
			}
			else if(doubleJump){
				doubleJump=false;
				initialJumpPlayerYPosition=transform.position.y;
				rigidbody.velocity=new Vector2(rigidbody.velocity.x,0);
				rigidbody.AddForce(new Vector3(0,initialJump,0));
			}

		}
		if (Input.GetButton ("Jump")) {
			if(rigidbody.velocity.y<=0 || isOnGround){
				return;
			}
			if(transform.position.y<initialJumpPlayerYPosition+maxJumpHeight){
				rigidbody.AddForce(new Vector3(0,jump,0));
			}
		}
		if (Input.GetButtonUp ("Jump")) {

		}

		//Ground Pound
		if (Input.GetAxis("Vertical") < 0 && Input.GetButtonDown("Fight")) {
			inGroundPound=true;
			rigidbody.velocity=new Vector2(0,0);
			rigidbody.useGravity=false;
			Invoke ("GroundPound",.3f);
			return;
		}
		if (Input.GetButtonDown ("Fight")) {
			animator.SetTrigger("Punch");
		}

	}

	void FixedUpdate() {
		//Player cannot move if on the wall
		float move = Input.GetAxis ("Horizontal");
		float verticalMove = Input.GetAxis ("Vertical");

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
		if(isOnGround){
			if (verticalMove < 0 && move<.2 && move>-.2) {
				flickDownTime=Time.time;
			}
			if (verticalMove == 0 && move==0) {
				if(Time.time-flickDownTime<=.2){
					flickDown = true;
				}
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

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.CompareTag ("Platform")) {
			inGroundPound = false;
			isOnGround = true;
			doubleJump = true;
		}
		if (collision.gameObject.CompareTag ("Wall")) {
			doubleJump = true;
			isOnWall = true;
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
			}
		}
	}

	void OnCollisionExit(Collision collision){
		if (collision.gameObject.CompareTag ("Platform")) {
			isOnGround=false;
		}
		if(collision.gameObject.CompareTag("Wall")){
			isOnWall=false;
		}
	}

	void OnTriggerExit(Collider collider){
		if (collider.gameObject.CompareTag ("Platform")) {
			if(flickDown){
				collider.isTrigger = false;
				flickDown = false;
			}
		}
	}
}
