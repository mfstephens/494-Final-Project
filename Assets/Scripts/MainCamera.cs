using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : MonoBehaviour {

	public static MainCamera access;

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
	}
	
	// Update is called once per frame
	void Update () 
	{
		Rect boundingBox = CalculateTargetsBoundingBox();
		Vector3 temp = CalculateCameraPosition(boundingBox);
		float adjustX = temp.x;
		float adjustY = temp.y;

		if (temp.y - mainCamera.orthographicSize < -190f) {
			adjustY = -190f + mainCamera.orthographicSize;
		}

		if (temp.x - mainCamera.orthographicSize * mainCamera.aspect  < -740f) {
			adjustX = -740f + mainCamera.orthographicSize * mainCamera.aspect;
		}

		if (temp.x + mainCamera.orthographicSize * mainCamera.aspect  > 472f) {
			adjustX = 472f - mainCamera.orthographicSize * mainCamera.aspect;
		}

		transform.position = new Vector3 (adjustX, adjustY, temp.z);

		mainCamera.orthographicSize = CalculateOrthographicSize(boundingBox);

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