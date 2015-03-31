using UnityEngine;
using System.Collections;

public class RespawnPositionTwo : MonoBehaviour {
	
	public static RespawnPositionTwo access;
	
	private Vector3[,] respawnPoints;
	private Vector3 lastRespawnPoint = Vector3.zero;
	
	// Use this for initialization
	void Start () {
		respawnPoints = new Vector3[2, 5] { { new Vector3(-423f, -116.4f, 5f), new Vector3(-423f, -13.6f, 5f), new Vector3(-423f, 88.7f, 5f), new Vector3(-374f, 37.9f, 5f), new Vector3(-374f, -60.6f, 5f) }, 
			{ new Vector3(423f, -116.4f, 5f), new Vector3(423f, -13.6f, 5f), new Vector3(423f, 88.7f, 5f), new Vector3(381, 37.9f, 5f), new Vector3(-381f, -60.6f, 5f) } };
		
		access = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public Vector3 generateRespawnPoint() {

		
		int tower = Random.Range (0, 2);
		int platform = Random.Range(0,5);
		Vector3 nextRespawnPoint = respawnPoints[tower,platform];
		print (nextRespawnPoint);
		return nextRespawnPoint;
	}
}
