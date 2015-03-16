using UnityEngine;
using System.Collections;

public class PlayerMove : MonoBehaviour {

	public float speed;
	public float jumpSpeed;
	public float jumpShortSpeed;
	public float dropSpeed;
	public float wallJump;

	private bool barrelRoll = false;
	private bool isOnGround = false;
	private bool doubleJump = false;
	private bool isOnLeftWall = false;
	private bool isOnRightWall = false;
	private PlayerController playerController;

	// Use this for initializationx
	void Start () {
		playerController = this.GetComponent<PlayerController> ();
	}

	void Update() {
		if (playerController.CanBallBePickedUp()) {
			print ("ADFSDFSDFSDFSDFSDF");
			playerController.PickUpBall();
		}
	}

	public void Movement(float moveX, float moveY, bool jump, bool cancelJump, bool speedBoost){
		rigidbody.velocity = new Vector2 (moveX * speed, rigidbody.velocity.y);

		if (jump) {
			print (this.name + jump);
			if (isOnGround)
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
			else if (doubleJump) {
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpSpeed);
				doubleJump = false;
			}
		}
		else if (cancelJump) {
			if (rigidbody.velocity.y > jumpShortSpeed)
				rigidbody.velocity = new Vector2 (rigidbody.velocity.x, jumpShortSpeed);
		} 
		else if (speedBoost)
			rigidbody.AddForce(new Vector3(transform.localScale.x*50000,0,0));
	}

	public bool isPlayerOnGround(){
		return isOnGround;
	}

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.CompareTag("Ground")) {
			isOnGround = true;
			doubleJump = true;
		}

		if (collision.gameObject.CompareTag ("LeftWall"))
			isOnLeftWall = true;



		//If Hit by ball that is not in your possession
		if (playerController.BallPossessed()!= null && playerController.BallPossessed() != collision.gameObject) {
			if(collision.gameObject.CompareTag("Ball")){
				//If Ball was not thrown by you, get hit
				print ("got here");

				if(!collision.gameObject.GetComponent<Ball>().thrownByPlayer(name)){
					print (collision.gameObject.rigidbody.velocity);
//					BallContainer.BallContainerSingleton.destroyBall(playerController.BallPossessed());
					playerController.HitByBall();
				}

			}

		}
	}

	void OnCollisionExit(Collision collision){
		if (collision.gameObject.CompareTag ("Ground"))
			isOnGround = false;

		if (collision.gameObject.CompareTag ("LeftWall"))
			isOnLeftWall = false;

		if (collision.gameObject.CompareTag ("RightWall"))
			isOnRightWall = false;
	}
}
