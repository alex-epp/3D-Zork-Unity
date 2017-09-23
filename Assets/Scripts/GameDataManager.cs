using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataManager : MonoBehaviour {
	public static GameData gameData;
	private static bool first = true;
	private static GameDataManager instance;
	public static Dictionary<string, Texture> icons;
	public static Dictionary<string, InteractiveObject> objects;
	[SerializeField] InteractiveObjectInterface[] objectList;
	void Awake() {
		if (first) {
			instance = this;
			gameData = new GameData ();
			Init ();
		}
	}
	public static void Init() {
		gameData.tools = new List<InteractiveObject> ();
		gameData.treasures = new List<InteractiveObject> ();
		icons = new Dictionary<string, Texture> ();
		objects = new Dictionary<string, InteractiveObject> ();
		gameData.used = new List<string> ();
		gameData.destroyed = new List<string> ();
		foreach (var o in instance.objectList) {
			objects [o.name] = new InteractiveObject(o);
			icons [objects [o.name].icon] = o.icon;
		}
		first = false;
	}
}

[System.Serializable]
public class GameData
{
	public Vector3 spawnPosition;
	public Vector3 spawnDirection;
	public Vector3 position;
	public Vector3 direction;
	public string level;
	public List<InteractiveObject> tools;
	public List<InteractiveObject> treasures;
	public InteractiveObject currObj;
	public bool playing;
	public enum LoadState {File, Portal, New, Continue};
	public LoadState loadState;
	public List<string> destroyed;
	public List<string> used;
}
	
[System.Serializable]
public class InteractiveObject {
	public string name;
	public string icon;
	public string interactPrompt;

	public enum Catagory {Tool, Treasure};
	public Catagory catagory;

