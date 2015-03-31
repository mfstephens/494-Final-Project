using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KingOfTheHill : MonoBehaviour {

	public static KingOfTheHill access;

	public Text[] playerScoreTexts;
	public int[] playerScores;
	public int currentPlayer = -1;
	private Color origColor;
	
	public float roundLength = 120f;
	
	public Text countdownText;
	public Text roundClock;
	public float RoundLength = 3.0f;
	
	private bool startGame = false;
	private float startGameTime;


	void Awake() {
		access = this;
	}

	void Start() {
		access = this;
		roundClock.enabled = false;
		StartCoroutine ("CountdownToBeginRound");
		origColor = this.gameObject.GetComponent<Renderer> ().material.color;
	}

	// Update is called once per frame
	void Update () {
		if (startGame) {
			int currentTime = Mathf.CeilToInt(roundLength-(Time.time - startGameTime));
			if(currentTime%60 == 0)
				roundClock.text = (currentTime/60).ToString()+":00";
			else
				roundClock.text = (currentTime/60).ToString()+":"+(currentTime%60).ToString();
		}
	}

	void FixedUpdate() {
		if (currentPlayer != -1) {
			playerScores[currentPlayer]++;
			playerScoreTexts[currentPlayer].text = playerScores[currentPlayer].ToString();
		}
		else {
			this.gameObject.GetComponent<Renderer>().material.color = origColor;
		}
	}

	IEnumerator CountdownToBeginRound(){
		float startTime = Time.time;
		while (Time.time < startTime + RoundLength) {
			int currentDifference = Mathf.FloorToInt(Time.time - startTime);
			if(currentDifference == 0){
				countdownText.text = "3";
			}
			else if (currentDifference == 1){
				countdownText.text = "2";
			}
			else if (currentDifference == 2){
				countdownText.text = "1";
			}
			
			yield return null;
		}
		roundClock.enabled = true;
		startGame = true;
		startGameTime = Time.time;
		countdownText.enabled = false;
	}


	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag ("Player")) {

			if (this.gameObject.GetComponent<Renderer> ().material.color == other.gameObject.GetComponent<Renderer> ().material.color) {
				return;
			}

			this.gameObject.GetComponent<Renderer> ().material.color = other.gameObject.GetComponent<Renderer> ().material.color;
			currentPlayer = other.gameObject.GetComponent<PlayerMove>().playerColor - 1;
			//other.transform.localScale += new Vector3(8f, 24f, 5f);
			//other.gameObject.GetComponent<PlayerController>().possessedBall.transform.localScale += new Vector3(9f,9f,9f);
			//other.gameObject.GetComponent<PlayerMove>().jumpSpeed = 800;
		}
	}

	public void updateCurrentPlayer(int player) {
		currentPlayer = player;
	}

	public bool isKing(int playerColor) {
		if (playerColor - 1 == currentPlayer) 
			return true;
		else 
			return false;
	}
}
