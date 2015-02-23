using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {
	public float throwSpeed = 25f;
	public float orbitSpeed = 300f;
	public bool throwBall = false; //player script should set this to true every throw
	public GameObject team1Player1, team1Player2, team2Player1, team2Player2; //player script set these
	public GameObject playerInPossession; // player script should set this to the player currently has ball

	Vector3 velocity;

	// Use this for initialization
	void Start () {
		velocity = new Vector3 (0f, 0f, 0f);
		throwSpeed *= transform.lossyScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		if(playerInPossession != null){
			transform.RotateAround(playerInPossession.transform.position, Vector3.up, orbitSpeed * Time.deltaTime);
		}
	
	
}

	void FixedUpdate(){
		if(throwBall){
			if(playerInPossession == null){return;}
			float xDist, yDist;
			Transform throwTo = transform; // initialize to keep compiler happy
			if(playerInPossession == team1Player1) {throwTo = team1Player2.transform;}
			else if(playerInPossession == team1Player2) {throwTo = team1Player1.transform;}
			else if(playerInPossession == team2Player1) {throwTo = team2Player2.transform;}
			else {throwTo = team2Player1.transform;}
			xDist = throwTo.position.x - transform.position.x;
			yDist = throwTo.position.y - transform.position.y;
			float angle = (Mathf.Atan(yDist / xDist));
			float xVel, yVel;
			xVel = throwSpeed * Mathf.Cos (angle);
			yVel = throwSpeed * Mathf.Sin (angle);
			if(xDist < 0) xVel = -xVel;
			if(yDist < 0) yVel = -yVel;
			Vector3 temp = transform.position;
			temp.z = throwTo.position.z;
			transform.position = temp;
			velocity = new Vector3(xVel, yVel, 0f);
			playerInPossession = null;
			throwBall = false;
		}
		transform.position += velocity * Time.fixedDeltaTime;
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Ground"){


		}

	}
	
	
}
