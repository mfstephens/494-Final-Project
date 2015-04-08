using UnityEngine;
using System.Collections;

public class PlayerStatistics : MonoBehaviour {

	private int ballsThrown = 0;
	private int successfulHits = 0;
	private int hitByBall = 0;
	private float timePossessedCube = 0;
	private int[] hitByPlayer;

	private bool isCubePossessed = false;

	// Use this for initialization
	void Start () {
		hitByPlayer = new int[4];
		for (int i = 0; i<4; i++) {
			hitByPlayer[i]=0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isCubePossessed == true) {
			timePossessedCube+=Time.deltaTime;
		}
	}

	public void HitByBall(int byPlayer){
		hitByBall++;
		hitByPlayer[byPlayer - 1]++;
	}

	public void BeginCubePosession(){
		isCubePossessed = true;
	}

	public void EndCubePosession(){
		isCubePossessed = false;
	}

	public void ThrewBall(){
		ballsThrown++;
	}
}
