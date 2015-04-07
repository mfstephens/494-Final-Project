using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : MonoBehaviour {

	public static MainCamera access;

	public float minCamX, minCamY, maxCamX;

	private Vector3 velocity = Vector3.zero;
	//public GameObject[] players;
	public List<GameObject> players;
	private Camera mainCamera;
		
	[SerializeField] 
	float boundingBoxPadding = 40f;
	
	[SerializeField]
	float minimumOrthographicSize = 60f;

	[SerializeField]
	float maximumOrthographicSize = 60f;
	
	[SerializeField]
	float zoomSpeed = 20f;
	
	void Start() {
		mainCamera = GetComponent<Camera> ();
		access = this;

		//ignore all collisions except with ball
		Physics.IgnoreLayerCollision (11, 10);
		Physics.IgnoreLayerCollision (11, 0);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		Rect boundingBox = CalculateTargetsBoundingBox();
		Vector3 temp = CalculateCameraPosition(boundingBox);
		float adjustX = temp.x;
		float adjustY = temp.y;

		mainCamera.orthographicSize = CalculateOrthographicSize (boundingBox);
		
		if (temp.y - mainCamera.orthographicSize <= minCamY) {
			adjustY = minCamY + mainCamera.orthographicSize;
		}

		if (temp.x - mainCamera.orthographicSize * mainCamera.aspect <= minCamX) {
			adjustX = minCamX + mainCamera.orthographicSize * mainCamera.aspect;
		}

		if (temp.x + mainCamera.orthographicSize * mainCamera.aspect >= maxCamX) {
			adjustX = maxCamX - mainCamera.orthographicSize * mainCamera.aspect;
		}


		Vector3 destination = new Vector3 (adjustX, adjustY, temp.z);

		transform.position = Vector3.Lerp (transform.position, destination, 0.05f);

		//boundingBox = CalculateTargetsBoundingBox();

		//transform.position = temp;
	}
	
	// MARK: Camera view
	Rect CalculateTargetsBoundingBox() {
		float minX = Mathf.Infinity;
		float maxX = Mathf.NegativeInfinity;
		float minY = Mathf.Infinity;
		float maxY = Mathf.NegativeInfinity;
		
		foreach (GameObject player in players) {
			Vector3 position = player.transform.position;
			
			minX = Mathf.Min(minX, position.x);
			minY = Mathf.Min(minY, position.y);
			maxX = Mathf.Max(maxX, position.x);
			maxY = Mathf.Max(maxY, position.y);
		}

		return Rect.MinMaxRect(minX - boundingBoxPadding, maxY + boundingBoxPadding, maxX + boundingBoxPadding, minY - boundingBoxPadding);
	}
	
	Vector3 CalculateCameraPosition(Rect boundingBox) {
		Vector2 boundingBoxCenter = boundingBox.center;
		
		return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, GetComponent<Camera>().transform.position.z);
	}
	
	float CalculateOrthographicSize(Rect boundingBox)
	{
		float orthographicSize = GetComponent<Camera>().orthographicSize;
		Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
		Vector3 topRightAsViewport = GetComponent<Camera>().WorldToViewportPoint(topRight);
		
		if (topRightAsViewport.x >= topRightAsViewport.y)
			orthographicSize = Mathf.Abs(boundingBox.width) / GetComponent<Camera>().aspect / 2f;
		else
			orthographicSize = Mathf.Abs(boundingBox.height) / 2f;
		
		
		return Mathf.Clamp(Mathf.Lerp(GetComponent<Camera>().orthographicSize, orthographicSize, Time.deltaTime * zoomSpeed), minimumOrthographicSize, maximumOrthographicSize);
	}
}