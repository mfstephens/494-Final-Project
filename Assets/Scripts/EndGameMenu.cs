using UnityEngine;
using System.Collections;
using InControl;

public class EndGameMenu : MonoBehaviour {

	public static EndGameMenu access;

	public InstantGuiWindow finalResultsMenu;
	public InstantGuiWindow newGameMenu;
	public InstantGuiButton[] playerReady;
	public InstantGuiElement[] newGameOptions;
	public InstantGuiList[] playerStatistics;
	public InstantGuiElement[] playerColors;

	private int currentMenuSelection = 0;
	
	private bool nextSelection = false;
	private bool previousSelection = false;

	private int numberOfPlayersReady = 0;

	void Awake(){
		access = this;
	}

	// Use this for initialization
	void Start () {
		finalResultsMenu.gameObject.SetActive (false);
		newGameMenu.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (finalResultsMenu.gameObject.activeSelf == true) {
			if(InputManager.Devices[0].Action1.WasPressed){
				playerReady[0].check = true;
				numberOfPlayersReady++;
			}
			if(InputManager.Devices[1].Action1.WasPressed){
				playerReady[1].check = true;
				numberOfPlayersReady++;
			}
			if(InputManager.Devices[2].Action1.WasPressed){
				playerReady[2].check = true;
				numberOfPlayersReady++;
			}
			if(InputManager.Devices[3].Action1.WasPressed){
				playerReady[3].check = true;
				numberOfPlayersReady++;
			}

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
					Time.timeScale = 1;
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

		if(InputManager.Devices.Count >= 3)
			AdvancedPlayer3Statistics ();

		if(InputManager.Devices.Count == 4)
			AdvancedPlayer4Statistics ();
	}

	private void SetBasicStatistics(){
		for (int i = 1; i <= InputManager.Devices.Count; i++) { 
			PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (i);
			playerStatistics [i-1].labels[0] += FlagRotate.access.getScoreForPlayer(i-1);
			playerStatistics [i-1].labels [1] += currentPlayer.ballsThrown.ToString ();
			playerStatistics [i-1].labels [2] += currentPlayer.successfulHits.ToString();
			print (currentPlayer.successfulHits);
			if(currentPlayer.ballsThrown == 0){
				playerStatistics [i-1].labels [3] += "0.00";
			}
			else{
				float hitPercentage = ((1.0f*currentPlayer.successfulHits)/currentPlayer.ballsThrown)*100f;
				playerStatistics [i-1].labels [3] += hitPercentage.ToString("F2");
			}
			playerStatistics [i-1].labels[4] += currentPlayer.hitByBall;
			playerStatistics [i-1].labels[5] += currentPlayer.timePossessedCube.ToString("F2");
			playerStatistics [i-1].labels[6] += currentPlayer.crownKills.ToString();
		}

	}

	private void AdvancedPlayer1Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (1);
		playerStatistics [0].labels[7] += currentPlayer.hitPlayer[1];
		playerStatistics [0].labels[8] += currentPlayer.hitPlayer[2];
		playerStatistics [0].labels[9] += currentPlayer.hitPlayer[3];

		playerColors [0].text = "Player 1";
	}

	private void AdvancedPlayer2Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (2);
		playerStatistics [1].labels[7] += currentPlayer.hitPlayer[0];
		playerStatistics [1].labels[8] += currentPlayer.hitPlayer[2];
		playerStatistics [1].labels[9] += currentPlayer.hitPlayer[3];

		playerColors [1].text = "Player 2";
	}

	private void AdvancedPlayer3Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (3);
		playerStatistics [2].labels[7] += currentPlayer.hitPlayer[0];
		playerStatistics [2].labels[8] += currentPlayer.hitPlayer[1];
		playerStatistics [2].labels[9] += currentPlayer.hitPlayer[3];

		playerColors [2].text = "Player 3";
	}

	private void AdvancedPlayer4Statistics(){
		PlayerStatistics currentPlayer = FinalStatistics.finalStatistics.getPlayer (4);
		playerStatistics [3].labels[7] += currentPlayer.hitPlayer[0];
		playerStatistics [3].labels[8] += currentPlayer.hitPlayer[1];
		playerStatistics [3].labels[9] += currentPlayer.hitPlayer[2];

		playerColors [3].text = "Player 4";
	}

	public void setPlayerColor(int player,Color playerColor){
		print ("Set Color for player " + player + playerColor.ToString());
		playerColors [player].style.main.textColor = playerColor;
	}
}
