using UnityEngine;
using System.Collections;

public class ballDestroy : MonoBehaviour {

	private float start;

	// Use this for initialization
	void Start () {
		
	}

	void Awake() {
		start = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - start > 2) {

			if (unlimitedBallPowerUp.access.currentPlayer != null && unlimitedBallPowerUp.access.currentPlayer.gameObject.GetComponent<PlayerController>().possessedBall == this.gameObject.GetComponent<Ball>()) {
				unlimitedBallPowerUp.access.currentPlayer.gameObject.GetComponent<PlayerController>().possessedBall = null;
			}

			BallContainer.BallContainerSingleton.ballContainer.Remove(this.gameObject.GetComponent<Ball>());
			Destroy(this.gameObject);

		}
	}
}
