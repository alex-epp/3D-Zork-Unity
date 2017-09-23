using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
	public void Start() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
	public void OnClickNew() {
		GameDataManager.Init ();
		GameDataManager.gameData.loadState = GameData.LoadState.New;
		GameDataManager.gameData.playing = true;
		GameDataManager.gameData.level = "level0";
		SceneManager.LoadScene("level0");
	}
	public void OnClickLoad()
	{
		//var path = EditorUtility.OpenFilePanel ("Open game file", Application.persistentDataPath, "que");
		var path = Application.persistentDataPath + "game" + ".que";
		FileStream stream;
		try {
			stream = new FileStream(path, FileMode.Open);
		} catch {
			Debug.Log ("Could not open " + path);
			return;
		}
		var serializer = new XmlSerializer(typeof(GameData));
		GameDataManager.gameData = serializer.Deserialize(stream) as GameData;
		stream.Close();
		GameDataManager.gameData.loadState = GameData.LoadState.File;
		GameDataManager.gameData.playing = true;
		SceneManager.LoadScene (GameDataManager.gameData.level);

	}
	public void OnClickSave()
	{
		//var path=EditorUtility.SaveFilePanel ("Save as...", Application.persistentDataPath, "game.que", "que");
		var path = Application.persistentDataPath + "game" + ".que";
		FileStream stream;
		try {
			stream = new FileStream (path, FileMode.Create);
		} catch {
			Debug.Log ("Could not open " + path);
			return;
		}
		Debug.Log ("Saving to: " + Application.persistentDataPath);
		var serializer = new XmlSerializer(typeof(GameData));
		serializer.Serialize(stream, GameDataManager.gameData);
		stream.Close();
	}
	public void OnClickQuit()
	{
		Application.Quit ();
	}
	public void OnClickContinue()
	{
		GameDataManager.gameData.loadState = GameData.LoadState.Continue;
		SceneManager.LoadScene (GameDataManager.gameData.level);
	}
	public void OnClickMainMenu()
	{
		SceneManager.LoadScene ("Main Menu");
	}
}
