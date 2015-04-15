using UnityEngine;
using System.Collections;

public class BountyDisplay : MonoBehaviour {

	private PlayerController control;
	private PlayerMove move;
	public float score;
	TextMesh myText;
	Vector3 forward;

	// Use this for initialization
	void Start () {
		control = GetComponentInParent<PlayerController> ();
		move = GetComponentInParent<PlayerMove> ();
		myText = GetComponent<TextMesh> ();
		myText.color = control.playerColor;
		this.gameObject.SetActive (false);
		forward = this.transform.eulerAngles;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	void Update() {


		if (FlagRotate.access.currentPlayer == (move.playerColor - 1)) {
			transform.localPosition = new Vector3 (transform.localPosition.x, 40, transform.localPosition.z);
		} else {
			transform.localPosition = new Vector3 (transform.localPosition.x, 28, transform.localPosition.z);

		}

		this.transform.eulerAngles = forward;

	}

	public void addScore(int score) {
		myText.text = "+" + score.ToString ();
		this.gameObject.SetActive (true);
		Invoke ("hideScore", 2f);
	}

	void hideScore() {
		this.gameObject.SetActive (false);
	}

}
