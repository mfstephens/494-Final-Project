using UnityEngine;
using System.Collections;
using InControl;

public class PauseMenu : MonoBehaviour {

	public static PauseMenu access;

	public InstantGuiWindow pause;
	public InstantGuiButton[] menuOptions;
	private int playerWhoSelectedPause = 0;
	private int currentMenuSelection = 0;

	private bool nextSelection = false;
	private bool previousSelection = false;

	// Use this for initialization
	void Start () {
		access = this;
		pause.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		InputDevice currentDevice = InputManager.Devices [playerWhoSelectedPause];
		if (pause.gameObject.activeSelf == true) {

			if (currentDevice.LeftStickY < 0 && !nextSelection) {
				NextSelection ();
				nextSelection = true;
			}
			else if (currentDevice.LeftStickY > 0 && !previousSelection) {
				PreviousSelection ();
				previousSelection = true;
			} 
			else if (!currentDevice.LeftStickY) {
				nextSelection = false;
				previousSelection = false;
			} 

			if(currentDevice.Action1.WasPressed){
				if(currentMenuSelection == 0){
					pause.gameObject.SetActive(false);
					Time.timeScale = 1;
					playerWhoSelectedPause = 0;
				}
				else if(currentMenuSelection == 1){

				}
				else if(currentMenuSelection == 2){
					Application.LoadLevel("_StartScreen");
				}
			}

			//Intuition is to press start again to resume
			if(currentDevice.MenuWasPressed){
				pause.gameObject.SetActive(false);
				Time.timeScale = 1;
				playerWhoSelectedPause = 0;
			}
		}
	}

	void NextSelection(){
		menuOptions [currentMenuSelection++].check = false;
		currentMenuSelection = currentMenuSelection % menuOptions.Length;
		menuOptions [currentMenuSelection].check = true;
	}
	
	void PreviousSelection(){
		menuOptions [currentMenuSelection--].check = false;
		
		if (currentMenuSelection == -1)
			currentMenuSelection = menuOptions.Length - 1;
		else
			currentMenuSelection = currentMenuSelection % menuOptions.Length;
		
		menuOptions [currentMenuSelection].check = true;
	}

	public void PlayerPausedGame(int player){
		pause.gameObject.SetActive (true);
		playerWhoSelectedPause = player;
		Time.timeScale = 0;
		menuOptions [0].check = true;
	}
}
