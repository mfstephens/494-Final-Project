using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;


public class KingOfTheHill : MonoBehaviour {

	public static KingOfTheHill access;

	private Color origColor;
	
	public float roundLength = 120f;
	
	public Text countdownText, getCubeText, nearVictoryText;
	public float RoundLength = 3.0f;
	
	private bool startGame = false;
	private float startGameTime;

	private int numberOfPlayersReady = 0;
	public int pointGoal;
	public GameObject trophyIcon;

	public GameObject victoryBar;
	bool startedFlashingVictoryBar = false;
	float victoryBarFlashSpeed = 0.6f;


	public GameObject[] players;
	public GameObject crown;

	private AudioSource audioSource;
	//public AudioClip beginGameClip;
	public AudioClip startSequence;

	
	void Awake() {
		access = this;
	}

	void Start() {
		audioSource = GetComponent<AudioSource> ();
		StartCoroutine ("CountdownToBeginRound");
		audioSource.Play ();
		origColor = this.gameObject.GetComponent<Renderer> ().material.color;
		getCubeText.enabled = false;
		nearVictoryText.enabled = false;

		//Disable the player move scripts and crown until the countdown is finished
		for (int i = 0; i < players.Length; i++) {
			players[i].GetComponent<PlayerController>().startFreeze = true;
		}
		audioSource.PlayOneShot (startSequence);
		StartCoroutine ("ShowGetCubeText");
	}

	// Update is called once per frame
	void Update () {
		//Game Currently in Progress
		if (startGame) {
			trophyIcon.transform.Rotate(Vector3.up);
			bool shakeInitiated = false;
			foreach (int score in FlagRotate.access.playerScores) {
				if (score >= 4000 && !startedFlashingVictoryBar) {
					StartCoroutine("FlashVictoryBarAndText");
					StartCoroutine("ShowVictoryNearText");
					startedFlashingVictoryBar = true;
				}
				if (score >= 4300 && victoryBarFlashSpeed == 0.6f) {
					victoryBarFlashSpeed = 0.3f;
				}
				if (score >= 4600 && victoryBarFlashSpeed == 0.3f) {
					victoryBarFlashSpeed = 0.15f;
				}
				if (score >= 4800 && victoryBarFlashSpeed == 0.15f) {
					victoryBarFlashSpeed = 0.10f;
				}

				if (score >= pointGoal) {
					StopCoroutine("FlashVictoryBar");
					EndGameMenu.access.EndOfGame();
					startGame = false;
					Time.timeScale = 0;
				}
			}
		}
	}

	IEnumerator ShowVictoryNearText () {
		int numBlinks = 0;
		while(numBlinks <= 6) {
			nearVictoryText.enabled = true;
			yield return new WaitForSeconds(.3f);
			nearVictoryText.enabled = false;
			yield return new WaitForSeconds(.3f);
			numBlinks++;
		}
	} 

	IEnumerator FlashVictoryBarAndText () {
		while (true) {
			victoryBar.GetComponent<Image>().color = new Color(255f, 0, 0);
			yield return new WaitForSeconds(victoryBarFlashSpeed);
			victoryBar.GetComponent<Image>().color = new Color(255f, 255f, 255f);
			yield return new WaitForSeconds(victoryBarFlashSpeed);
		}
	}

	IEnumerator ShowGetCubeText () {
		yield return new WaitForSeconds(3f);
		int numBlinks = 0;
		while(numBlinks <= 4) {
			getCubeText.enabled = true;
			yield return new WaitForSeconds(.3f);
			getCubeText.enabled = false;
			yield return new WaitForSeconds(.3f);
			numBlinks++;
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
		startGame = true;
		startGameTime = Time.time;
		countdownText.enabled = false;
		//audioSource.PlayOneShot (beginGameClip);
		for (int i = 0; i < players.Length; i++) {
			players[i].GetComponent<PlayerController>().startFreeze = false;
		}
		crown.GetComponent<Rigidbody> ().isKinematic = false;
	}

	void OnTriggerEnter(Collider other) {
		FlagRotate.access.GetComponent<Rigidbody>().useGravity = true;
		this.gameObject.GetComponent<Renderer> ().material.color = other.gameObject.GetComponent<PlayerController>().playerColor;
	}
}
