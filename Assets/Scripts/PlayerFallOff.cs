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

		rigid.AddForce (0, 55000f, -10000f);
		rigid.AddExplosionForce (2000, ball.transform.position, 20);

		rigid.AddTorque(transform.up * 10000);
		rigid.AddTorque(transform.forward * 10000);
		rigid.AddTorque (transform.right * 10000);
	}
}
