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

			if (this.gameObject.GetComponent<Renderer> ().material.color == other.gameObject.GetComponent<Renderer> ().material.color) {
				return;
			}

			this.gameObject.GetComponent<Renderer> ().material.color = other.gameObject.GetComponent<Renderer> ().material.color;
			currentPlayer = other.gameObject.GetComponent<PlayerMove>().playerColor - 1;
			//other.transform.lossyScale.Scale(new Vector3(2f, 2f, 2f)); //= Vector3(16f,48f,10f);
			other.transform.localScale += new Vector3(8f, 24f, 5f);
			other.gameObject.GetComponent<PlayerController>().possessedBall.transform.localScale += new Vector3(9f,9f,9f);
			other.gameObject.GetComponent<PlayerMove>().jumpSpeed = 800;
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
