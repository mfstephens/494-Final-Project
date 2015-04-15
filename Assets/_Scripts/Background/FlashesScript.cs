using UnityEngine;
using System.Collections;

public class FlashesScript : MonoBehaviour {

	public GameObject[] largeFlashes;
	public GameObject[] smallFlashes;

	public bool isFlashing = false;

	// Use this for initialization
	void Start () {
		//flashAll ();
	}

	public void flashAll() {
		isFlashing = true;
	}

	public void stopFlashing() {
		isFlashing = false;
	}

	void FixedUpdate() {
		if (isFlashing) {
			int willFlash = Random.Range (0, 3);
			if (willFlash == 0) {
				int randomNumber = Random.Range (0, 3);
				LFlashScript cur = largeFlashes [randomNumber].GetComponent<LFlashScript> ();
				StartCoroutine (cur.flash ());
			}
		}
	}

}
