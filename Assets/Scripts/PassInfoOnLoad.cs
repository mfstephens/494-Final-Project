using UnityEngine;
using System.Collections;
using InControl;

public class PassInfoOnLoad : MonoBehaviour {

	// set in StartMenu and used in characterSelection
	public static string gameNameToLoad;
	// set in characterSelection and used in start sequence of game to set 
	// each robot's main material/color
	public static Color[] playerColor;

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
		playerColor = new Color[4];
	}

}
