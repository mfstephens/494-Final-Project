using UnityEngine;
using System.Collections;

public class ballBehavior : MonoBehaviour {

	public GameObject owner;
	public float orbitSpeed;
	public bool possessed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (possessed) {
//			this.transform.LookAt (owner.transform.position);
//			this.transform.position += Vector3.Cross(this.transform.forward, Vector3.up).normalized * orbitSpeed;
//		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.gameObject.name.Equals (owner.gameObject.GetComponent<catchAndThrow> ().opponent1.gameObject.name) || other.gameObject.name.Equals (owner.gameObject.GetComponent<catchAndThrow> ().opponent2.gameObject.name)) {
			print ("ball dropped");
			owner.gameObject.GetComponent<catchAndThrow> ().ballDropped();
			this.rigidbody.velocity = Vector3.zero;
		}
	}
}
