using UnityEngine;
using System.Collections;

public class PlayerAim : MonoBehaviour {
	
	public float fRadius = 20.0f;
	public Transform playerBody;
	
	// Reference to the LineRenderer we will use to display the simulated path
	public LineRenderer sightLine;
	
	// Reference to a Component that holds information about fire strength, location of cannon, etc.
	public PlayerController playerController;
	
	// Number of segments to calculate - more gives a smoother line
	public int segmentCount = 100;
	public float trajectoryOffest;
	
	// Length scale for each segment
	public float segmentScale = 1;
	
	// gameobject we're actually pointing at (may be useful for highlighting a target, etc.)
	private Collider _hitObject;
	public Collider hitObject { get { return _hitObject; } }
	
	void Start () {
		sightLine = gameObject.GetComponent<LineRenderer> ();
	}
	
	public void UpdateGuidePosition (Vector3 pos) {
		float angle = Mathf.Atan2 (pos.y, pos.x) * Mathf.Rad2Deg;
		pos = Quaternion.AngleAxis (angle, Vector3.forward) * (Vector3.right * fRadius);
		simulatePath (pos);
	}

	void simulatePath(Vector3 direction) {
		sightLine.enabled = true;
		Vector3[] segments = new Vector3[segmentCount];
		
		// The first line point is wherever the player's cannon, etc is
		segments[0] = transform.position;
		
		// The initial velocity
		Vector3 segVelocity = direction;
		
		// reset our hit object
		_hitObject = null;

		RaycastHit hit;
		if (Physics.Raycast(transform.position, segVelocity, out hit))
		{
			// remember who we hit
			_hitObject = hit.collider;
		}

		
		// At the end, apply our simulations to the LineRenderer
		
		// Set the colour of our path to the colour of the next ball
		//		Color startColor = playerFire.nextColor;
		//		Color endColor = startColor;
		//		startColor.a = 1;
		//		endColor.a = 0;
		//		sightLine.SetColors(startColor, endColor);


		Vector3 temp = this.transform.position;
		temp.z = 1;
		this.transform.position = temp;
		sightLine.SetPosition(0, transform.position);
		sightLine.SetPosition(1, hit.point);
	}
	
	public void RemoveGuide () {
		sightLine.enabled = false;
	}
}
