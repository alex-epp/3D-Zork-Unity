using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

using UnityEngine;

public class levelport : MonoBehaviour
{
	[SerializeField] string level;
	[SerializeField] Vector3 position;
	[SerializeField] Vector3 direction;

	void OnTriggerEnter(Collider other)
	{
		GameDataManager.gameData.spawnPosition = position;
		GameDataManager.gameData.spawnDirection = direction;
		GameDataManager.gameData.loadState = GameData.LoadState.Portal;
		GameDataManager.gameData.level = level;
		SceneManager.LoadScene (level);
	}
}
