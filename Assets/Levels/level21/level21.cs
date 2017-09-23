using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class level21 : MonoBehaviour {

	void Start () {
		// If the player hasn't picked up the rope yet, or it's in the inventory (e.g. it hasn't been used)
		if (!GameDataManager.gameData.destroyed.Contains ("Rope") || GameDataManager.gameData.tools.Where (o => o.name == "Rope").SingleOrDefault () != null) {
			foreach (var o in GameObject.FindGameObjectsWithTag ("Handle"))
				o.GetComponent<MeshRenderer> ().enabled = false;
		} else {
			foreach (var o in GameObject.FindGameObjectsWithTag("Handle"))
				o.tag = "HangingRope";
		}
	}
}
