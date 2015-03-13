using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallContainer : MonoBehaviour {

	public static BallContainer BallContainerSingleton;

	private List<Ball>ballContainer;
	public Ball player1Ball;
	public Ball player2Ball;

	// Use this for initialization
	void Start () {
		BallContainerSingleton = this;
		ballContainer = new List<Ball>();
		instantiateBallForPlayer (1);
		instantiateBallForPlayer (2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void instantiateBallForPlayer(int i){
		if (i == 1) {
			Ball newBall=Instantiate (player1Ball, new Vector3 (2f, 29f, 5f), Quaternion.identity) as Ball;
			ballContainer.Add (newBall);
		}
		if (i == 2) {
			Ball newBall=Instantiate (player2Ball, new Vector3(243f,31f,5f),Quaternion.identity) as Ball;
			ballContainer.Add (newBall);
		}
	}

	//Determines what ball is closest to a given position
	public Ball closestBallToPosition(Vector3 position){
		Ball closestBall = null;
		float shortestDistance = 0;
		for(int i = 0; i<ballContainer.Count; i++){
			float distance = Vector3.Distance(position,ballContainer[i].transform.position);
			if(closestBall == null){
				shortestDistance = distance;
				closestBall = ballContainer[i];
			}
			else if(shortestDistance > distance){
				shortestDistance = distance;
				closestBall = ballContainer[i];
			}
		}
		return closestBall;
	}

	//Destroy Ball and remove from list
	public void destroyBall(Ball ball){
		for(int i = 0; i<ballContainer.Count; i++){
			if(ball == ballContainer[i]){
				Destroy (ball.gameObject);
				break;
			}

		}
	}
}
