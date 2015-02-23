using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class startSequence : MonoBehaviour {

	public GameObject startSceen, setupScreen;
	public GameObject[] menuText;
	private int textSelect = 0;
	public GameObject[] playersReady;
	private int numPlayersReady = 0;


	// Use this for initialization
	void Start () {
		setupScreen.SetActive (false);
		menuText[textSelect].GetComponent<Text>().color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
		if (startSceen.activeSelf == true) {
			if (Input.GetKeyDown (KeyCode.DownArrow)) {
				startSelectDown();
			}
			else if (Input.GetKeyDown(KeyCode.UpArrow)) {
				startSelectUp();
			}	
			else if (Input.GetKeyDown(KeyCode.Return)) {
				if (textSelect == 0) {
					setupScreen.SetActive(true);
					startSceen.SetActive(false);
				}
			}
		}
		else if (setupScreen.activeSelf == true) {
			if (Input.GetKeyDown(KeyCode.Alpha1)) {
				playerReady(0);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha2)) {
				playerReady(1);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha3)) {
				playerReady (2);
			}
			else if (Input.GetKeyDown(KeyCode.Alpha4)) {
				playerReady (3);
			}

			if(numPlayersReady == 4) {
				Invoke("startGame", 2f);
				numPlayersReady++;
			}
		}
	} 

	void startSelectDown() {
		if (textSelect == (menuText.Length - 1)) {
			return;
		}
		else {
			menuText[textSelect].GetComponent<Text>().color = Color.white;
			textSelect++;
			menuText[textSelect].GetComponent<Text>().color = Color.yellow;
		}
	}

	void startSelectUp() {
		if (textSelect == 0) {
			return;
		}
		else {
			menuText[textSelect].GetComponent<Text>().color = Color.white;
			textSelect--;
			menuText[textSelect].GetComponent<Text>().color = Color.yellow;
		}
	}

	void playerReady(int playerNum) {
		playersReady [playerNum].SetActive (true);
		numPlayersReady++;

	}

	void startGame() {
		setupScreen.SetActive (false);
	}

}
