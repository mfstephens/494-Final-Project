using UnityEngine;
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
	public Color startColor = Color.red;
	public Color endColor = Color.white;
	public bool isOnGround = false;
	GameObject ball1, ball2, ball3, ball4, player1, player2, player3, player4, controlBallPowerUp;
	public float returnSpeed;
	
	// Use this for initialization
	void Start () {
		ballTrail = GetComponent<TrailRenderer> ();
		ballTrail.enabled = false;
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");
		controlBallPowerUp = GameObject.Find ("Power Up 1");
		
		this.GetComponent<Renderer>().material.color = startColor;
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
	}
	public void findPlayerAndReturn() {
		float step = returnSpeed * Time.deltaTime;
		if (playerColor == 1) {
			transform.position = Vector3.MoveTowards(transform.position, player1.transform.position, step);
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
}

