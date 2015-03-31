using UnityEngine;
using System.Collections;

public class SingleHorizontalPlatformMove : MonoBehaviour {

	public float platformMin, platformMax, moveSpeed;
	
	private Rigidbody rigid;
	
	void Start() {
		rigid = this.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (this.transform.position.x < platformMin) {
			moveSpeed = moveSpeed * -1;
			this.transform.position = new Vector3(platformMin, this.transform.position.y, this.transform.position.z);
		}
		else if (this.transform.position.x > platformMax) {
			moveSpeed = moveSpeed * -1;
			this.transform.position = new Vector3(platformMax, this.transform.position.y, this.transform.position.z);
		}
		
		Vector3 newPos = this.transform.position + new Vector3 (moveSpeed, 0, 0);
		rigid.MovePosition (newPos);

	}
}
