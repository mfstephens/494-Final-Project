using UnityEngine;
using System.Collections;

public class PlayerFallOff : MonoBehaviour {

	private Rigidbody rigid;

	// Use this for initialization
	void Start () {
		rigid = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void fallOff(GameObject ball) {
		rigid.constraints = RigidbodyConstraints.None;

		GetComponent<PlayerController> ().bubbleShield.GetComponent<BubbleShield> ().endShield ();

		rigid.AddForce (0, 55000f, 0);
		rigid.AddExplosionForce (2000f, ball.transform.position, 10f);

		rigid.AddTorque(transform.up * 10000f);
		rigid.AddTorque(transform.forward * 10000f);
		rigid.AddTorque (transform.right * 10000f);
	}
}
