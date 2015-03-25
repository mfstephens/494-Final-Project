using UnityEngine;
using System.Collections;

public class CaptureTheFlagMode : MonoBehaviour {

	public static CaptureTheFlagMode access;

	public GameObject team1Player1, team1Player2, team2Player1, team2Player2;
	public Vector3 t1p1PosReturn, t1p2PosReturn, t2p1PosReturn, t2p2PosReturn;

	// Use this for initialization
	void Start () {
		access = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void teamScored (GameObject scoringPlayer, GameObject flag) {
		if (flag.name.Equals ("yellowFlag")) {
			if (scoringPlayer.Equals(team1Player1)) {
				scoringPlayer.transform.position = t1p1PosReturn;
				flag.GetComponent<FlagBehavior>().colorFlagOpp();
			}
			else if (scoringPlayer.Equals(team1Player2)) {
				scoringPlayer.transform.position = t1p2PosReturn;
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
				scoringPlayer.transform.position = t2p1PosReturn;
				flag.GetComponent<FlagBehavior>().colorFlagOpp();
			}
			else if (scoringPlayer.Equals(team2Player2)) {
				scoringPlayer.transform.position = t2p2PosReturn;
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
