﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class startSequence : MonoBehaviour {

	public GameObject startSceen, setupScreen;
	public GameObject[] menuText;
	private int textSelect = 0;
	public GameObject[] playersReady;
	private int numPlayersReady = 0;
	private bool stickHeldDown = false;

	private InputDevice player1;
	private InputDevice player2;
	private InputDevice player3;
	private InputDevice player4;

	public GameObject Kenny;
	public GameObject Cartman;


	// Use this for initialization
	void Start () {
		setupScreen.SetActive (false);
		startSceen.SetActive (true);
		menuText[textSelect].GetComponent<Text>().color = Color.yellow;
		if (InputManager.Devices.Count < 4) {
			Debug.LogError ("Only "+InputManager.Devices.Count+" controllers connected");
			player1=InputManager.Devices[0];
			player2=InputManager.Devices[1];
		} else {
			player1=InputManager.Devices[0];
			player2=InputManager.Devices[1];
			//player3=InputManager.Devices[2];
			//player4=InputManager.Devices[3]; Only have two players right now for testing
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (startSceen.activeSelf == true) {
			if (player1.DPadDown.WasPressed || player1.LeftStickY<0) {
				if(!stickHeldDown){
					startSelectDown();
					stickHeldDown=true;
				}
			}
			else if (player1.DPadUp.WasPressed || player1.LeftStickY>0) {
				if(!stickHeldDown){
					startSelectUp();
					stickHeldDown=true;
				}
			}	
			else if (player1.Action1.WasPressed) {
				if (textSelect == 0) {
					setupScreen.SetActive(true);
					startSceen.SetActive(false);
				}
			}
			else if(player1.LeftStickY==0){
				stickHeldDown=false;
			}
		}
		else if (setupScreen.activeSelf == true) {
			//Player 1 is Cartman ( for now )
			if (player1.Action1.WasPressed) {
				playerReady(0);
				Cartman.GetComponent<PlayerMovement>().setController(0);
			}
			//Player 2 is Kenny (for now)
			else if (player2.Action1.WasPressed) {
				playerReady(1);
				Kenny.GetComponent<PlayerMovement>().setController(1);
			}
			//Player 3 is Kyle (for now)
			/*else if (Input.GetKeyDown(KeyCode.Alpha3)) {
				playerReady (2);
			}
			//Player 4 is Stan (for now)
			else if (Input.GetKeyDown(KeyCode.Alpha4)) {
				playerReady (3);
			}*/

			if(numPlayersReady == 2) {
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
		Kenny.SetActive (true);
		Cartman.SetActive (true);
	}

}
