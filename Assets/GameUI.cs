using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	
	public float timeLeft = 60.0f;

	bool timeStarted = false;

	public GameObject timer;
	public GameObject team1Score;
	public GameObject team2Score;

	// Use this for initialization
	void Start () {
		startTimer ();
	}
	
	// Update is called once per frame
	void Update () {
		if (timeLeft >= 0) {
			timeLeft -= Time.deltaTime;
			timer.GetComponent<Text>().text = timeLeft.ToString ("F1");
		} else {
			print ("Game Over!");
		}
	}

	void startTimer() {
		timeLeft = 60.0f;
	}

	void updateTeamScore(int teamNumber, float score) {
		if (teamNumber == 1) {
//			team1Score. += score;

		} else if (teamNumber == 2) {
//			team2Score += score;
		}
	}

}
