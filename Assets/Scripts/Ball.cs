﻿using UnityEngine;
using System.Collections;

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
	public float returnSpeed;
	public bool ballShouldReturn;
	public float recallDistanceThreshold;
	
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

		if (transform.position == player1.transform.position) {
			ballShouldReturn = false;
		}

		if (ballShouldReturn) {
			float step = returnSpeed * Time.deltaTime;
			if (playerColor == 1) {
				float playerBallDistance = Vector3.Distance(transform.position, player1.transform.position);
				if (playerBallDistance  > recallDistanceThreshold) {
					gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
					gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
					transform.position = Vector3.MoveTowards(transform.position, player1.transform.position, step);
				}
			}
			else if (playerColor == 2) {
				transform.position = Vector3.MoveTowards(transform.position, player2.transform.position, step);
			}
			else if (playerColor == 3) {
				transform.position = Vector3.MoveTowards(transform.position, player3.transform.position, step);
			}
			else if (playerColor == 4) {
				transform.position = Vector3.MoveTowards(transform.position, player4.transform.position, step);
			}
			else {
				Debug.Log ("BALL NOT POSSESSED BY ANYONE");
			}
		}

	}
	public void findPlayerAndReturn() {

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
	}

	public void applyExtraControl (Vector3 control) {
		gameObject.GetComponent<Rigidbody>().AddForce(control);
	}

}

