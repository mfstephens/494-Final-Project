using UnityEngine;
using System.Collections;

public class HorizontalPlatformMove : MonoBehaviour {

	public float platformMin, platformMax, moveSpeed;
	
	// Update is called once per frame
	void Update () {
		if ((this.transform.position.x <= platformMin) || (this.transform.position.x >= platformMax)) {
			moveSpeed = moveSpeed * -1;
		}
		this.transform.position += new Vector3 (moveSpeed, 0, 0);
	}

}
