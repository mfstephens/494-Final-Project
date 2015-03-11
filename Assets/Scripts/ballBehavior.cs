using UnityEngine;
using System.Collections;

public class ballBehavior : MonoBehaviour {

	public static ballBehavior ball;

	[SerializeField] AudioClip[] deflectionSounds;
	private AudioSource source;

	public GameObject owner = null;
	public float orbitSpeed;
	public bool possessed = false;
	public bool deflected = false;
	private float colorChange = .03f;
	public Color freeColor,teamA, teamB;
	private Color newColor, oldColor;
	public bool colliding = false;

	// Use this for initialization
	void Start () {
		ball = this;
		source = this.GetComponent<AudioSource> ();
		gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {

		if (!colliding && !possessed) {
			collider.isTrigger = false;
		}

		oldColor = this.gameObject.renderer.material.color;

		if (!possessed)
			newColor = freeColor;

		else {
			if(GameController.gameController.getTeamID()==1)
				newColor = teamB;
			else
				newColor = teamA;
		}

		this.gameObject.renderer.material.color = newColor;
		this.gameObject.GetComponent<TrailRenderer> ().material.color = newColor;
	}

	public bool isBallPossessed(){
		return possessed;
	}

	void OnCollisionEnter(Collision other) {
		colliding = true;
	}
	
	void OnCollisionExit(Collision other) {
		if (other.gameObject.CompareTag ("Platform") || other.gameObject.CompareTag("Base") || other.gameObject.CompareTag("Ceiling") || other.gameObject.CompareTag("LeftWall") || other.gameObject.CompareTag("RightWall")){
			if(!source.isPlaying)
				source.PlayOneShot(deflectionSounds[Random.Range (0,deflectionSounds.Length)],.5f);
		}
	
		colliding = false;
	}
	
	void OnTriggerEnter(Collider other) {
		colliding = true;
	}
	
	void OnTriggerExit(Collider other) {
		colliding = false;
	}
}
