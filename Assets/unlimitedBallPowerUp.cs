using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class unlimitedBallPowerUp : MonoBehaviour {

	public static unlimitedBallPowerUp access;

	public Ball player1pre;
	public Ball player2pre;
	public Ball player3pre;
	public Ball player4pre;


	public Ball currentPre;
	public Color originalColor;
	public Color currentColor;
	public GameObject currentPlayer = null;
	bool used = false;
	int curPlayerColor = -1;
	private float startTime = -1;
	private string ballName;
	private float duration = 10f;
	private float colorChange = 0;
	public int numThrows = 0;

	//public List<Ball> ballContainer;

	// Use this for initialization
	void Start () {
		access = this;
	}
	
	// Update is called once per frame
	void Update () {
		if ((startTime != -1) && (Time.time - startTime > 10)) {
			startTime = -1;
			colorChange = 0;
			numThrows = 0;

			// need to change this statement for the number of players...
			if ((currentPlayer.GetComponent<PlayerController>().possession == true) || (BallContainer.BallContainerSingleton.ballContainer.Count == 3)) {
				Destroy(currentPlayer.GetComponent<PlayerController>().possessedBall.gameObject.GetComponent<ballDestroy>());
			}
			else {
				Ball newBall = Instantiate (currentPre, currentPlayer.transform.position, Quaternion.identity) as Ball;
				newBall.playerColor = curPlayerColor;
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
		else if (!other.gameObject.tag.Equals("Ball")) {
			return;
		}

		currentColor = other.gameObject.renderer.material.color;
		this.gameObject.renderer.material.color = other.gameObject.renderer.material.color;

		foreach (Ball cur in BallContainer.BallContainerSingleton.ballContainer) {
			if (other.gameObject == cur.gameObject) {
				currentPlayer = matchBallToPlayerAndPrefab (cur.name);
				curPlayerColor = cur.gameObject.GetComponent<Ball>().playerColor;
			}
		}

		//give the player who stole the power up the full amount of time
		startTime = -1;
	}

	public void makeNewBall() {

		if (startTime == -1) {
			startTime = Time.time;
		}

		Ball newBall = Instantiate (currentPre, currentPlayer.transform.position, Quaternion.identity) as Ball;
		BallContainer.BallContainerSingleton.ballContainer.Add(newBall);
		print ("got past here");
	}

	GameObject matchBallToPlayerAndPrefab(string ballName) {
		if (ballName.Equals("Player1Ball(Clone)")) {
			curPlayerColor = 1;
			currentPre = player1pre;
			return GameObject.Find ("Player1");		
		}
		else if (ballName.Equals("Player2Ball(Clone)")) {
			curPlayerColor = 2;
			currentPre = player2pre;
			return GameObject.Find("Player2");
		}
		else if (ballName.Equals("Player3Ball(Clone)")) {
			curPlayerColor = 3;
			currentPre = player3pre;
			return GameObject.Find("Player3");
		}
		else {
			curPlayerColor = 4;
			currentPre = player4pre;
			return GameObject.Find("Player4");
		}
	}

}
