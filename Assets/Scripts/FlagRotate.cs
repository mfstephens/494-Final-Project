using UnityEngine;
using System.Collections;

public class FlagRotate : MonoBehaviour {

	public float rotateSpeed;
	public Vector3[] positions;
	private float startTime = 0;

	void Update() {
//		if ((Time.time < 3) && (Time.time - startTime > 0.1f)) {
//			startTime = Time.time;
//			int random = Random.Range(0,2);
//			Vector3 newPos = positions[random];
//			positions[random] = this.transform.position;
//			this.transform.position = newPos;
//		}
		if (Time.time - startTime > 20f) {
			startTime = Time.time;
			int random = Random.Range(0,2);
			Vector3 newPos = positions[random];
			positions[random] = this.transform.position;
			this.transform.position = newPos;	
			this.gameObject.GetComponentInChildren<KingOfTheHill>().updateCurrentPlayer(-1);
		}
	}

	void FixedUpdate() {
		this.transform.Rotate (0, rotateSpeed, 0);
	}
}
