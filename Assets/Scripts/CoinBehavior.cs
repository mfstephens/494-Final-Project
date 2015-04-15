using UnityEngine;
using System.Collections;

public class CoinBehavior : MonoBehaviour {

	public GameObject owner;
	private HingeJoint playerAttach;
	private float smoothTime = 5f;
	private float orbitSpeed = 300;
	private float moveToSpeed = 400f;
	public Vector3 randomLocation;
	private float lastChange = 0;
	private bool killed = false;
	GameObject otherPlayer;

	// Use this for initialization
	void Start () {
		killed = false;
	}

	void Awake() {
		killed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (killed) {
			if (Vector3.Distance(this.transform.position,otherPlayer.transform.position) < 5f) {
				Destroy(this.gameObject);
			}
			else {
				this.transform.position = Vector3.MoveTowards(this.transform.position, otherPlayer.transform.position, Time.deltaTime * moveToSpeed);
			}
			return;
		}

		if (Vector3.Distance (this.transform.position, owner.transform.position) > 18f) {
			smoothTime = 10f;
		}
		else {
			smoothTime = 1f;
			if (Time.time - lastChange > 0.5f) {
				lastChange = Time.time;
				randomLocation = generateRandomLoc();
			}
		}
	}

	void LateUpdate()
	{
		if (!killed) {
			this.transform.position = Vector3.Lerp (this.transform.position, owner.transform.position + randomLocation, Time.deltaTime * smoothTime);
		}	
	}

	public void assignPlayer(GameObject player) {
		owner = player;
		randomLocation = generateRandomLoc ();
	}

	Vector3 generateRandomLoc() {
		int negPos = Random.Range (0, 2);
		if (negPos == 0) {
			return new Vector3 (Random.Range (5, 18f), Random.Range (8, 18), 0);
		} 
		else {
			return new Vector3 (Random.Range (-5, -18f), Random.Range (8, 18), 0);
		}
	}

	public void playerHit(GameObject player) {
		killed = true;
		otherPlayer = player;
		print (otherPlayer.name);
	}
}
