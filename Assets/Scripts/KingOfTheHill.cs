using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class KingOfTheHill : MonoBehaviour {

	public static KingOfTheHill access;


	private Color origColor;
	
	public float roundLength = 120f;
	
	public Text countdownText;
	public Text roundClock;
	public float RoundLength = 3.0f;
	
	private bool startGame = false;
	bool flagDropped = false;
	private float startGameTime;


	void Awake() {
		access = this;
	}

	void Start() {
		access = this;
		roundClock.enabled = false;
		StartCoroutine ("CountdownToBeginRound");
		origColor = this.gameObject.GetComponent<Renderer> ().material.color;
	}

	// Update is called once per frame
	void Update () {
		if (startGame) {
			int currentTime = Mathf.CeilToInt(roundLength-(Time.time - startGameTime));
			if(currentTime%60 < 10){
				roundClock.text = (currentTime/60).ToString() + ":0"+(currentTime%60).ToString();
			}
			else{
				roundClock.text = (currentTime/60).ToString() + ":"+(currentTime%60).ToString();
			}
			if (currentTime == 0) {
				startGame = false;
				Time.timeScale = 0;
			}

			if (!flagDropped) {
				FlagRotate.access.GetComponent<Rigidbody>().useGravity = true;
				flagDropped = true;
			}
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
		this.gameObject.GetComponent<Renderer> ().material.color = other.gameObject.GetComponent<PlayerController>().playerColor;
	}


}
