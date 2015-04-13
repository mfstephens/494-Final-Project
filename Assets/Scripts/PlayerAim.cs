using UnityEngine;
using System.Collections;

public class PlayerAim : MonoBehaviour {
	
	public float fRadius = 20.0f;
	public GameObject player;
	
	public void UpdateGuidePosition (Vector3 pos) {
		float angle = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg;
		pos = Quaternion.AngleAxis (angle, Vector3.forward) * (Vector3.right * fRadius);
		transform.position = player.transform.position + pos;
		transform.LookAt (player.transform.position);
	}
}
