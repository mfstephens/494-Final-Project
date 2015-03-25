using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlagBehavior : MonoBehaviour {
	
	public Color oppColor, origColor;
	private float timeColored = 0;
	private bool scored = false;
	public Text oppScoreText;
	private int oppScore = 0;

	// Use this for initialization
	void Start () {
		colorFlagOrig();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (scored) {
			oppScore++;
			oppScoreText.text = oppScore.ToString();
		}
	}

	void OnTriggerEnter(Collider other) {
		CaptureTheFlagMode.access.teamScored (other.gameObject, this.gameObject);
	}

	void OnTriggerExit(Collider other) {

	}

	public void colorFlagOpp() {
		this.gameObject.GetComponent<Renderer>().material.color = oppColor;
		scored = true;
	}

	public void colorFlagOrig() {
		this.gameObject.GetComponent<Renderer>().material.color = origColor;
		scored = false;
	}
}
