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
	GameObject ball1, ball2, ball3, ball4, player1, player2, player3, player4, controlBallPowerUp;
	public List<GameObject> players;
	public float returnSpeed;
	public bool ballShouldReturn;
	public float recallDistanceThreshold;
	public bool ballCanBeControlled;
	public float controlFactor;
	public float kingControlFactor;

	public PhysicMaterial bouncy;
	public PhysicMaterial bouncyUnlimited;

	// Use this for initialization
	void Start () {
		ballTrail = GetComponent<TrailRenderer> ();
		ballTrail.enabled = false;
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");
		controlBallPowerUp = GameObject.Find ("Power Up 1");
		ballShouldReturn = false;
		ballCanBeControlled = false;

		ballTrail.material = this.gameObject.GetComponent<Renderer>().material;
		//this.GetComponent<Renderer>().material.color = startColor;
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

		


//		GameObject closestPlayer = players[0];
//		float minDist = Mathf.Infinity;
//
//		foreach (GameObject cur in players) {
//			float curDist = Vector3.Distance(cur.transform.position, this.transform.position);
//			if (curDist < minDist) {
//				minDist = curDist;
//				closestPlayer = cur;
//			}
//		}
//
//		if (minDist < 50f) {
//			float step = 10 * Time.deltaTime;
//			transform.position = Vector3.MoveTowards(transform.position, closestPlayer.transform.position, step);
//			return;
//		}

		float player1BallDistance = Vector3.Distance(transform.position, player1.transform.position);
		float player2BallDistance = Vector3.Distance(transform.position, player2.transform.position);
		float player3BallDistance = Vector3.Distance(transform.position, player3.transform.position);
		float player4BallDistance = Vector3.Distance(transform.position, player4.transform.position);

		if (player1BallDistance  <= recallDistanceThreshold) {
			ballShouldReturn = false;
		} else if (player2BallDistance <= recallDistanceThreshold) {
			ballShouldReturn = false;
		}
		else if (player3BallDistance <= recallDistanceThreshold) {
			ballShouldReturn = false;
		}
		else if (player4BallDistance <= recallDistanceThreshold) {
			ballShouldReturn = false;
		}
		
		if (ballShouldReturn) {
			float step = returnSpeed * Time.deltaTime;
			if (playerColor == 1) {
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
				gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				transform.position = Vector3.MoveTowards(transform.position, player1.transform.position, step);
			}
			else if (playerColor == 2) {
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
				gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				transform.position = Vector3.MoveTowards(transform.position, player2.transform.position, step);
			}
			else if (playerColor == 3) {
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
				gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				transform.position = Vector3.MoveTowards(transform.position, player3.transform.position, step);
			}
			else if (playerColor == 4) {
				gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
				gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
				transform.position = Vector3.MoveTowards(transform.position, player4.transform.position, step);
			}
			else {
				Debug.Log ("BALL NOT POSSESSED BY ANYONE");
			}
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

