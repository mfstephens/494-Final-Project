using UnityEngine;
using System.Collections;

public class CrowdBehavior : MonoBehaviour {

	public static CrowdBehavior access;
	public FlashesScript[] flashes;
	private AudioSource sounds;
	public AudioClip cheer;
	public AudioClip fightSong;

	// Use this for initialization
	void Start () {
		access = this;
		sounds = this.GetComponent<AudioSource> ();
		sounds.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void crowdFlash() {
		sounds.PlayOneShot (cheer);
		foreach (FlashesScript cur in flashes) {
			cur.flashAll();
		}
		Invoke ("stopCrowdFlash", 3f);
	}

	public void stopCrowdFlash() {
		foreach (FlashesScript cur in flashes) {
			cur.stopFlashing();
		}
	}
}
