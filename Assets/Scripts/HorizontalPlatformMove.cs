using UnityEngine;
using System.Collections;

public class HorizontalPlatformMove : MonoBehaviour {

	public float platformMin, platformMax, moveSpeed;

	private Rigidbody rigid;

	void Start() {
		rigid = this.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if ((this.transform.position.x <= platformMin) || (this.transform.position.x >= platformMax)) {
			moveSpeed = moveSpeed * -1;
		}

		Vector3 newPos = this.transform.position + new Vector3 (moveSpeed, 0, 0);

		rigid.MovePosition (newPos);

//		foreach (Transform child in gameObject.transform) {
//			if (child.CompareTag("Player")) {
//				Vector3 newPlayerPos = child.transform.position + new Vector3 (moveSpeed, 0, 0);
//				child.GetComponent<Rigidbody>().MovePosition(newPlayerPos);
//			}
//		}
	}

	void OnCollisionEnter(Collision other) {
//		if (other.gameObject.CompareTag ("Player")) {
//			other.transform.parent = this.transform;
//		}
	}

	void OnCollisionExit(Collision other) {
//		other.transform.parent = null;
	}


}
