using UnityEngine;
using System.Collections;


public class PowerUpControlBall : MonoBehaviour {
	Color originalColor;
	public GameObject currentPlayer;
	PlayerController currentController;
	bool started = false;
	float startTime, duration;
	GameObject player1, player2, player3, player4;


	// Use this for initialization
	void Start () {
		currentPlayer = null;
		player1 = GameObject.Find ("Player1");
		player2 = GameObject.Find ("Player2");
		player3 = GameObject.Find ("Player3");
		player4 = GameObject.Find ("Player4");
		duration = 3f;
		originalColor = renderer.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentPlayer != null){
			if(currentController.playerControl.Action2.WasPressed){
				currentController.controlBall = true;
			}
			if(currentController.controlBall && currentController.throwing 
			   && (currentController.playerControl.Action1.WasPressed ||
			   currentController.playerControl.Action2.WasPressed ||
			   currentController.playerControl.Action3.WasPressed ||
			   currentController.playerControl.Action4.WasPressed)){
				setNoControl();
				return;
			}
			if(!started && currentController.throwing && currentController.controlBall){
				startTime = Time.time;
				started = true;
			}
			if(started && ((startTime + duration) < Time.time)) setNoControl();
		}
	}

	//Player controller calls this after time runs out on powerup. 
	public void setNoControl(){
		currentController.throwing = false;
		currentController.controlBall = false;
		started = false;
		currentPlayer = null;
		renderer.material.color = originalColor;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag != "Ball") return;
		if (gameObject.renderer.material.color == other.gameObject.renderer.material.color) return;
		if(currentPlayer!= null) currentPlayer.GetComponent<PlayerController>().controlBall = false;
		renderer.material.color = other.gameObject.renderer.material.color;
		if("Player1Ball(Clone)" == other.name) {
			currentPlayer = player1; 
			currentController = currentPlayer.GetComponent<PlayerController>();
			currentController.controlBall = false;
			currentController.throwing = false;
		}
		else if("Player2Ball(Clone)" == other.name) {
			currentPlayer = player2;
			currentController = currentPlayer.GetComponent<PlayerController>();
			currentController.controlBall = false;
			currentController.throwing = false;
		}
		else if("Player3Ball(Clone)" == other.name) {currentPlayer = player3; 
			currentController = currentPlayer.GetComponent<PlayerController>();
			currentController.controlBall = false;
			currentController.throwing = false;
		}
		else if("Player4Ball(Clone)" == other.name) {currentPlayer = player4; 
			currentController = currentPlayer.GetComponent<PlayerController>();
			currentController.controlBall = false;
			currentController.throwing = false;
		}
	}
}
