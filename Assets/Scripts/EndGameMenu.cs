using UnityEngine;
using System.Collections;
using InControl;

public class EndGameMenu : MonoBehaviour {

	public static EndGameMenu access;

	public InstantGuiWindow finalResultsMenu;
	public InstantGuiWindow newGameMenu;
	public InstantGuiElement[] playerReady;
	public InstantGuiElement[] newGameOptions;
	public InstantGuiList[] playerStatistics;

	private int currentMenuSelection = 0;
	
	private bool nextSelection = false;
	private bool previousSelection = false;

	private int numberOfPlayersReady = 0;

	// Use this for initialization
	void Start () {
		access = this;
		finalResultsMenu.gameObject.SetActive (false);
		newGameMenu.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (finalResultsMenu.gameObject.activeSelf == true) {
			if(InputManager.Devices[0].Action1.WasPressed){
				playerReady[0].text = "Waiting";
				numberOfPlayersReady++;
			}
			if(InputManager.Devices[1].Action1.WasPressed){
				playerReady[1].text = "Waiting";
				numberOfPlayersReady++;
			}
			/*if(InputManager.Devices[2].Action1.WasPressed){
				playerReady[2].text = "Waiting";
				numberOfPlayersReady++;
			}
			if(InputManager.Devices[3].Action1.WasPressed){
				playerReady[3].text = "Waiting";
				numberOfPlayersReady++;
			}*/

			if(numberOfPlayersReady == InputManager.Devices.Count){
				finalResultsMenu.gameObject.SetActive(false);
				newGameMenu.gameObject.SetActive(true);
				newGameOptions[0].check = true;
				currentMenuSelection = 0;
			}

		}

		if (newGameMenu.gameObject.activeSelf == true) {
			if (InputManager.Devices[0].LeftStickY < 0 && !nextSelection) {
				NextSelection ();
				nextSelection = true;
			}
			else if (InputManager.Devices[0].LeftStickY > 0 && !previousSelection) {
				PreviousSelection ();
				previousSelection = true;
			} 
			else if (!InputManager.Devices[0].LeftStickY) {
				nextSelection = false;
				previousSelection = false;
			} 

			if(InputManager.Devices[0].Action1.WasPressed){
				if(currentMenuSelection == 0){
					Application.LoadLevel(Application.loadedLevelName);
				}

				else if(currentMenuSelection == 1){
					Application.LoadLevel("_StartScreen");
				}
			}

		}
	}

	void NextSelection(){
		newGameOptions [currentMenuSelection++].check = false;
		currentMenuSelection = currentMenuSelection % newGameOptions.Length;
		newGameOptions [currentMenuSelection].check = true;
	}
	
	void PreviousSelection(){
		newGameOptions [currentMenuSelection--].check = false;
		
		if (currentMenuSelection == -1)
			currentMenuSelection = newGameOptions.Length - 1;
		else
			currentMenuSelection = currentMenuSelection % newGameOptions.Length;
		
		newGameOptions [currentMenuSelection].check = true;
	}

	public void EndOfGame(){
		finalResultsMenu.gameObject.SetActive (true);
		SetBasicStatistics();
		AdvancedPlayer1Statistics ();
		AdvancedPlayer2Statistics ();
		AdvancedPlayer3Statistics ();
		AdvancedPlayer4Statistics ();
	}

	private void SetBasicStatistics(){
		for (int i =1; i <= 4; i++) { 
			PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (i);
			playerStatistics [i-1].labels [0] += currentPlayer.ballsThrown.ToString ();
			playerStatistics [i-1].labels [1] += currentPlayer.successfulHits.ToString();
			print (currentPlayer.successfulHits);
			if(currentPlayer.ballsThrown == 0){
				playerStatistics [i-1].labels [2] += "0.00";
			}
			else{
				float hitPercentage = ((1.0f*currentPlayer.successfulHits)/currentPlayer.ballsThrown)*100f;
				playerStatistics [i-1].labels [2] += hitPercentage.ToString("F2");
			}
			playerStatistics [i-1].labels[3] += currentPlayer.hitByBall;
			playerStatistics [i-1].labels[4] += currentPlayer.timePossessedCube.ToString("F2");
		}

	}

	private void AdvancedPlayer1Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (1);
		playerStatistics [0].labels[5] += currentPlayer.hitByPlayer[1];
		playerStatistics [0].labels[6] += currentPlayer.hitByPlayer[2];
		playerStatistics [0].labels[7] += currentPlayer.hitByPlayer[3];
	}

	private void AdvancedPlayer2Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (2);
		playerStatistics [1].labels[5] += currentPlayer.hitByPlayer[0];
		playerStatistics [1].labels[6] += currentPlayer.hitByPlayer[2];
		playerStatistics [1].labels[7] += currentPlayer.hitByPlayer[3];
	}

	private void AdvancedPlayer3Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (3);
		playerStatistics [2].labels[5] += currentPlayer.hitByPlayer[0];
		playerStatistics [2].labels[6] += currentPlayer.hitByPlayer[1];
		playerStatistics [2].labels[7] += currentPlayer.hitByPlayer[3];
	}

	private void AdvancedPlayer4Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (4);
		playerStatistics [3].labels[5] += currentPlayer.hitByPlayer[0];
		playerStatistics [3].labels[6] += currentPlayer.hitByPlayer[1];
		playerStatistics [3].labels[7] += currentPlayer.hitByPlayer[2];
	}
	
}
