using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BallType {
	Standard,
	InfinitePowerup
}

public class Ball : MonoBehaviour {
	
	public float BallHeldDuration;

	public int playerColor;
	private TrailRenderer ballTrail;
	private BallType ballType = BallType.Standard;
	public float startTime;
	public float infiniteBallDuration;
	public bool possesed;
	private float TimePossessed;
	public Color trailColor;
	public bool isOnGround = false;
	public GameObject ballOwner, controlBallPowerUp;
	public List<GameObject> players;
	public float returnSpeed;
	public bool ballShouldReturn;
	public float recallDistanceThreshold;
	public bool ballCanBeControlled;
	public float controlFactor;
	public float kingControlFactor;

	private Vector3 ballVel;
	public PhysicMaterial bouncy;
	public PhysicMaterial bouncyUnlimited;

	// Use this for initialization
	void Start () {
		ballTrail = GetComponent<TrailRenderer> ();
		ballTrail.enabled = false;
		controlBallPowerUp = GameObject.Find ("Power Up 1");
		ballShouldReturn = false;
		ballCanBeControlled = false;

		ballTrail.material = this.gameObject.GetComponent<Renderer>().material;
	}
	
	void Update () {
		switch (ballType) {
		case BallType.InfinitePowerup:
			if ((startTime + infiniteBallDuration) > Time.time) {
				Destroy(this.gameObject);
			}
			break;
		case BallType.Standard:
			break;
		}


		float playerBallDistance = Vector3.Distance(transform.position, ballOwner.transform.position);
		if (playerBallDistance  <= recallDistanceThreshold) {
			ballShouldReturn = false;
		}
		
		if (ballShouldReturn && !ballOwner.GetComponent<PlayerMove> ().isPlayerFalling) {
			float step = returnSpeed * Time.deltaTime;
			gameObject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
			gameObject.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
			//transform.position = Vector3.MoveTowards (transform.position, ballOwner.transform.position, step);
			GetComponent<Rigidbody>().MovePosition(Vector3.MoveTowards (transform.position, ballOwner.transform.position, step));
			ballVel = ballOwner.transform.position;

		}
		else if (ballShouldReturn && ballVel.magnitude > 0) {
			print ("adjusting velocity");
			float step = returnSpeed * Time.deltaTime;
			Vector3 temp = Vector3.MoveTowards (transform.position, ballVel, step) - transform.position;
			//Vector3 temp = ballVel = transfo
			GetComponent<Rigidbody>().velocity = temp / Time.deltaTime;
			//GetComponent<Rigidbody>().AddForce(temp);
			//GetComponent<Rigidbody>().add
			ballVel = Vector3.zero;
			ballShouldReturn = false;
		}

	}

	public void setBallType (BallType bt) {
		switch (bt) {
		case BallType.InfinitePowerup:
			ballType = bt;
			startTime = Time.time;
			break;
		case BallType.Standard:
			ballType = BallType.Standard;
			break;
		}
	}

	public void returnBall () {
		ballShouldReturn = true;
		ballCanBeControlled = false;
	}

	void OnCollisionEnter () {
//		if ((KingOfTheHill.access.currentPlayer + 1) != playerColor) {
//			ballCanBeControlled = false;
//		}
		ballCanBeControlled = false;
	}

	public void applyExtraControl (Vector3 control) {
		if (ballCanBeControlled) {
			Vector3 normalizedControl = control.normalized; 
			Vector3 ballVel = gameObject.GetComponent<Rigidbody>().velocity;
			Vector3 crossProduct = Vector3.Cross(ballVel, Vector3.forward);
			Vector3 projection = Vector3.zero;
//			if ((KingOfTheHill.access.currentPlayer + 1) == playerColor) {
//				projection = Vector3.Project(normalizedControl * kingControlFactor, crossProduct);
//				this.gameObject.GetComponent<Collider>().material = bouncyUnlimited;
//			}
//			else {
				projection = Vector3.Project(normalizedControl * controlFactor, crossProduct);
				this.gameObject.GetComponent<Collider>().material = bouncy;

//			}
			gameObject.GetComponent<Rigidbody>().AddForce(projection);
		}
	}

}

