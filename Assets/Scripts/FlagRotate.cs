using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlagRotate : MonoBehaviour {

	public static FlagRotate access;

	public float rotateSpeed;
	public Vector3[] positions;
	private float startTime = 0;

	public GameObject possessingPlayer;
	public int currentPlayer = -1;
	private Color origColor;

	public Text[] playerScoreTexts;
	public int[] playerScores;

	void Start() {
		access = this;
	}

	void Update() {
//		if ((Time.time < 3) && (Time.time - startTime > 0.1f)) {
//			startTime = Time.time;
//			int random = Random.Range(0,2);
//			Vector3 newPos = positions[random];
//			positions[random] = this.transform.position;
//			this.transform.position = newPos;
//		}
//		if (Time.time - startTime > 20f) {
//			startTime = Time.time;
//			int random = Random.Range(0,2);
//			Vector3 newPos = positions[random];
//			positions[random] = this.transform.position;
//			this.transform.position = newPos;	
//			this.gameObject.GetComponentInChildren<KingOfTheHill>().updateCurrentPlayer(-1);
//		}
		if (possessingPlayer != null) {
			this.transform.position = possessingPlayer.transform.position + new Vector3(0,23f,0);
		} else {
			print ("player is null");
		}
	}

	void FixedUpdate() {
		if (currentPlayer != -1) {
			playerScores[currentPlayer]++;
			playerScoreTexts[currentPlayer].text = playerScores[currentPlayer].ToString();
		}
		this.transform.Rotate (0, rotateSpeed, 0);
	}

	void OnCollisionEnter(Collision other) {
		
		if (other.gameObject.CompareTag ("Player")) {


			if (this.gameObject.GetComponentInChildren<Renderer> ().material.color == other.gameObject.GetComponent<PlayerController>().playerColor) {
				return;
			}

			//this.transform.parent = other.gameObject.transform;

			currentPlayer = other.gameObject.GetComponent<PlayerMove>().playerColor - 1;
			possessingPlayer = other.gameObject;
			this.transform.localScale = this.transform.localScale / 2f;
			this.GetComponent<Collider>().isTrigger = true;
			this.GetComponent<Rigidbody>().useGravity = false;

			//other.transform.localScale += new Vector3(8f, 24f, 5f);
			//other.gameObject.GetComponent<PlayerController>().possessedBall.transform.localScale += new Vector3(9f,9f,9f);
			//other.gameObject.GetComponent<PlayerMove>().jumpSpeed = 800;
		}
	}

	public void dropFlag() {
		print ("dropped flag");
		this.GetComponent<Collider>().isTrigger = false;
		Physics.IgnoreCollision(possessingPlayer.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
		this.GetComponent<Rigidbody>().AddExplosionForce(65000f, this.transform.position, 20f);
		this.transform.localScale = this.transform.localScale * 2f;
		possessingPlayer = null;
		this.GetComponent<Rigidbody>().useGravity = true;
		currentPlayer = -1;

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
