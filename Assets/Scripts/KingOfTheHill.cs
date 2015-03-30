using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KingOfTheHill : MonoBehaviour {

	public static KingOfTheHill access;

	public Text[] playerScoreTexts;
	public int[] playerScores;
	public int currentPlayer = -1;
	private Color origColor;

	void Awake() {
		access = this;
	}

	void Start() {

		origColor = this.gameObject.GetComponent<Renderer> ().material.color;
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

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag ("Player")) {
			this.gameObject.GetComponent<Renderer> ().material.color = other.gameObject.GetComponent<Renderer> ().material.color;
			currentPlayer = other.gameObject.GetComponent<PlayerMove>().playerColor - 1;
		}
	}

	public void updateCurrentPlayer(int player) {
		currentPlayer = player;

	}
}
