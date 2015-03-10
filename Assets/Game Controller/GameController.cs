using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

enum GameStatus {
	PreStart,
	Started,
	Ended
}

public class GameController : MonoBehaviour {
	float gameLength = 120.0f;
	float countdownTimerLength = 3.0f;
	int numberOfTeams = 2;
	float team1Score, team2Score;

	GameStatus status;
	float timeRemaining;
	float countdownRemaining;
	int teamIDWithBall;
	public GameObject[] players;

	public GameObject gameUI;
	public GameObject timerUI;
	public GameObject countdownTimerUI;
	public GameObject team1ScoreUI;
	public GameObject team2ScoreUI;

	// Use this for initialization
	void Start () {
		HideGameUI ();
	}

	// Update is called once per frame
	void Update () {
		switch (status) {
		case GameStatus.PreStart:
			countdownRemaining -= Time.deltaTime;
			ShowGameUI ();
			if (countdownRemaining <= 0) {
				StartGame ();
			} else if (countdownRemaining <= 0.5) {
				countdownTimerUI.GetComponent<Text>().text = "GO!";
			} else {
				// countdown timer
				countdownTimerUI.GetComponent<Text>().text = countdownRemaining.ToString ("F0");
			}
			break;
		case GameStatus.Started:
			// update game time
			timeRemaining -= Time.deltaTime;

			// calculate scores for each team
			if (teamIDWithBall == 1) {
				team1Score += 0.25f;
			} else if (teamIDWithBall == 2) {
				team2Score += 0.25f;
			}

			// update UI with new data
			updateGameUI();

			// end the game if we're out of time
			if (timeRemaining <= 0) {
				EndGame();
			}

			break;
		case GameStatus.Ended:
			// game's over yo
			break;
		}

	}

	void updateGameUI () {
		timerUI.GetComponent<Text>().text = timeRemaining.ToString ("F1");
		team1ScoreUI.GetComponent<Text> ().text = team1Score.ToString("F0");
		team2ScoreUI.GetComponent<Text> ().text = team2Score.ToString("F0");
	}

	void HideGameUI () {
		gameUI.SetActive (false);
	}

	void ShowGameUI() {
		gameUI.SetActive (true);
	}

	void SetUpGame () {
		// init our timers
		countdownRemaining = countdownTimerLength;
		timeRemaining = gameLength;

		// update game status
		status = GameStatus.PreStart;

		// clear team scores
		team1Score = 0;
		team2Score = 0;

		// default value: -1 == no team has the ball
		teamIDWithBall = -1;

		// pause game until further notice
		PauseGame ();
	}

	void EndGame () {
		Time.timeScale = 0;
		status = GameStatus.Ended;
	}

	void StartGame () {
		// remove countdown timer
		Destroy(countdownTimerUI);

		// start the game
		PlayGame ();
		status = GameStatus.Started;
	}

	// Won't stop ball movement, but will stop player movement. SHould fix to do both later.
	void PauseGame () {
		foreach (GameObject player in players) {
			player.GetComponent<CustomProfileExample.PlayerMovement>().enabled = false;
		}
	}
	void PlayGame () {
		foreach (GameObject player in players) {
			player.GetComponent<CustomProfileExample.PlayerMovement>().enabled = true;
		}
	}

	/* 
	 * Public functions
	*/

	public void UpdateTeamIDWithBall (int teamID) {
		teamIDWithBall = teamID;
	}

	public void InitNewGame () {
		ShowGameUI ();
		SetUpGame ();
	}

}
