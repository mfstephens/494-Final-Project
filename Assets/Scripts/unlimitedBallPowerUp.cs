using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class unlimitedBallPowerUp : MonoBehaviour {

	public static unlimitedBallPowerUp access;

	public Color originalColor;
	public Color currentColor;
	public GameObject currentPlayer = null;
	bool used = false;
	int playerColor = -1;
	private float startTime = -1;
	private string ballName;
	private float duration = 10f;
	private float colorChange = 0;

	//public List<Ball> ballContainer;

	// Use this for initialization
	void Start () {
		access = this;
	}
	
	// Update is called once per frame
	void Update () {
		if ((startTime != -1) && (Time.time - startTime > 10)) {
			print ("TIMES UP");
			startTime = -1;
			colorChange = 0;

			//TODO:there is a bug with this for loop, it sets the last ball the player needs to pick up to color -1...
			foreach (Ball cur in BallContainer.BallContainerSingleton.ballContainer) {
				if (cur.gameObject.name.Equals(ballName)) {
					cur.gameObject.GetComponent<Ball>().playerColor = -1;
				}
			}

			if (currentPlayer.GetComponent<PlayerController>().possessedBall != null) {
				Destroy(currentPlayer.GetComponent<PlayerController>().possessedBall.gameObject.GetComponent<ballDestroy>());
				currentPlayer.GetComponent<PlayerController>().possessedBall.gameObject.GetComponent<Ball>().playerColor = playerColor;
			}
			else {
				Ball newBall = Instantiate (BallContainer.BallContainerSingleton.player1Ball, currentPlayer.transform.position, Quaternion.identity) as Ball;
				newBall.playerColor = playerColor;
				BallContainer.BallContainerSingleton.ballContainer.Add(newBall);
				currentPlayer.gameObject.GetComponent<PlayerController>().possessedBall = newBall;
			}

			currentPlayer = null;
			this.gameObject.renderer.material.color = originalColor;
		}
		else if (startTime != -1) {
			this.gameObject.renderer.material.color = Color.Lerp(currentColor, originalColor, colorChange);
			if (colorChange < 1){
				colorChange += Time.deltaTime/duration;
			}
		}
	}

	void OnTriggerEnter(Collider other) {

		// return if the block is already owned by you
		if (this.gameObject.renderer.material.color == other.gameObject.renderer.material.color) {
			return;
		}
		currentColor = other.gameObject.renderer.material.color;
		this.gameObject.renderer.material.color = other.gameObject.renderer.material.color;

		Ball addBall = null;

		foreach (Ball cur in BallContainer.BallContainerSingleton.ballContainer) {
			if (other.gameObject == cur.gameObject) {
				currentPlayer = matchBallToPlayer (cur.name);
				playerColor = cur.gameObject.GetComponent<Ball>().playerColor;
			}
		}

		startTime = -1;


	}

	public void makeNewBall() {

		if (startTime == -1) {
			startTime = Time.time;
		}

		Ball newBall = Instantiate (BallContainer.BallContainerSingleton.player1Ball, currentPlayer.transform.position, Quaternion.identity) as Ball;
		BallContainer.BallContainerSingleton.ballContainer.Add(newBall);
		currentPlayer.gameObject.GetComponent<PlayerController>().possessedBall = newBall;

		//will prevent new balls from being picked up
		newBall.playerColor = playerColor;
	}

	GameObject matchBallToPlayer(string ballName) {
		if (ballName.Equals("Player1Ball(Clone)")) {
			playerColor = 1;
			return GameObject.Find ("Player1");		
		}
		else if (ballName.Equals("Player2Ball(Clone)")) {
			playerColor = 2;
			return GameObject.Find("Player2");
		}
		else if (ballName.Equals("Player3Ball(Clone)")) {
			playerColor = 3;
			return GameObject.Find("Player3");
		}
		else {
			playerColor = 4;
			return GameObject.Find("Player4");
		}
	}

}
