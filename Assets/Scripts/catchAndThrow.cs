using UnityEngine;
using System.Collections;
using InControl;


namespace CustomProfileExample{

	public class catchAndThrow : MonoBehaviour {

		public GameObject ball;
		public GameObject teammate;
		public float throwSpeed;

		public bool possesion = false;
		private bool justThrown = false;

		catchAndThrow teamMateThrow;
		ballBehavior ballScript;
		PlayerMovement playerMove;

		// Use this for initialization
		void Start () {
			teamMateThrow = teammate.GetComponent<catchAndThrow> ();
			ballScript = ball.GetComponent<ballBehavior> ();
			playerMove = this.GetComponent<PlayerMovement> ();
		}

		// Update is called once per frame
		void FixedUpdate () {  

			if (justThrown && (Vector3.Distance (ball.transform.position, this.transform.position) >= 15)) {
				justThrown = false;
			}
			else if (!justThrown && (Vector3.Distance (ball.transform.position, this.transform.position) < 15) && !ballScript.possessed) { //&& this.gameObject.name.Equals("Cartman")) {
				controlBall();
			}

			// player carrying ball
			if (possesion) {
				if (ball.rigidbody.velocity == Vector3.zero) {
					ball.transform.position = this.transform.position;
				}
				else {
					ball.transform.position = Vector3.Lerp( ball.transform.position, this.transform.position, 0.5f );
				}
			}
		}

		void controlBall() {
			ball.transform.rotation = Quaternion.identity;
			ball.rigidbody.velocity = Vector3.zero;
			possesion = true;
			ballScript.possessed = true;
			ballScript.owner = this.gameObject;
			ball.collider.isTrigger = true;
			ball.rigidbody.useGravity = false;
		}


		public void attemptThrow(){
			// if you currently have the ball
			if ((ballScript.owner != null) && (ballScript.owner == this.gameObject)) {
				float ballX = playerMove.getLeftStickX();
				float ballY = playerMove.getLeftStickY();

				justThrown = true;
				possesion = false;

				// make sure the ball isn't lagging
				ball.transform.position = this.transform.position;
				ball.transform.position += new Vector3(ballX, ballY, this.transform.position.z);

				ball.rigidbody.useGravity = true;
				ballScript.owner = null;
				ballScript.possessed = false;

				// give the ball some velocity
				ball.rigidbody.AddForce (ballX * throwSpeed, ballY * throwSpeed, 0);
			}
		}

		void OnCollisionEnter(Collision other) {
			if (other.gameObject.name.Equals ("KennySprite_2") || other.gameObject.name.Equals ("KennySprite_3")) {
				//TODO: collision when you have the ball?
			}
		}

	}

}
