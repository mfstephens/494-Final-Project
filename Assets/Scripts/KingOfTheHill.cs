using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;


public class KingOfTheHill : MonoBehaviour {

	public static KingOfTheHill access;

	private Color origColor;
	
	public float roundLength = 120f;
	
	public Text countdownText, getCubeText;
	public Text roundClock;
	public float RoundLength = 3.0f;
	
	private bool startGame = false;
	private float startGameTime;

	private int numberOfPlayersReady = 0;
	
	void Awake() {
		access = this;
	}

	void Start() {
		access = this;
		roundClock.enabled = false;
		//endGameUI.gameObject.SetActive(false);
		StartCoroutine ("CountdownToBeginRound");
		origColor = this.gameObject.GetComponent<Renderer> ().material.color;
		getCubeText.enabled = false;
		StartCoroutine ("ShowGetCubeText");
	}

	// Update is called once per frame
	void Update () {
		//Game Currently in Progress
		if (startGame) {
			int currentTime = Mathf.CeilToInt(roundLength-(Time.time - startGameTime));
			if(currentTime%60 < 10){
				roundClock.text = (currentTime/60).ToString() + ":0"+(currentTime%60).ToString();
			}
			else{
				roundClock.text = (currentTime/60).ToString() + ":"+(currentTime%60).ToString();
			}
			if (currentTime == 0) {
				EndGameMenu.access.EndOfGame();
				startGame = false;
				Time.timeScale = 0;
			}
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
		roundClock.enabled = true;
		startGame = true;
		startGameTime = Time.time;
		countdownText.enabled = false;
	}

	void OnTriggerEnter(Collider other) {
		FlagRotate.access.GetComponent<Rigidbody>().useGravity = true;
		this.gameObject.GetComponent<Renderer> ().material.color = other.gameObject.GetComponent<PlayerController>().playerColor;
	}
}
