using UnityEngine;
using System.Collections;

public class SFlashScript : MonoBehaviour {

	public GameObject[] flashSprites;

	public IEnumerator flash() {
		flashSprites [0].SetActive (true);
		// Wait 
		yield return new WaitForSeconds(0.05f);
		flashSprites [0].SetActive (false);
		flashSprites [1].SetActive (true);
		// Wait
		yield return new WaitForSeconds(0.05f);
		flashSprites [1].SetActive (false);
		flashSprites [2].SetActive (true);
		// Wait
		yield return new WaitForSeconds(0.05f);
		flashSprites [2].SetActive (false);
	}
	
}
