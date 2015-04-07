using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using InControl;

public class characterSelection : MonoBehaviour {
	GameObject[] selections; // The 8 different character selections

	// In the following arrays, the index corresponds to the player.
	// [0] = player1, [1] = player2, [2] = player3, and [3] = player4
	GameObject[] player_cursors; // each player's cursor object
	bool[] cursorMoveHoriz_ready; // player must release joystick before moving cursor to new selection
	bool[] cursorMoveVert_ready;
	int[] selections_occupied; // keeps track of which selection each player is currently at
	bool[] players_ready; // the players that have locked in their selection

	int numPlayers;
	float joystick_threshold = .5f;
	float offsetLeft = -13.5f; // offsets for setting the cursor position relative to the character selections
	float offsetDown = -9f;
	SpriteRenderer[] sp;
	InstantGuiButton readyButton;



	// Use this for initialization
	void Start () {
		numPlayers = InputManager.Devices.Count;
		selections = new GameObject[8];
		selections_occupied = new int[numPlayers];
		players_ready = new bool[numPlayers];
		cursorMoveHoriz_ready = new bool[numPlayers];
		cursorMoveVert_ready = new bool[numPlayers];
		player_cursors = new GameObject[4];
		player_cursors[0] = GameObject.Find ("cursorP1");
		player_cursors[1] = GameObject.Find ("cursorP2");
		player_cursors[2] = GameObject.Find ("cursorP3");
		player_cursors[3] = GameObject.Find ("cursorP4");

		for (int i = 0; i < selections.Length; i++) {
			selections[i] = GameObject.Find("Selection" + i);
		}

		for (int i = 0; i < player_cursors.Length; i++) {
			sp = player_cursors[i].GetComponentsInChildren<SpriteRenderer>();
			for(int j = 0; j < sp.Length; j++){
				sp[j].enabled = false;
			}
		}

		for (int i = 0; i < numPlayers; i++) {
			selections_occupied[i] = i;
			players_ready[i] = false;
			cursorMoveHoriz_ready[i] = true;
			cursorMoveVert_ready[i] = true;
			sp = player_cursors[i].GetComponentsInChildren<SpriteRenderer>();
			for(int j = 0; j < sp.Length; j++){
				if(sp[j].name != "checkMark") sp[j].enabled = true;
			}
			float locX = selections [i].transform.position.x;
			float locY = selections [i].transform.position.y;
			float locZ = selections [i].transform.position.z;
			player_cursors[i].transform.position = new Vector3 (locX + offsetLeft, locY + offsetDown, locZ);
		}

		readyButton = GameObject.Find ("Ready!").GetComponent<InstantGuiButton> ();
		readyButton.disabled = true;
		readyButton.pressed = false;
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal, vertical;
		int numReady = 0;
		for(int i = 0; i < numPlayers; i++) if(players_ready[i])numReady++;
		if (numReady == numPlayers) {
			readyButton.disabled = false;
			readyButton.pressed = true;
			for (int i = 0; i < numPlayers; i++) {
				if(InputManager.Devices[i].Action1.WasPressed) startGame_Handler();
				if(InputManager.Devices[i].Action2.WasPressed){
					readyButton_Handler(i, false);
					readyButton.disabled = true;
					readyButton.pressed = false;
					break;
				}
			}
			return;
		}
		// go through every player's input and update selection choices & associated data structures
		for (int i = 0; i < numPlayers; i++) {
			if(InputManager.Devices[i].Action1.WasPressed) readyButton_Handler(i, true);
			if(InputManager.Devices[i].Action2.WasPressed) readyButton_Handler(i, false);
			horizontal = InputManager.Devices[i].LeftStickX;
			vertical = InputManager.Devices[i].LeftStickY;
			if(Mathf.Abs(horizontal) < .3f) cursorMoveHoriz_ready[i] = true;
			if(Mathf.Abs(vertical) < .3f) cursorMoveVert_ready[i] = true;
			if(cursorMoveHoriz_ready[i]){
				if(horizontal > joystick_threshold) SelectLeft_or_Right_Handler(i, true);
				else if(horizontal < -joystick_threshold) SelectLeft_or_Right_Handler(i, false);
			}
			if(cursorMoveVert_ready[i]){
				if(vertical > joystick_threshold) SelectUp_or_Down_Handler(i, true); 
				else if(vertical < -joystick_threshold) SelectUp_or_Down_Handler(i, false);
			}
		}
	}

