using UnityEngine;
using System.Collections;

public class Platforms : MonoBehaviour {

	Collider parentCollider;

	// Use this for initialization
	void Start () {
		parentCollider = transform.parent.GetComponent<Collider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){

		if(other.gameObject.CompareTag("Player")){
			Physics.IgnoreCollision(parentCollider,other.gameObject.GetComponent<Collider>(),true);
		}
	}

	void OnTriggerStay(Collider other) {
		//other.gameObject.GetComponent<PlayerMove> ().doubleJump = false;
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			Physics.IgnoreCollision(parentCollider,other.gameObject.GetComponent<Collider>(),false);
		}
	}


}
