﻿using UnityEngine;
using System.Collections;

public class ballBehavior : MonoBehaviour {

	public GameObject owner = null;
	public float orbitSpeed;
	public bool possessed = false;
	private Vector3 originalScale;

	// Use this for initialization
	void Start () {
		originalScale = transform.lossyScale;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision other) {
		//owner.GetComponent<catchAndThrow> ().isPassing = false;
		//owner = null;
		//this.rigidbody.useGravity = true;
		//this.collider.isTrigger = false;
	}

	void onCollisionStay(Collision other){

	}
}
