using UnityEngine;
using System.Collections;

public class Flames : MonoBehaviour {

	public float fireTime = 5f;

	private float TimeInstantiated;

	// Use this for initialization
	void Start () {
		print ("Instantiated");
		TimeInstantiated = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (TimeInstantiated + fireTime < Time.time) {
			print("Destroy");
			Destroy (this.gameObject);
		}
	}
}
