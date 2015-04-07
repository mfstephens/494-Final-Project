using UnityEngine;
using System.Collections;

public class BubbleShield : MonoBehaviour {

	public GameObject player;
	private PlayerController control;
	private PlayerMove move;
	public float rechargeSpeed;
	public Vector3 minimumSize;
	private Vector3 originalSize;
	private float percentDone;

	// Use this for initialization
	void Start () {
		Color assign = player.GetComponent<Renderer> ().material.color;
		this.GetComponent<Renderer> ().material.color = new Color (assign.a, assign.g, assign.b, 0.5f);

		originalSize = this.transform.localScale;
		control = player.GetComponent<PlayerController> ();
		move = player.GetComponent<PlayerMove> ();

		//ignore all collisions except with ball
		Physics.IgnoreLayerCollision (11, 10);
		//Physics.IgnoreLayerCollision (11, 9);
		Physics.IgnoreCollision (GetComponent<Collider> (), player.GetComponent<Collider> ());
		Physics.IgnoreLayerCollision (11, 8);
		Physics.IgnoreLayerCollision (11, 0);
		this.GetComponent<Renderer> ().enabled = false;
		this.GetComponent<Collider>().isTrigger = true;

	}
	
	// Update is called once per frame
	void Update () {
		this.transform.position = player.transform.position;

		if (control.isStunned) {
			return;
		}

		if (percentDone >= 1) {
			percentDone = 1;
			control.isStunned = true;
			endShield ();
			Invoke("unStun", 4f);
		}
		else if (percentDone <= 0) {
			percentDone = 0;
		}

		if (control.shieldOn) {
			percentDone += Time.deltaTime * rechargeSpeed;
		}
		else {
			percentDone -= Time.deltaTime * rechargeSpeed;
		}

		this.transform.localScale = Vector3.Lerp(originalSize, minimumSize, percentDone);
	}

	public void startShield() {
		if (move.canShield ()) {
			control.shieldOn = true;
			this.GetComponent<Renderer> ().enabled = true;
			this.GetComponent<Collider>().isTrigger = false;
		}
	}

	public void endShield() {
		control.shieldOn = false;
		this.GetComponent<Renderer> ().enabled = false;
		this.GetComponent<Collider>().isTrigger = true;

	}

	public void unStun() {
		control.isStunned = false;
		percentDone = 0;
		control.lockPosition = false;
	}

}
