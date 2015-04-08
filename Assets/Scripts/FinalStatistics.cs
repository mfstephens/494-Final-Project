using UnityEngine;
using System.Collections;

public class FinalStatistics : MonoBehaviour {

	public static FinalStatistics finalStatistics;
	
	private PlayerStatistics player1Stats;
	private PlayerStatistics player2Stats;
	private PlayerStatistics player3Stats;
	private PlayerStatistics player4Stats;

	// Use this for initialization
	void Start () {
		finalStatistics = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerHitByBall(int playerHit,int byPlayer){

		switch (playerHit) {
			case 1:
				player1Stats.HitByBall(byPlayer);
				break;
			case 2:
				player2Stats.HitByBall(byPlayer);
				break;
			case 3:
				player3Stats.HitByBall(byPlayer);
				break;
			case 4:
				player4Stats.HitByBall(byPlayer);
				break;
			default:
				break;

		}
	}

	public void BeginCubePosession(int playerNumber){
		switch (playerNumber) {
		case 1:
			player1Stats.BeginCubePosession();
			break;
		case 2:
			player2Stats.BeginCubePosession();
			break;
		case 3:
			player3Stats.BeginCubePosession();
			break;
		case 4:
			player4Stats.BeginCubePosession();
			break;
		default:
			break;
			
		}
	}

	public void EndCubePosession(int playerNumber){
		switch (playerNumber) {
		case 1:
			player1Stats.EndCubePosession();
			break;
		case 2:
			player2Stats.EndCubePosession();
			break;
		case 3:
			player3Stats.EndCubePosession();
			break;
		case 4:
			player4Stats.EndCubePosession();
			break;
		default:
			break;
			
		}
	}
}
