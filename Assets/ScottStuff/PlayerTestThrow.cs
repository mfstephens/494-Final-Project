using UnityEngine;
using System.Collections;

public class PlayerTestThrow : MonoBehaviour {
	public GameObject ball;
	bool grounded = true;
	Vector3 vel;
	public float jumpVel = 30f;
	public float hSpeed = 30f;
	public float ballPickupDist = 8f;
	public bool possession = false;
	// Use this for initialization
	void Start () {
		grounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		vel = rigidbody.velocity;
		if (Input.GetAxis ("Fire1") != 0f && possession){
			ball.GetComponent<BallController> ().throwBall = true;
			possession = false;
		}

		// Horizontal movement
		float vX = Input.GetAxis ("Horizontal"); // Returns a number [-1..1]
		vel.x = vX * hSpeed;

		if (Input.GetKeyDown (KeyCode.Space)  && grounded) {
			vel.y = jumpVel;
		}
		
		// set direction to face
		if (vel.x > 0)
			transform.eulerAngles = new Vector3 (0, 0, 0);
		if (vel.x < 0)
			transform.eulerAngles = new Vector3 (0, 180, 0);
		
		transform.position += vel * Time.deltaTime;
	
	}

	void OnCollisionEnter(Collision other){
		if(other.gameObject == ball){
			if(ball.GetComponent<BallController>().playerInPossession != null) return;
			possession = true;
			ball.GetComponent<BallController>().playerInPossession = gameObject;
		}
		
		
	}


	void OnTriggerEnter(Collider other){
		if(other.gameObject == ball){
			if(ball.GetComponent<BallController>().playerInPossession != null) return;
			possession = true;
			ball.GetComponent<BallController>().playerInPossession = gameObject;
		}


	}

}
