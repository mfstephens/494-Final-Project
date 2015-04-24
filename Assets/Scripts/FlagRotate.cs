using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlagRotate : MonoBehaviour {

	public static FlagRotate access;

	public GameObject coinPrefab;

	public float rotateSpeed;
	public Vector3[] positions;
	private float lastCoin = 0;

	public GameObject possessingPlayer;
	public int currentPlayer = -1;
	private Color origColor;
	
	public int[] playerScores;
	public int currentBounty = 0;

	public GameObject[] playerIcons;
	public float pointToDistanceFactor;
	public ParticleSystem playerIconParticles;


	void Start() {
		access = this;

		//playerIconParticles.Play();
		for (int i = 1;i <= playerIcons.Length;i++) {
			playerIcons[i - 1].GetComponent<Text>().color = GameObject.Find("Player" + i.ToString()).GetComponentInChildren<SkinnedMeshRenderer>().material.color;
			playerIcons[i - 1].GetComponentInChildren<Image>().color = playerIcons[i - 1].GetComponent<Text>().color;
		}

		GetComponent<Rigidbody> ().isKinematic = true;

	}

	void Update() {
		if (possessingPlayer != null) {
			FinalStatistics.finalStatistics.AddCubePosession (currentPlayer + 1, Time.deltaTime);
			this.transform.position = possessingPlayer.transform.position + new Vector3 (0, 25f, 0);
		}

		if ((currentPlayer != -1) && (Time.time - lastCoin > 2f)) {
			lastCoin = Time.time;
			makeCoin(possessingPlayer);
			currentBounty += 100;
		}
		
	}

	void FixedUpdate() {
		if (currentPlayer != -1) {
			playerScores[currentPlayer]++;
			MovePlayerCursor (currentPlayer);
		}
		this.transform.Rotate (0, rotateSpeed, 0);
	}

	public void MovePlayerCursor (int player) {
		GameObject playerIcon = playerIcons[player];
		Vector3 temp = playerIcon.GetComponent<RectTransform> ().anchoredPosition3D;
		temp.x = Mathf.Lerp(-360f, 360f, playerScores[player]/5000f);
		playerIcon.GetComponent<RectTransform> ().anchoredPosition3D = temp;
		temp.z -= 50f;
		//playerIconParticles.GetComponent<RectTransform> ().anchoredPosition3D = temp;
	}

	void OnCollisionEnter(Collision other) {
		
		if (other.gameObject.CompareTag ("Player")) {

			if (this.gameObject.GetComponentInChildren<Renderer> ().material.color == other.gameObject.GetComponent<PlayerController>().playerColor) {
				return;
			}
			//this.transform.parent = other.gameObject.transform;

			currentPlayer = other.gameObject.GetComponent<PlayerMove>().playerColor - 1;
			possessingPlayer = other.gameObject;
			this.transform.rotation = Quaternion.identity;
			this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
			this.GetComponent<Collider>().isTrigger = true;
			this.GetComponent<Rigidbody>().useGravity = false;
			this.transform.localScale = this.transform.localScale / 2f;

			//other.transform.localScale += new Vector3(8f, 24f, 5f);
			//other.gameObject.GetComponent<PlayerController>().possessedBall.transform.localScale += new Vector3(9f,9f,9f);
			//other.gameObject.GetComponent<PlayerMove>().jumpSpeed = 800;
		}
	}

	public void dropFlag() {
		this.transform.localScale = this.transform.localScale * 2f;
		this.GetComponent<Collider>().isTrigger = false;
		Physics.IgnoreCollision(possessingPlayer.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
		this.GetComponent<Rigidbody>().AddExplosionForce(65000f, this.transform.position, 20f);
		this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.None ^ RigidbodyConstraints.FreezePositionZ;
		possessingPlayer = null;
		this.GetComponent<Rigidbody>().useGravity = true;
		currentPlayer = -1;
		currentBounty = 0;

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

	void makeCoin(GameObject player) {
		coinPrefab.GetComponent<CoinBehavior>().assignPlayer(player);
		coinPrefab.transform.position = player.transform.position + new Vector3(30f, 0, 0);
		GameObject coin = Instantiate(coinPrefab);
		possessingPlayer.GetComponent<PlayerController> ().coins.Add (coin);
	}

	public int getScoreForPlayer(int playerNumber){
		return playerScores[playerNumber];
	}
}
