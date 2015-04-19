using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using InControl;

public class ApplyColors : MonoBehaviour {
	MeshRenderer ballMesh;

	void Awake () {
		SkinnedMeshRenderer smr;
		for (int i = 0; i < InputManager.Devices.Count; i++) {
			//GameObject.Find ("P" + (i + 1) + "Score").GetComponent<Text>().color = PassInfoOnLoad.playerColor[i];
			string playerName = "Player" + (i + 1);
			ballMesh = GameObject.Find (playerName + "Ball").GetComponent<MeshRenderer>();
			ballMesh.material.color = PassInfoOnLoad.playerColor[i];
			ballMesh.material.color += new Color(.3f, .3f, .3f, .5f);
			GameObject player = GameObject.Find (playerName);
			smr = player.GetComponentInChildren<SkinnedMeshRenderer> ();
			int num = smr.materials.Length;
			for (int j = 0; j < num; j++) {
				if (smr.materials[j].name != "Glow" && smr.materials[j].name != "Black") {
					smr.materials[j].color = PassInfoOnLoad.playerColor[i];
					player.GetComponent<PlayerController>().playerColor = smr.materials[j].color;
					break;
				}
			}

		}
	}

}
