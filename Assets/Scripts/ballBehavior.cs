using UnityEngine;
using System.Collections;

public class ballBehavior : MonoBehaviour {

	public GameObject owner = null;
	public float orbitSpeed;
	public bool possessed = false;
	public bool deflected = false;
	private float colorChange = .03f;
	public Color possesedColor, freeColor;
	private Color newColor, oldColor;
	public bool colliding = false;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		if (!colliding && !possessed) {
			collider.isTrigger = false;
		}

		oldColor = this.gameObject.renderer.material.color;

		if (!possessed) {
			if (oldColor.Equals(possesedColor)) {
				oldColor = freeColor;
			}

			if (oldColor.b >= 1) {
				colorChange = -.03f;
			}
			else if (oldColor.b <= 0) {
				colorChange = .03f;
			}
			newColor = new Color (oldColor.r, oldColor.g, oldColor.b + colorChange);
		}
		else {
			newColor = possesedColor;
		}

		this.gameObject.renderer.material.color = newColor;
		this.gameObject.GetComponent<TrailRenderer> ().material.color = newColor;
	}
	

	void OnCollisionEnter(Collision other) {
		colliding = true;
		print ("enter c");
	}
	
	void OnCollisionExit(Collision other) {
		colliding = false;
		print ("exit c");
	}
	
	void OnTriggerEnter(Collider other) {
		print ("enter t");
		colliding = true;
	}
	
	void OnTriggerExit(Collider other) {
		print ("exit t");
		colliding = false;
	}
}
