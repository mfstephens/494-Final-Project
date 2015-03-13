using UnityEngine;
using System.Collections;

public enum Players{
	Player1,
	Player2,
	Player3,
	Player4,
	None
}

public class Ball : MonoBehaviour {

	public float BallHeldDuration;

	private int playerColor;
	private Players possessedBy;
	private Players thrownBy;
	private TrailRenderer ballTrail;

	private float TimePossessed;
	public Color startColor = Color.red;
	public Color endColor = Color.white;

	// Use this for initialization
	void Start () {
		possessedBy = Players.None;
		ballTrail = GetComponent<TrailRenderer> ();
		ballTrail.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Gradually change the balls color to trigger a bomb?
		if (possessedBy != Players.None) {
			float percentDone = (Time.time - TimePossessed) / BallHeldDuration;
			this.renderer.material.color = Color.Lerp (startColor, endColor, percentDone);
		}
		else {
			this.renderer.material.color = startColor;
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
		ballTrail.enabled = true;
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
}

