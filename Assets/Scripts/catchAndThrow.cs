using UnityEngine;
using System.Collections;
using InControl;

	public class catchAndThrow : MonoBehaviour {

		public GameObject ball;
		public GameObject teammate;
		public float throwSpeed;

		public bool possesion = false;
		private bool justThrown = false;

		public short teamNumber;

		catchAndThrow teamMateThrow;
		ballBehavior ballScript;
		PlayerMovement playerMove;

		// Use this for initialization
		void Start () {
			teamMateThrow = teammate.GetComponent<catchAndThrow> ();
			ballScript = ball.GetComponent<ballBehavior> ();
			playerMove = this.GetComponent<PlayerMovement> ();

			if (name == "Cartman" || name == "Kenny")
				teamNumber = 1;
			else
				teamNumber = 2;
		}

		void Update(){
			if (possesion)
				GameController.gameController.UpdateTeamIDWithBall (teamNumber);
			else if (!ballBehavior.ball.isBallPossessed ())
				GameController.gameController.UpdateTeamIDWithBall (-1);
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

		public void dropBall(){

			GameObject.Find ("Cartman").GetComponent<catchAndThrow> ().possesion = false;
			GameObject.Find ("Kenny").GetComponent<catchAndThrow> ().possesion = false;
			GameObject.Find ("Kyle").GetComponent<catchAndThrow> ().possesion = false;
			GameObject.Find ("Stan").GetComponent<catchAndThrow> ().possesion = false;

			GameObject.Find ("Cartman").GetComponent<catchAndThrow> ().justThrown = true;
			GameObject.Find ("Kenny").GetComponent<catchAndThrow> ().justThrown = true;
			GameObject.Find ("Kyle").GetComponent<catchAndThrow> ().justThrown = true;
			GameObject.Find ("Stan").GetComponent<catchAndThrow> ().justThrown = true;


		float ballX = Random.Range (-.4f, .4f);
			float ballY = 1f;
			
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

		public void randomBallVelocity(){
			justThrown = true;
			possesion = false;

			float ballX = Random.Range (-1f, -.5f);
			float ballY = Random.Range (.5f, 1f);

			// make sure the ball isn't lagging
			ball.transform.position = this.transform.position;
			ball.transform.position += new Vector3(ballX, ballY, this.transform.position.z);
			
			ball.rigidbody.useGravity = true;
			ballScript.owner = null;
			ballScript.possessed = false;
			
			ball.rigidbody.AddForce (throwSpeed * ballX, throwSpeed * ballY,0);
		}

		void OnCollisionEnter(Collision other) {
			if (other.gameObject.name.Equals ("KennySprite_2") || other.gameObject.name.Equals ("KennySprite_3")) {
				//TODO: collision when you have the ball?
			}
		}

	}
