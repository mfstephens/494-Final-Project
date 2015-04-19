using UnityEngine;
using System.Collections;
using InControl;

public class StartMenu : MonoBehaviour {

	public InstantGuiWindow mainScreen;
	public InstantGuiWindow[] secondaryScreens;
	public InstantGuiButton[] menuOptions;
	public InstantGuiTabs controlTabs;
	public InstantGuiButton[] chooseGameOptions;

	private bool nextSelection = false;
	private bool previousSelection = false;

	private InputDevice player1;

	private int currentMainMenuSelection = 0;
	private int currentChooseGameSelection = 0;

	//Means we are currently on the main menu screen
	private float currentScreen = -1f;


	// Use this for initialization
	void Start () {
		player1 = InputManager.Devices[0];
		menuOptions[currentMainMenuSelection].check = true;
	}
	
	// Update is called once per frame
	void Update () {

		//Selections from the main screen
		if (currentScreen == -1) {
			if (player1.LeftStickY < 0 && !nextSelection) {
				NextSelection ();
				nextSelection = true;
			}
			else if (player1.LeftStickY > 0 && !previousSelection) {
				PreviousSelection ();
				previousSelection = true;
			} 
			else if (!player1.LeftStickY) {
				nextSelection = false;
				previousSelection = false;
			} 

			if (player1.Action1.WasPressed) {

				currentScreen = currentMainMenuSelection;

				if(currentScreen == 0){
					if (InputManager.Devices.Count == 1) {
						return;
					}
					
					if (InputManager.Devices.Count == 2){
						PassInfoOnLoad.gameNameToLoad = "_Two";
					}
					else if (InputManager.Devices.Count == 3){
						PassInfoOnLoad.gameNameToLoad = "_Three";
					}
					else if (InputManager.Devices.Count == 4) {
						PassInfoOnLoad.gameNameToLoad = "_Four";
					}
					Application.LoadLevel("_CharSelect");
				}
				else {
					menuOptions [currentMainMenuSelection].onPressed.Activate (menuOptions [currentMainMenuSelection]);
				}

				//currentScreen = currentMainMenuSelection;

				//Check if there's enough controllers connected to choose both options
//				if(currentScreen == 0){
//					if(InputManager.Devices.Count < 3)
//						chooseGameOptions[1].disabled = true;
//				}

			}
			return;
		}

		//Selections from the choose game menu
//		if (currentScreen == 0) {
//			chooseGameOptions[currentChooseGameSelection].check = true;
//			if(player1.LeftStickX != 0 && !nextSelection){
//				chooseGameOptions[currentChooseGameSelection].check = false;
//				currentChooseGameSelection = (currentChooseGameSelection + 1)%chooseGameOptions.Length;
//
//				//If next selection requires 3-4 players and not enough devices are connected, don't highlight that button
//				if(currentChooseGameSelection == 1){
//					if(chooseGameOptions[1].disabled == true){
//						currentChooseGameSelection = 0;
//					}
//				}
//
//				chooseGameOptions[currentChooseGameSelection].check = true;
//				nextSelection = true;
//			}
//			else if (!player1.LeftStickX){
//				nextSelection = false;
//				previousSelection = false;
//			}
//
//			//Player selects game to play
//			if(player1.Action1.WasPressed){
//				//1-2 Players Required
//				if(currentChooseGameSelection == 0){
//					PassInfoOnLoad.gameNameToLoad = "_OneToTwo";
//				}
//				else{
//					PassInfoOnLoad.gameNameToLoad = "_ThreeToFour";
//				}
//				Application.LoadLevel("_CharSelect");
//			}
//		}

		//Selections from the controller menu
		if (currentScreen == 1) {
			if (player1.RightTrigger.WasPressed || player1.LeftTrigger.WasPressed) {
				if(controlTabs.fields[0].gameObject.activeSelf == true)
					controlTabs.selected = 1;
				else
					controlTabs.selected = 0;
			}
		}

		//Back button to view previous screen
		if (player1.Action2.WasPressed) {
				DisableSecondaryScreens();
				mainScreen.gameObject.SetActive(true);
				currentScreen = -1;
		}
	}

	void NextSelection(){
		menuOptions [currentMainMenuSelection++].check = false;
		currentMainMenuSelection = currentMainMenuSelection % menuOptions.Length;
		menuOptions [currentMainMenuSelection].check = true;
	}

	void PreviousSelection(){
		menuOptions [currentMainMenuSelection--].check = false;

		if (currentMainMenuSelection == -1)
			currentMainMenuSelection = menuOptions.Length - 1;
		else
			currentMainMenuSelection = currentMainMenuSelection % menuOptions.Length;

		menuOptions [currentMainMenuSelection].check = true;
	}

	void DisableSecondaryScreens(){
		foreach (InstantGuiWindow window in secondaryScreens)
			window.gameObject.SetActive (false);
	}
}
