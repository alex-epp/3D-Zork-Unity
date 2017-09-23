using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level15load : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!GameDataManager.gameData.used.Contains ("Sand"))
			foreach (var o in GameObject.FindGameObjectsWithTag("Chest"))
				if (o.transform.position.y > -100)
					o.transform.position = new Vector3(o.transform.position.x, -10000, o.transform.position.z);
	}
}
