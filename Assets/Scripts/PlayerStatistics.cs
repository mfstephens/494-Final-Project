using UnityEngine;
using System.Collections;

public class PlayerStatistics : MonoBehaviour {

	public int ballsThrown = 0;
	public int successfulHits = 0;
	public int hitByBall = 0;
	public float timePossessedCube = 0;
	public int[] hitPlayer = new int[]{0,0,0,0};
	public int crownKills = 0;
	

	public void HitByPlayer(){
		hitByBall++;
	}

	public void HitPlayer(int player){
		hitPlayer [player - 1]++;
	}

	public void AddTimePossession(float time){
		timePossessedCube += time;
	}

	public void ThrewBall(){
		ballsThrown++;
	}

	public void SuccessfulHit(){
		successfulHits++;
	}

	public void KilledCrownLeader(){
		crownKills++;
	}
}
