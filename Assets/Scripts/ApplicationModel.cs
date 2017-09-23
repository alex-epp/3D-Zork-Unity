using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class ApplicationModel : MonoBehaviour
{
	[SerializeField] float playerReach;
	[SerializeField] GameObject player;
	[SerializeField] GameObject actionIconField;
	[SerializeField] Texture leftMouseIcon;
	[SerializeField] AudioClip collectSound;
	[SerializeField] SightCone sightCone;
	[SerializeField] Light torch;
	public MessageManager msgManager;
	public InventoryManager invManager;
	private InteractiveObject viewing;

	void Start () {
		switch (GameDataManager.gameData.loadState) {
		case GameData.LoadState.New:
			break;
		case GameData.LoadState.Portal:
			player.transform.position = GameDataManager.gameData.spawnPosition;
			player.transform.localRotation = Quaternion.Euler (0f, GameDataManager.gameData.spawnDirection.y, 0f);
			Camera.main.transform.localRotation = Quaternion.Euler (GameDataManager.gameData.spawnDirection.x, 0f, 0f);
			break;
		case GameData.LoadState.File:
		case GameData.LoadState.Continue:
			player.transform.position = GameDataManager.gameData.position;
			player.transform.localRotation = Quaternion.Euler (0f, GameDataManager.gameData.direction.y, 0f);
			Camera.main.transform.localRotation = Quaternion.Euler (GameDataManager.gameData.direction.x, 0f, 0f);
			break;
		}

		var u = new List<string> (GameDataManager.gameData.used);
		var d = new List<string> (GameDataManager.gameData.destroyed);
		foreach (var index in u)
			UseObject (GameDataManager.objects [index]);
		foreach (var index in d)
			DestroyObject (GameDataManager.objects [index]);
		GameDataManager.gameData.used = u;
		GameDataManager.gameData.destroyed = d;
	}
	void Update() {
		if (Input.GetButtonDown("Take") && viewing != null) {
			viewing.interact (null, this);
		}
		if (Input.GetButtonDown("Use") && invManager.Current() != null && viewing != null) {
			viewing.interact (invManager.Current(), this);
		}
		if (Input.GetButtonUp ("Cancel")) {
			GameDataManager.gameData.position = player.transform.position;
			GameDataManager.gameData.direction = Camera.main.transform.rotation.eulerAngles;
			SceneManager.LoadScene ("Game Menu");
		}
		if (Input.GetKeyDown (KeyCode.F1))
			Debug.Log (player.transform.position + ", " + Camera.main.transform.rotation.eulerAngles);
		if (Input.GetKeyDown (KeyCode.F2))
			Application.CaptureScreenshot (Application.persistentDataPath + "screenshot.png");
		if (GameDataManager.gameData.currObj != null)
			torch.enabled = GameDataManager.gameData.currObj.name == "Torch";
		else
			torch.enabled = false;
	}
	public void DestroyObject(string name) {
		GameObject[] sp = GameObject.FindGameObjectsWithTag (name);
		for (int i = 0; i < sp.Length; ++i)
			Destroy (sp [i]);

		sightCone.UnregisterObject (name);
		GameDataManager.gameData.destroyed.Add (name);
	}
	public void DestroyObject(InteractiveObject obj) {
		DestroyObject (obj.name);
	}
	public void RevealObject(InteractiveObject obj) {
		foreach (var o in GameObject.FindGameObjectsWithTag(obj.name)) {
			o.transform.position = new Vector3(o.transform.position.x, 0, o.transform.position.z);
		}
	}
	public void Take(InteractiveObject obj) {
		AudioSource.PlayClipAtPoint (collectSound, transform.position);
		invManager.AddObject (obj);
	}
	public void UseObject(InteractiveObject obj) {
		foreach (var o in GameObject.FindGameObjectsWithTag (obj.name)) {
			o.layer = LayerMask.NameToLayer ("Level");
			o.tag = "Untagged";
		}
		sightCone.UnregisterObject (obj.name);
		GameDataManager.gameData.used.Add (obj.name);
	}
	public void SetViewing(string s, bool b) {
		if (b) {
			msgManager.Action (GameDataManager.objects[s].interactPrompt, leftMouseIcon);
			viewing = GameDataManager.objects[s];
		} else {
			msgManager.ClearAction ();
			viewing = null;
		}
	}
	public void Win() {
		SceneManager.LoadScene ("win");
	}
}