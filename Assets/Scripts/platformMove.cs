using UnityEngine;
using System.Collections;

public class platformMove : MonoBehaviour {

	public float platformMin, platformMax, moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ((this.transform.position.y <= platformMin) || (this.transform.position.y >= platformMax)) {
			moveSpeed = moveSpeed * -1;
		}

		this.transform.position += new Vector3 (0, moveSpeed, 0);
	}
}
