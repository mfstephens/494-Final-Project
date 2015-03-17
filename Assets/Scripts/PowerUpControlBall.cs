using UnityEngine;
using System.Collections;

public class PowerUpControlBall : MonoBehaviour {
	Color originalColor;
	public GameObject currentPlayer, player1Ball, player2Ball, player3Ball, player4Ball;
	GameObject player1, player2, player3, player4;

	// Use this for initialization
	void Start () {
		currentPlayer = null;
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");
		player1Ball = GameObject.Find ("Player1Ball(Clone)");
		player2Ball = GameObject.Find ("Player2Ball(Clone)");
		player3Ball = GameObject.Find ("Player3Ball(Clone)");
		player4Ball = GameObject.Find ("Player4Ball(Clone)");
		originalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentPlayer != null){
			if(currentPlayer.GetComponent<PlayerController>().playerControl.Action2.WasPressed){
				currentPlayer.GetComponent<PlayerController>().controlBall = true;
			}
		}
	}
	public void setNoControl(){
		currentPlayer = null;
		renderer.material.color = originalColor;

	}
	void OnCollisionEnter(Collision other){
		if (other.gameObject == player1Ball) {
			if(currentPlayer != null && currentPlayer != player1) currentPlayer.GetComponent<PlayerController>().controlBall = false;
			currentPlayer = player1;
			renderer.material.color = player1Ball.renderer.material.color;
		}
		else if(other.gameObject == player2Ball){
			if(currentPlayer != null && currentPlayer != player2) currentPlayer.GetComponent<PlayerController>().controlBall = false;
			currentPlayer = player2;
			renderer.material.color = player2Ball.renderer.material.color;
		}
		else if(other.gameObject == player3Ball){
			if(currentPlayer != null && currentPlayer != player3) currentPlayer.GetComponent<PlayerController>().controlBall = false;
			currentPlayer = player3;
			renderer.material.color = player3Ball.renderer.material.color;
		}
		else if(other.gameObject == player4Ball){
			if(currentPlayer != null && currentPlayer != player4) currentPlayer.GetComponent<PlayerController>().controlBall = false;
			currentPlayer = player4;
			renderer.material.color = player4Ball.renderer.material.color;
		}
	}

}
