using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CaptureTheFlagMode : MonoBehaviour {

	public static CaptureTheFlagMode access;

	public float roundLength = 120f;

	public Text countdownText;
	public Text roundClock;
	public GameObject team1Player1, team1Player2, team2Player1, team2Player2;
	public Vector3 t1p1PosReturn, t1p2PosReturn, t2p1PosReturn, t2p2PosReturn;
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

	public void teamScored (GameObject scoringPlayer, GameObject flag) {
		if (flag.name.Equals ("yellowFlag")) {
			if (scoringPlayer.Equals(team1Player1)) {
				//scoringPlayer.transform.position = t1p1PosReturn;
				flag.GetComponent<FlagBehavior>().colorFlagOpp();
			}
			else if (scoringPlayer.Equals(team1Player2)) {
				//scoringPlayer.transform.position = t1p2PosReturn;
				flag.GetComponent<FlagBehavior>().colorFlagOpp();
			}
			else if (scoringPlayer.Equals(team2Player1)) {
				flag.GetComponent<FlagBehavior>().colorFlagOrig();
			}
			else if (scoringPlayer.Equals(team2Player2)) {
				flag.GetComponent<FlagBehavior>().colorFlagOrig();
			}
		}
		else if (flag.name.Equals("redFlag")) {
			if (scoringPlayer.Equals(team2Player1)) {
				//scoringPlayer.transform.position = t2p1PosReturn;
				flag.GetComponent<FlagBehavior>().colorFlagOpp();
			}
			else if (scoringPlayer.Equals(team2Player2)) {
				//scoringPlayer.transform.position = t2p2PosReturn;
				flag.GetComponent<FlagBehavior>().colorFlagOpp();
			}
			if (scoringPlayer.Equals(team1Player1)) {
				flag.GetComponent<FlagBehavior>().colorFlagOrig();
			}
			else if (scoringPlayer.Equals(team1Player2)) {
				flag.GetComponent<FlagBehavior>().colorFlagOrig();
			}
		}
	}

	//outdated
	public void returnPlayer(GameObject player) {
		if (player.Equals(team1Player1)) {

			player.transform.position = t1p1PosReturn;
		}
		else if (player.Equals(team1Player2)) {
			player.transform.position = t1p2PosReturn;
		}
		else if (player.Equals(team2Player1)) {
			player.transform.position = t2p1PosReturn;
		}
		else if (player.Equals(team2Player2)) {
			player.transform.position = t2p2PosReturn;
		}
	}
	
}