	public InteractiveObject() {}
	public InteractiveObject(InteractiveObjectInterface i) {
		this.name = i.name;
		this.icon = i.icon.name;
		this.interactPrompt = i.interactPrompt;
		this.catagory = i.catagory;
	}
	public void interact(InteractiveObject other, ApplicationModel a)
	{
		if (other == null) {
			if (name == "Sword" || name == "Shovel" || name == "Torch" || name == "Rope" || name == "Key") {
				a.msgManager.Feedback ("You got the " + name + "!", 3);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Coal") {
				a.msgManager.Feedback ("You got a chunk of coal!", 3);
				a.UseObject (this);
				a.Take (this);
			} else if (name == "Egg") {
				a.msgManager.Feedback ("You found an ornate jeweled egg!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Scarab") {
				a.msgManager.Feedback ("You found a scarab!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Pouch") {
				a.msgManager.Feedback ("You got a pouch full of coins!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Painting") {
				a.msgManager.Feedback ("You got a valuable painting!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Chest") {
				a.msgManager.Feedback ("You found a treasure chest!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Trident") {
				a.msgManager.Feedback ("You found a crystal trident!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Figurine") {
				a.msgManager.Feedback ("You found an intricately carved figurine!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Platinum") {
				a.msgManager.Feedback ("Wow! It's a solid bar of platinum!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Sceptor") {
				a.msgManager.Feedback ("You found a sceptor!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "Coffin") {
				a.msgManager.Feedback ("You found a gold coffin!", 5);
				a.DestroyObject (this);
				a.Take (this);
			} else if (name == "TrapDoor") {
				a.msgManager.Feedback ("It's locked! You will need a key...", 3);
			} else if (name == "Rug") {
				a.msgManager.Feedback ("It's too heavy...", 3);
			} else if (name == "Tree") {
				GameDataManager.gameData.spawnPosition = new Vector3 (-1.3f, -2.7f, 3.4f);
				GameDataManager.gameData.spawnDirection = new Vector3 (37.7f, 195.4f, 0f);
				GameDataManager.gameData.loadState = GameData.LoadState.Portal;
				GameDataManager.gameData.level = "level7";
				SceneManager.LoadScene ("level7");
			} else if (name == "Ice") {
				a.msgManager.Feedback ("This ice is brittle...", 3);
			} else if (name == "Sign") {
				a.msgManager.Feedback ("Welcome to the maze of twisty little passageways!", 3);
			} else if (name == "Bolt") {
				a.msgManager.Feedback ("You cannot turn the bolt with your bare hands!", 1);
			} else if (name == "Water") {
				a.msgManager.Feedback ("Actually, you can't swim.", 1);
			} else if (name == "Toolbox") {
				a.msgManager.Feedback ("You found a wrench in the toolbox!", 3);
				a.UseObject (this);
				a.Take (GameDataManager.objects ["Wrench"]);
			} else if (name == "Sand") {
				a.msgManager.Feedback ("You see a pile of soft sand here.", 1);
			} else if (name == "Machine") {
				a.msgManager.Feedback ("The machine has an input slot and a label: 'Pressure-O-Matic 6000'", 3);
			} else if (name == "Handle") {
				a.msgManager.Feedback ("It looks like you could tie something to this handle...", 3);
			} else if (name == "HangingRope") {
				GameDataManager.gameData.spawnPosition = new Vector3 (-2.2f, -20.4f, -0.2f);
				GameDataManager.gameData.spawnDirection = new Vector3 (287.5f, 83.8f, 0f);
				GameDataManager.gameData.loadState = GameData.LoadState.Portal;
				GameDataManager.gameData.level = "level21";
				SceneManager.LoadScene ("level21");
			} else if (name == "Trophy Case") {
				if (GameDataManager.gameData.treasures.Count < 11)
					a.msgManager.Feedback ("You need " + (11 - GameDataManager.gameData.treasures.Count) + " more treasures.", 1);
				else
					a.Win ();
			}
		} else if (other.name == "Torch" && name == "Rug") {
			a.msgManager.Feedback ("The rug burns away, revealing a passageway underneath!", 3);
			a.DestroyObject (this);
		} else if (other.name == "Sword" && name == "Ice") {
			a.msgManager.Feedback ("The ice breaks!", 3);
			a.DestroyObject (this);
		} else if (other.name == "Key" && name == "TrapDoor") {
			a.msgManager.Feedback ("You unlock the trapdoor.", 3);
			a.DestroyObject (this);
		} else if (other.name == "Wrench" && name == "Bolt") {
			a.msgManager.Feedback ("You turn the bolt!", 3);
			a.UseObject (this);
			a.DestroyObject ("Water");
		} else if (other.name == "Shovel" && name == "Sand") {
			a.msgManager.Feedback ("You dig up a chest!", 3);
			a.UseObject (this);
			a.RevealObject (GameDataManager.objects ["Chest"]);
		} else if (other.name == "Coal" && name == "Machine") {
			a.msgManager.Feedback ("You insert the coal and it comes out as diamond!", 5);
			a.Take (GameDataManager.objects ["Diamond"]);
			a.invManager.RemoveObject (other);
		} else if (other.name == "Rope" && name == "Handle") { 
			a.msgManager.Feedback ("You tie the rope to the handle. It looks like you can climb down now!", 3);
			a.invManager.RemoveObject (other);
			foreach (var o in GameObject.FindGameObjectsWithTag(name)) {
				o.GetComponent<MeshRenderer> ().enabled = true;
				o.tag = "HangingRope";
				a.SetViewing ("HangingRope", true);
			}
		} else if (other.name == "Sword") {
			a.msgManager.Feedback ("Violence is not the answer here.", 1);
		} else {
			a.msgManager.Feedback ("Using the " + other.name + " on the " + name + " seems to have no effect", 1);
		}
	}
}

[System.Serializable]
public class InteractiveObjectInterface {
	public string name;
	public Texture icon;
	public string interactPrompt;
	public InteractiveObject.Catagory catagory;
}