	void startGame_Handler(){
		Material[] meshMats;
		for (int i = 0; i < numPlayers; i++) {
			meshMats = selections[selections_occupied[i]].GetComponentInChildren<SkinnedMeshRenderer>().materials;
			for(int j = 0; j < meshMats.Length; j++){
				if (meshMats[j].name != "Glow" && meshMats[j].name != "Black"){
					PassInfoOnLoad.playerColor[i] = meshMats[j].color;
					break;
				}
			}
		}
		Application.LoadLevel(PassInfoOnLoad.gameNameToLoad);
	}

	void readyButton_Handler(int playerNum, bool readyButton){
		sp = player_cursors[playerNum].GetComponentsInChildren<SpriteRenderer>();
		// player was ready, now isn't
		if (players_ready[playerNum] && !readyButton) {
			for(int j = 0; j < sp.Length; j++){
				if(sp[j].name != "checkMark") sp[j].enabled = true;
				else sp[j].enabled = false;
			}
			players_ready[playerNum] = false;
		} else if(!players_ready[playerNum] && readyButton) { // player wasn't ready, but is now
			for(int j = 0; j < sp.Length; j++){
				if(sp[j].name != "checkMark") sp[j].enabled = false;
				else sp[j].enabled = true;
			}
			players_ready[playerNum] = true;
		}
	} // end readyButton_Handler

	void SelectLeft_or_Right_Handler(int playerNum, bool isRightSelect){
		// update player's cursor status
		cursorMoveHoriz_ready[playerNum] = false;

		// Loop until a selection to the isRightSelect is found that is not occupied
		// or return if there is none
		int nextSelection = selections_occupied[playerNum] - 1; 
		if(isRightSelect) nextSelection = selections_occupied[playerNum] + 1; 
		while (true) {
			bool found = true;
			// if no selection to the isRightSelect exists then return
			if(!isRightSelect){
				if (nextSelection < 0 || nextSelection == 3) return;
			}
			else {
				if (nextSelection > 7 || nextSelection == 4) return;
			}

			// loop through all players to see if anyone is occupying 
			// the selection to the isRightSelect
			for(int i = 0; i < numPlayers; i++){
				if(selections_occupied[i] == nextSelection){
					found = false;
					break;
				}
			}

			if(found) break;
			else {
				if(!isRightSelect) nextSelection--;
				else nextSelection++;
			}
		} // end while

		// move player's cursor to new selection
		float locX = selections [nextSelection].transform.position.x;
		float locY = selections [nextSelection].transform.position.y;
		float locZ = selections [nextSelection].transform.position.z;
		player_cursors [playerNum].transform.position = new Vector3 (locX + offsetLeft, locY + offsetDown, locZ);

		// mark new selection as now occupied
		selections_occupied[playerNum] = nextSelection;

	} //end SelectLeft_or_Right_Handler

	
	void SelectUp_or_Down_Handler(int playerNum, bool isUpSelect){
		// update player's cursor status
		cursorMoveVert_ready[playerNum] = false;

		// check to see if there exsists a selection that is 
		// above(if isUpSelect) or below (if !isUpSelect). Return if there is none
		int nextSelection;
		if (isUpSelect) {
			nextSelection = selections_occupied [playerNum] - 4;
			if (nextSelection < 0) return;
		} else {
			nextSelection = selections_occupied [playerNum] + 4;
			if (nextSelection > 7) return;
		}

		// loop through all players to see if anyone is occupying 
		// the selection to the isUpSelect. Return if occupied
		for(int i = 0; i < numPlayers; i++){
			if(selections_occupied[i] == nextSelection){
				return;
			}
		}

		// if made it this far then selection exists and is not occupied

		// move player's cursor to new selection
		float locX = selections [nextSelection].transform.position.x;
		float locY = selections [nextSelection].transform.position.y;
		float locZ = selections [nextSelection].transform.position.z;
		player_cursors [playerNum].transform.position = new Vector3 (locX + offsetLeft, locY + offsetDown, locZ);
		
		// mark new selection as now occupied
		selections_occupied[playerNum] = nextSelection;
		
	} //end SelectUp_or_Down_Handler
}
