using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public float speed = 10.0f;
	public float jump = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			rigidbody.AddForce (new Vector3 (0, jump, 0));
		}
	}

	void FixedUpdate() {
		float move = Input.GetAxis ("Horizontal");
		if (move != 0) {
			rigidbody.velocity = new Vector2 (move * speed, rigidbody.velocity.y);
		}
	}
}
