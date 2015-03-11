using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour {

	public float timeInterval = 5.5f;
	public float moveSpeed = 1f;
	float downPosition, upPosition, startTimer;
	bool down = false;
	// Use this for initialization
	void Start () {
		downPosition = -99f;
		upPosition = 0f;
		startTimer = Time.time;
	}
	

	void FixedUpdate () {
		if (startTimer + timeInterval < Time.time){
			if(down){
				Vector3 moveTemp = transform.position;
				moveTemp.y += moveSpeed;
				transform.position = moveTemp;
				if(moveTemp.y >= upPosition){
					down = false;
					startTimer = Time.time;
				}
			}
			else if(!down){
				Vector3 moveTemp = transform.position;
				moveTemp.y -= moveSpeed;
				transform.position = moveTemp;
				if(moveTemp.y <= downPosition){
					down = true;
					startTimer = Time.time;
				}
			}

		}
	}
}
