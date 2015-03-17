using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallContainer : MonoBehaviour {

	public static BallContainer BallContainerSingleton;

	public List<Ball>ballContainer;
	public Ball player1Ball;
	public Ball player2Ball;
	public Ball player3Ball;
	public Ball player4Ball;

	// Use this for initialization
	void Start () {
		BallContainerSingleton = this;
		ballContainer = new List<Ball>();
		instantiateBallForPlayerAtPosition (1,new Vector3(2f,29f,5f));
		instantiateBallForPlayerAtPosition (2,new Vector3(243f,31f,5f));
		instantiateBallForPlayerAtPosition (3, new Vector3 (200f, 31f, 5f));
		instantiateBallForPlayerAtPosition (4, new Vector3 (202f, 31f, 5f));
	}

	//Instantiates a ball that corresponds to a player (their color) at a position
	void instantiateBallForPlayerAtPosition(int i,Vector3 position){
		if (i == 1) {
			Ball newBall=Instantiate (player1Ball, position, Quaternion.identity) as Ball;
			newBall.playerColor = 1;
			ballContainer.Add (newBall);
		}
		if (i == 2) {
			Ball newBall=Instantiate (player2Ball, position,Quaternion.identity) as Ball;
			newBall.playerColor = 2;
			ballContainer.Add (newBall);
		}
		if (i == 3) {
			Ball newBall=Instantiate (player3Ball, position, Quaternion.identity) as Ball;
			newBall.playerColor = 3;
			ballContainer.Add (newBall);
		}
		if (i == 4) {
			Ball newBall=Instantiate (player4Ball, position, Quaternion.identity) as Ball;
			newBall.playerColor = 4;
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
