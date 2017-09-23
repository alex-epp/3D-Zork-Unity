using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTest : MonoBehaviour {

	void OnControllerColliderHit(ControllerColliderHit h) {
		if (h.gameObject.layer == LayerMask.NameToLayer ("Death")) {
			GameDataManager.gameData.position = GameDataManager.gameData.spawnPosition;
			GameDataManager.gameData.direction = GameDataManager.gameData.spawnDirection;
			SceneManager.LoadScene ("death");
		}
	}
}