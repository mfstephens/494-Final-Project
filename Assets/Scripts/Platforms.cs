using UnityEngine;
using System.Collections;

public class Platforms : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			Collider parentCollider = this.gameObject.transform.parent.collider;
			Physics.IgnoreCollision(parentCollider,other.gameObject.collider,true);
		}

	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.CompareTag("Player")){
			Collider parentCollider = this.gameObject.transform.parent.collider;
			Physics.IgnoreCollision(parentCollider,other.gameObject.collider,false);
		}
	}
}
