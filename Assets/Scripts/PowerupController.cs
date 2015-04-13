using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PowerupType {
	speedBoost, slowTime
}

public class PowerupController : MonoBehaviour {

	public float speedPowerupDuration = 5f;
	float speedPowerupStartTime;
	public ItemBlock itemBlockPrefab;
	public List<ItemBlock> activeItemBlocks;
	float timeSinceLastItem;

	// dictionary with the powerup type as a key and the player as the value
	Dictionary<PowerupType, GameObject> activePlayerPowerups;

	// Use this for initialization
	void Start () {
		ItemBlock ib = Instantiate (itemBlockPrefab);
		ib.SetType(GetRandomPowerupType());
		activeItemBlocks.Add(ib);
		activePlayerPowerups = new Dictionary<PowerupType, GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time >= (timeSinceLastItem + 20)) {
			ItemBlock ib = Instantiate (itemBlockPrefab);
			ib.SetType (GetRandomPowerupType ());
			activeItemBlocks.Add (ib);
			timeSinceLastItem = Time.time;
		}
		foreach (KeyValuePair<PowerupType, GameObject> entry in activePlayerPowerups) {
			switch (entry.Key) {
			case PowerupType.speedBoost:
				print ("current time: " + Time.time);
				print ("stop time: " + (speedPowerupStartTime + speedPowerupDuration));
				if (Time.time >= (speedPowerupStartTime + speedPowerupDuration)) {
					GameObject player = entry.Value;
					player.GetComponent<PlayerMove> ().speed -= 400f;
					player.GetComponent<PlayerMove> ().jumpSpeed -= 400f;
					player.GetComponent<PlayerMove> ().jumpShortSpeed -= 400f;
					activePlayerPowerups.Remove(entry.Key);
				}
				break;
			}
		}
	}

	public void GivePowerupTypeToPlayer (PowerupType type, GameObject player) {
		switch (type) {
		case PowerupType.speedBoost:
			speedPowerupStartTime = Time.time;
			timeSinceLastItem = Time.time;
			activePlayerPowerups.Add(type, player);
			player.GetComponent<PlayerMove> ().speed += 400f;
			player.GetComponent<PlayerMove> ().jumpSpeed += 400f;
			player.GetComponent<PlayerMove> ().jumpShortSpeed += 400f;
			break;
		}
	}

	PowerupType GetRandomPowerupType () {
		int r = Mathf.RoundToInt(Random.Range(0, 3.0f));
		return PowerupType.speedBoost;
		return (PowerupType)r;
	}
}
