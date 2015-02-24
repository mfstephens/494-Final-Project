using UnityEngine;
using System.Collections;

public class catchAndThrow : MonoBehaviour {

	public GameObject ball;
	public GameObject teammate;
	public GameObject opponent1, opponent2;
	public float orbitSpeed;

	public bool possesion = false;
	private bool isPassing = false;
	float timeSoFar = 0;
	float timeToPass = 0.5f; // how long you want the pass to take.
	Vector3 startPosition = Vector3.zero;
	Vector3 finalPosition = Vector3.zero;
		
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {  
		if ((Vector3.Distance (ball.transform.position, this.transform.position) < 10) && !possesion) {
			controlBall();
		}
		else if ((Vector3.Distance (ball.transform.position, this.transform.position) >= 10) && possesion) {
			releaseBall();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			ball.rigidbody.useGravity = true;
			if (possesion) {
				StartPass( teammate.transform.position );
			}
		}

		if (isPassing) {
			var percent = timeSoFar / timeToPass;
			ball.transform.position = Vector3.Lerp( startPosition, finalPosition, percent );
			timeSoFar += Time.deltaTime;
			if ( timeSoFar >= timeToPass ) {
				isPassing = false;
			}
		}

		if (possesion) {
			ball.transform.RotateAround(this.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
		}
	}

	void releaseBall() {
		ball.collider.isTrigger = false;
		possesion = false;
		ball.rigidbody.useGravity = true;
	}

	public void ballDropped() {
		ball.collider.isTrigger = false;
		possesion = false;
		ball.rigidbody.useGravity = true;
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name.Equals ("KennySprite_2") || other.gameObject.name.Equals ("KennySprite_3")) {
			ballDropped();
		}
	}

	void controlBall() {
		ball.GetComponent<ballBehavior> ().possessed = true;
		print ("setting owner");
		ball.GetComponent<ballBehavior> ().owner = this.gameObject;
		possesion = true;
		ball.collider.isTrigger = true;
		ball.rigidbody.useGravity = false;
		ball.rigidbody.velocity = Vector3.zero;
		//ball.transform.position = this.transform.position + new Vector3 (10f, 0, 0);
	}

	void StartPass( Vector3 finalPos ) {
		startPosition = ball.transform.position;
		finalPosition = teammate.transform.position;
		timeSoFar = 0;
		isPassing = true;
	}






}
