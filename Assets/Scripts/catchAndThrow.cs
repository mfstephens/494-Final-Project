using UnityEngine;
using System.Collections;
using InControl;

public class catchAndThrow : MonoBehaviour {

	public GameObject ball;
	public GameObject teammate;
	//public GameObject opponent1, opponent2;
	public float orbitSpeed;

	public bool possesion = false;
	public bool isPassing = false;
	public bool isCatching = false;
	private bool deflected = false;
	float timeSoFar = 0;
	float timeToPass = 0.5f; // how long you want the pass to take.
	Vector3 startPosition = Vector3.zero;
	Vector3 finalPosition = Vector3.zero;
	catchAndThrow teamMateThrow;
	ballBehavior ballScript;
 
		
	// Use this for initialization
	void Start () {
		teamMateThrow = teammate.GetComponent<catchAndThrow> ();
		ballScript = ball.GetComponent<ballBehavior> ();
	}

	// Update is called once per frame
	void FixedUpdate () {  
		// catch ball
		if ((Vector3.Distance (ball.transform.position, this.transform.position) < 50) && !possesion && teamMateThrow.isPassing) {
			isCatching = true;
			controlBall();
		}
		// pick up ball
		else if ((Vector3.Distance (ball.transform.position, this.transform.position) < 20) && !ballScript.possessed && !teamMateThrow.isPassing) {
			controlBall();
		}

		// control ball flight during pass
		if (isPassing) {
			var percent = timeSoFar / timeToPass;
			ball.transform.position = Vector3.Lerp( startPosition, finalPosition, percent );
			timeSoFar += Time.deltaTime;
			if ( timeSoFar >= timeToPass ) {
				missedCatch();
			}
		}

		// player carrying ball
		if ((ballScript.owner != null) && ballScript.owner.gameObject.name.Equals(this.gameObject.name) && !isPassing) {

			if (ball.rigidbody.velocity == Vector3.zero) {
				ball.transform.position = this.transform.position;
			}
			else {
				ball.transform.position = Vector3.Lerp( ball.transform.position, this.transform.position, 0.5f );
			}
		}
	}

	void missedCatch() {
		isPassing = false;
		teamMateThrow.isCatching = false;
		ballScript.owner = null;
		ballScript.possessed = false;
		ball.rigidbody.useGravity = true;
		ball.collider.isTrigger = false;
		ball.rigidbody.velocity = (finalPosition - startPosition) * 2;
	}

	void controlBall() {
		ball.transform.rotation = Quaternion.identity;
		ball.rigidbody.velocity = Vector3.zero;
		possesion = true;
		teamMateThrow.isPassing = false;
		ballScript.possessed = true;
		ballScript.owner = this.gameObject;
		ball.collider.isTrigger = true;
		ball.rigidbody.useGravity = false;
	}

	void releaseBall() {
		isCatching = false;
		ball.collider.isTrigger = false;
		possesion = false;
		ball.rigidbody.useGravity = true;
		ballScript.possessed = false;
	}

	public void ballDropped() {
		ball.collider.isTrigger = false;
		possesion = false;
	}

	public void attemptThrow(){
		if (ballScript.owner.gameObject.name.Equals(this.gameObject.name)) {
			releaseBall();
			ball.rigidbody.useGravity = true;
			StartPass (teammate.transform.position);
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name.Equals ("KennySprite_2") || other.gameObject.name.Equals ("KennySprite_3")) {
			ballDropped();
		}
	}

	void StartPass( Vector3 finalPos ) {
		startPosition = ball.transform.position;
		finalPosition = teammate.transform.position;
		timeSoFar = 0;
		isPassing = true;
	}






}
