using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public float moveSpeed = 7f;
	public LayerMask ground;
	public float gravity = -20f;
	public float airAccel = 2f;

	float distToGround, buffer;
	bool grounded;
	float moveX, moveZ, extentsX, extentsZ;
	Vector3 moveVector;

	// Use this for initialization
	void Start () {
		Physics.gravity = new Vector3 (0f, gravity, 0f);
		extentsX = collider.bounds.extents.x;
		extentsZ = collider.bounds.extents.z;
		buffer = .08f;
		float xDist = collider.bounds.extents.x;
		float yDist = collider.bounds.extents.y;
		distToGround = collider.bounds.extents.y;


	}
	
	// Update is called once per frame
	void Update () {
		grounded = IsGrounded ();
		//print (rigidbody.velocity);
		moveX = Input.GetAxis ("Horizontal");
		moveZ = Input.GetAxis ("Vertical");
		moveVector = new Vector3 (moveX, 0, moveZ).normalized * moveSpeed;

	}

	void FixedUpdate(){
		MovementUpdate ();

	}
	void LateUpdate(){

	}



	void MovementUpdate(){
		//print (transform.InverseTransformDirection(rigidbody.velocity));
		print (grounded);
		if(grounded){
			rigidbody.velocity = transform.TransformDirection(moveVector);
		} else{
			//Vector3 relReal = transform.InverseTransformDirection(rigidbody.velocity);
			bool update = false;
			//float addToZ = Mathf.Abs(rigidbody.velocity.x);
			//float addToX = Mathf.Abs(rigidbody.velocity.z);

			moveVector = transform.InverseTransformDirection(rigidbody.velocity);
			if(moveX > 0f && moveVector.x < 0f){ moveVector.x = 0f; update = true;}
			else if(moveX < 0f && moveVector.x > 0f){ moveVector.x = 0f; update = true;}
			if(moveZ > 0f && moveVector.z < 0f){ moveVector.z = 0f; update = true;}
			else if(moveZ < 0f && moveVector.z > 0f) {moveVector.z = 0f; update = true;}
			if(update) rigidbody.velocity = transform.TransformDirection(moveVector);
		}
		
	}

	bool IsGrounded(){
		Vector3 front, back, left, right;
		front = back = left = right = transform.position;
		front.x += extentsX + buffer;
		back.x -= extentsX + buffer;
		left.z -= extentsZ + buffer;
		right.z += extentsZ + buffer;


		return ((Physics.Raycast (front, Vector3.down, distToGround + buffer, ground)) 
		        || (Physics.Raycast (back, Vector3.down, distToGround + buffer, ground)) 
		        || (Physics.Raycast (left, Vector3.down, distToGround + buffer, ground))
		        || (Physics.Raycast (right, Vector3.down, distToGround + buffer, ground)));
	}
}
