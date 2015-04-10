using UnityEngine;
using System.Collections;

enum ItemBlockDirection {
	down, up
}

public class ItemBlock : MonoBehaviour {

	float spinSpeed = 50f;
	Vector3 origin;
	float bounceSpeed = 0.25f;
	float bounceDistance = 5f;
	ItemBlockDirection direction;
	PowerupType type;
	PowerupController controller;

	void Start () {
		origin = transform.position;
		direction = ItemBlockDirection.up;
		controller = GameObject.Find ("Main Camera").GetComponent<PowerupController>();
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.up * spinSpeed * Time.deltaTime);

		// switch directions of bouncing
		if (transform.position.y >= (origin.y + bounceDistance)) {
			direction = ItemBlockDirection.down;
		} else if (transform.position.y <= (origin.y - bounceDistance)) {
			direction = ItemBlockDirection.up;
		}

		// move block up or down
		Vector3 temp = transform.position;
		switch (direction) {
		case ItemBlockDirection.down:
			temp.y -= bounceSpeed;
			break;
		case ItemBlockDirection.up:
			temp.y += bounceSpeed;
			break;
		}
		transform.position = temp;
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			controller.GivePowerupTypeToPlayer(type, other.gameObject);
			Destroy(this.gameObject);
		}
	}

	public void SetType (PowerupType _type) {
		type = _type;
	}
}
