using UnityEngine;
using System.Collections;

public class EndGamePlayerAnim : MonoBehaviour {
	Animator anim;
	// Use this for initialization
	bool isFirst = false;
	float switchPoseDelay = 7.5f;
	float switchPoseTime = 0f;
	bool spinToLeft = false;
	bool isDroppingBows = true;
	EllipsoidParticleEmitter[] epe;

	void Start () {
		anim = GetComponent<Animator> ();
		if (gameObject.name == "FirstPlace") {
			anim.SetTrigger("DropBows");
			isFirst = true;
			epe = GetComponentsInChildren<EllipsoidParticleEmitter> ();
			switchPoseTime = Time.time + switchPoseDelay;
			//transform.eulerAngles = new Vector3 (0f, 149f, 0f);
		} 
		if (gameObject.name == "FourthPlace") {
			anim.SetBool ("Cry", true);
		}
	
	}
	
	// Update is called once per frame
	//void Update () {
		//if (isFirst && /*spinTime < Time.time*/) {
			//spinTime = Time.time + spinDelay;
			//if(spinToLeft) transform.eulerAngles += new Vector3 (0f, -.5f, 0f);//StartCoroutine(spin (true));
			//else transform.eulerAngles += new Vector3 (0f, .5f, 0f); //StartCoroutine(spin (false));
			//if(spinToLeft && transform.eulerAngles.y <= 150f) spinToLeft = false;
			//if(!spinToLeft && transform.eulerAngles.y >= 210f) spinToLeft = true;
		//}
	
	//}

	void FixedUpdate(){
		if (isFirst && switchPoseTime < Time.time) {
			switchPoseTime = Time.time + switchPoseDelay;
			if(isDroppingBows){
				anim.SetTrigger("ToHead");
				isDroppingBows = false;
				StartCoroutine(spin());
			}
			else {
				anim.SetTrigger ("DropBows");
				for (int i = 0; i < epe.Length; i++) {
					epe[i].emit = false;
				}
				transform.eulerAngles = new Vector3 (0f, 180f, 0f);
				isDroppingBows = true;
			}

		}


	}

	IEnumerator  spin(){
		yield return new WaitForSeconds(0.8f);
		for (int i = 0; i < epe.Length; i++) {
			epe[i].emit = true;
		}
		while(!isDroppingBows) {
			transform.eulerAngles += new Vector3 (0f, 13f, 0f);
			yield return null;
		}

	}
	
}
