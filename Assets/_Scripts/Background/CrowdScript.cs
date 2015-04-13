using UnityEngine;
using System.Collections;

public class CrowdScript : MonoBehaviour {

	public GameObject[] heads;

	private bool move = true;

	// Use this for initialization
	void Start() {
	
	}
	
	void FixedUpdate() {
		crowdMove ();
	}
	
	void setMove (bool newMove) {
		move = newMove;
	}

	void crowdMove() {
		if (move == true) {
			// Random number to determine if a head will be toggled
			int willToggle = Random.Range (0,9);
			if (willToggle == 0) {
		
				// Random number to pick the head that is toggled
				int randomNumber = Random.Range (0, 54);

				if (heads [randomNumber].activeSelf == true) {
						heads [randomNumber].SetActive (false);
				} else {
						heads [randomNumber].SetActive (true);
				}
			}
		}	
	}
}
