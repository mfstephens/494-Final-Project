using UnityEngine;
using System.Collections;

public enum Players{
	Player1,
	Player2,
	Player3,
	Player4,
	None
}

public enum BallType {
	Standard,
	InfinitePowerup
}

public class Ball : MonoBehaviour {

	public float BallHeldDuration;

	public int playerColor;
	public Players possessedBy;
	private Players thrownBy;
	private TrailRenderer ballTrail;
	private BallType ballType = BallType.Standard;
	public float startTime;
	public float infiniteBallDuration;

	private float TimePossessed;
	public Color startColor = Color.red;
	public Color endColor = Color.white;
	public bool isOnGround = false;
	GameObject ball1, ball2, ball3, ball4, player1, player2, player3, player4, controlBallPowerUp;

	// Use this for initialization
	void Start () {
		ballTrail = GetComponent<TrailRenderer> ();
		ballTrail.enabled = false;
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");
		controlBallPowerUp = GameObject.Find ("Power Up 1");

		this.renderer.material.color = startColor;
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

	//Who is in control of the ball
	public void ballPickedUpBy(string player){
		if (player == "Player1")
			possessedBy = Players.Player1;
		else if (player == "Player2")
			possessedBy = Players.Player2;
		else if (player == "Player3")
			possessedBy = Players.Player3;
		else if (player == "Player4")
			possessedBy = Players.Player4;
		else {
			Debug.LogError ("Invalid Paramter Sent To ballPickedUpBy");
		}
		ballTrail.enabled = false;
		TimePossessed = Time.time;
	}

	//Records who threw the ball and sets the balls possession to none
	public void ballThrown(){
		thrownBy = possessedBy;
		possessedBy = Players.None;
//		ballTrail.enabled = true;
	}

	//Returns whether or not a ball was thrown by a player
	public bool thrownByPlayer(string name){

		if (name == "Player1") {
			if (thrownBy == Players.Player1)
				return true;
		}
		if (name == "Player2") {
			if (thrownBy == Players.Player2)
				return true;
		}
		if (name == "Player3"){
			if (thrownBy ==Players.Player3)
				return true;
		}
		if (name == "Player4") {
			if (thrownBy == Players.Player4)
				return true;
		}
		return false;	
	}

	// take away controlBall power up on a collision
	void OnCollisionEnter(Collision other) {
//		if (!other.gameObject.tag.Equals ("Platform") && !other.gameObject.tag.Equals ("Ground") && !other.gameObject.tag.Equals ("RightWall") && !other.gameObject.tag.Equals ("LeftWall") && (playerColor != -1)) {
//			print ("should be returning!");
//			findPlayerAndReturn(other);
//		}

//		if (other.gameObject.CompareTag ("Ground") || other.gameObject.CompareTag ("Platform"))
//			isOnGround = true;

		/* if ((name == "Player1Ball(Clone)") && (player1.GetComponent<PlayerController> ().controlBall == true) 
		    && (player1.GetComponent<PlayerController> ().throwing == true) && (other.gameObject != player1)){
			controlBallPowerUp.GetComponent<PowerUpControlBall>().setNoControl ();
		}
		else if ((name == "Player2Ball(Clone)") && player2.GetComponent<PlayerController> ().controlBall == true 
		         && player2.GetComponent<PlayerController> ().throwing == true && other.gameObject != player2) {
			controlBallPowerUp.GetComponent<PowerUpControlBall> ().setNoControl ();
		}
		else if ((name == "Player3Ball(Clone)") && player3.GetComponent<PlayerController> ().controlBall == true 
		         && player3.GetComponent<PlayerController> ().throwing == true && other.gameObject != player3){
			controlBallPowerUp.GetComponent<PowerUpControlBall> ().setNoControl ();
		}
		else if ((name == "Player4Ball(Clone)") && player4.GetComponent<PlayerController> ().controlBall == true 
		         && player4.GetComponent<PlayerController> ().throwing == true && other.gameObject != player4) {
			controlBallPowerUp.GetComponent<PowerUpControlBall> ().setNoControl ();
		} */

	}

	public void findPlayerAndReturn() {
		if (thrownBy == Players.Player1) {
			this.gameObject.transform.position = player1.transform.position;
		}
		else if (thrownBy == Players.Player2) {
			this.gameObject.transform.position = player2.transform.position;
		}
		else if (thrownBy == Players.Player3) {
			this.gameObject.transform.position = player3.transform.position;
		}
		else if (thrownBy == Players.Player4) {
			this.gameObject.transform.position = player4.transform.position;
		}
		else {
			Debug.Log ("BALL NOT POSSESSED BY ANYONE");
		}
	}


	void OnCollisionExit(Collision other){
		if (other.gameObject.CompareTag ("Ground") || other.gameObject.CompareTag ("Platform"))
			isOnGround = false;
	}

	public bool isBallOnGround(){
		return isOnGround;
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

