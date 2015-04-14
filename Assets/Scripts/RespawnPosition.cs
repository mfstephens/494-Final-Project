using UnityEngine;
using System.Collections;

public class RespawnPosition : MonoBehaviour {

	public static RespawnPosition access;

	public GameObject flag;
	private Vector3[] respawnPoints;
	private Vector3 lastRespawnPoint = Vector3.zero;

	// Use this for initialization
	void Start () {
//		respawnPoints = new Vector3[3, 5] { { new Vector3(-675.6f, -116.4f, 5f), new Vector3(-675.6f, -13.6f, 5f), new Vector3(-675.6f, 88.7f, 5f), new Vector3(-632.5f, 37.9f, 5f), new Vector3(-632.5f, -60.6f, 5f) }, 
//			{ new Vector3(-155.4f, -116.4f, 5f), new Vector3(-155.4f, -13.6f, 5f), new Vector3(-155.4f, 88.7f, 5f), new Vector3(-110.3f, 37.9f, 5f), new Vector3(-110.3f, -60.6f, 5f) },
//			{ new Vector3(418.7f, -116.4f, 5f), new Vector3(418.7f, -13.6f, 5f), new Vector3(418.7f, 88.7f, 5f), new Vector3(375.3f, 37.9f, 5f), new Vector3(375.3f, -60.6f, 5f) } };
		respawnPoints = new Vector3[2];
		respawnPoints [0] = new Vector3 (-149f, 158f, 5f);
		respawnPoints [1] = new Vector3 (-113f, 158f, 5f);
		access = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 generateRespawnPoint() {
		Vector3 nextRespawnPoint = lastRespawnPoint;

		while (nextRespawnPoint == lastRespawnPoint) {
			// if the flag is in the middle

			int location = Random.Range(0,2);

//			int tower = Random.Range(0,3);
//			int platform = Random.Range(0,5);
			// if the flag is on one of the sides
			//nextRespawnPoint = respawnPoints[tower,platform];
			nextRespawnPoint = respawnPoints[location];
		}

		lastRespawnPoint = nextRespawnPoint;
		return nextRespawnPoint;
	}
}
