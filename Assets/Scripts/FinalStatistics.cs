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
		player1Stats = new PlayerStatistics();
		player2Stats = new PlayerStatistics();
		player3Stats = new PlayerStatistics();
		player4Stats = new PlayerStatistics();
		print ("Yo nigga");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayerThrewBall(int player){

		switch (player) {
			case 1:
				player1Stats.ThrewBall();
				break;
			case 2:
				player2Stats.ThrewBall();
				break;
			case 3:
				player3Stats.ThrewBall();
				break;
			case 4:
				player4Stats.ThrewBall();
				break;
			default:
				break;
		}

	}

	public void PlayerHitByBall(int playerHit,int byPlayer){
		print ("Player Hit: " + playerHit);
		print ("By Player: " + byPlayer);
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

		switch (byPlayer) {
			case 1:
				player1Stats.SuccessfulHit();
				break;
			case 2:
				player2Stats.SuccessfulHit();
				break;
			case 3:
				player3Stats.SuccessfulHit();
				break;
			case 4:
				player4Stats.SuccessfulHit();
				break;
			default:
				break;
		}
	}

	public void AddCubePosession(int playerNumber,float time){
		switch (playerNumber) {
		case 1:
			player1Stats.AddTimePossession(time);
			break;
		case 2:
			player2Stats.AddTimePossession(time);
			break;
		case 3:
			player3Stats.AddTimePossession(time);
			break;
		case 4:
			player4Stats.AddTimePossession(time);
			break;
		default:
			break;
			
		}
	}

	public PlayerStatistics getPlayer(int playerNumber){

		switch (playerNumber) {
			case 1:
				return player1Stats;
				break;
			case 2:
				return player2Stats;
				break;
			case 3:
				return player3Stats;
				break;
			case 4:
				return player4Stats;
				break;
			default:
				return player1Stats;
				break;

		}
	}
}
