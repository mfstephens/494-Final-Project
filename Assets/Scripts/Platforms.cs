using UnityEngine;
using System.Collections;

public class Platforms : MonoBehaviour {

	Collider parentCollider;

	// Use this for initialization
	void Start () {
		//parentCollider = this.gameObject.transform.parent.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter(Collider other){
//		print ("enter" + other.gameObject.tag);
//	
//		if(other.gameObject.CompareTag("Player")){
//			print ("on trigger enter");
//			Physics.IgnoreCollision(parentCollider,other.gameObject.GetComponent<Collider>(),true);
//		}
	}

	void OnTriggerStay(Collider other) {
		//other.gameObject.GetComponent<PlayerMove> ().doubleJump = false;
	}

	void OnTriggerExit(Collider other){
//		if(other.gameObject.CompareTag("Player")){
//			Physics.IgnoreCollision(parentCollider,other.gameObject.GetComponent<Collider>(),false);
//		}
	}


}
