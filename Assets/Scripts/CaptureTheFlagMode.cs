using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CaptureTheFlagMode : MonoBehaviour {

	public static CaptureTheFlagMode access;

	public float roundLength = 120f;

	public Text countdownText;
	public Text roundClock;
	public GameObject playerOne, playerTwo;
	private int playerOneScore = 0, playerTwoScore = 0;
	public Text playerOneScoreText, playerTwoScoreText;
	public float RoundLength = 3.0f;

	private bool startGame = false;
	private float startGameTime;

	// Use this for initialization
	void Start () {
		access = this;
		roundClock.enabled = false;
		StartCoroutine ("CountdownToBeginRound");
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

	public void playerScore(GameObject playerHit) {
		if (playerHit.name.Equals("Player1")) {
			playerTwoScore += 10;
			playerTwoScoreText.text = playerTwoScore.ToString();
		}
		else if (playerHit.name.Equals("Player2")) {
			playerOneScore += 10;
			playerOneScoreText.text = playerOneScore.ToString();
		}
	}
	
}
