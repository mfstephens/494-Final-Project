using UnityEngine;
using System.Collections;

public class ScoreBoard : MonoBehaviour {

	private struct playerAndScore{
		public int player;
		public int score;
	};

	public static ScoreBoard scoreBoard;

	public InstantGuiWindow[] playerScoreboard;
	public InstantGuiElement[] playerScores;
	public InstantGuiElement[] playerRank;

	void Awake() {
		scoreBoard = this;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){

	}

	public void setPlayerWindowColor(int player,Color windowColor){
		playerScoreboard [player].mainGuiTexture.color = windowColor;
	}

	public void setPlayerColor(int player,Color playerColor){
		playerScoreboard [player].style.main.textColor = playerColor;

		EndGameMenu.access.setPlayerColor (player, playerColor);
	}

	public void setPlayerScore(int player,string score){
		playerScores [player].text = score;
	}

	public string getPlayerScore(int player){
		return playerScores[player].text;
	}

	public void setPlayerRank(int[] tempScores){

//		ArrayList tempContainer = new ArrayList ();
//		for (int i = 0; i < tempScores.Length; i++) {
//			playerAndScore temp;
//			temp.score = tempScores[i];
//			temp.player = i;
//			tempContainer.Add(temp);
//		}
//
//		tempContainer.Sort ();
//
//		int currentRank = 1;
//		foreach (playerAndScore i in tempContainer) {
//			playerRank[i.player].text = currentRank.ToString();
//			currentRank++;
//		}

	}
}
