using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
	
	[SerializeField] RawImage[] toolIconSlots;
	[SerializeField] RawImage[] treasureIconSlots;
	[SerializeField] RawImage selectedIconSlot;

	private int freeToolSlot = 0;
	private int freeTreasureSlot = 0;
	private List<int> treasureIndices = new List<int> ();
	private int currObjIndex;

	void Start()
	{
		// Initialization that happens on every level load
		freeToolSlot = freeTreasureSlot = 0;
		treasureIndices = new List<int> ();
		foreach (var t in GameDataManager.gameData.tools) {
			toolIconSlots [freeToolSlot].texture = GameDataManager.icons[t.icon];
			toolIconSlots [freeToolSlot].enabled = true;
			freeToolSlot++;
		}
		foreach (var t in GameDataManager.gameData.treasures) {
			treasureIndices.Add (freeTreasureSlot);
			treasureIconSlots [freeTreasureSlot].texture = GameDataManager.icons[t.icon];
			treasureIconSlots [freeTreasureSlot].enabled = true;
			freeTreasureSlot++;
		}
		if (GameDataManager.gameData.currObj != null) {
			currObjIndex = GameDataManager.gameData.tools.IndexOf (GameDataManager.gameData.currObj);
			selectedIconSlot.texture = GameDataManager.icons[GameDataManager.gameData.currObj.icon];
			selectedIconSlot.enabled = true;
		}
	}

	void Update()
	{
		var d = Input.GetAxis("Inventory Scroll");
		if (d > 0f)
			Scroll (1);
		else if (d < 0f)
			Scroll (-1);

		if (Input.GetButtonDown ("Inv+"))
			Scroll (1);
		if (Input.GetButtonDown ("Inv-"))
			Scroll (-1);
	}

	void Scroll(int d)
	{
		if (GameDataManager.gameData.tools.Count > 1) {
			if (d > 0)
				Select ((currObjIndex + d) % GameDataManager.gameData.tools.Count);
			if (d < 0)
				Select (currObjIndex == 0 ? GameDataManager.gameData.tools.Count - 1 : currObjIndex + d);
		}

	}

	public void RemoveObject(InteractiveObject o) {
		if (o.catagory == InteractiveObject.Catagory.Tool) {
			if (!GameDataManager.gameData.tools.Contains (o))
				Debug.Log ("Don't have: " + o.name);
			GameDataManager.gameData.tools.Remove (o);
			for (var i = 0; i < freeToolSlot-2; ++i)
				toolIconSlots [i].texture = GameDataManager.icons [GameDataManager.gameData.tools [i].icon];
			toolIconSlots [freeToolSlot-1].enabled = false;
			freeToolSlot--;
			Select (0);
		}
	}

	public void AddObject(InteractiveObject o)
	{
		if (o.catagory == InteractiveObject.Catagory.Tool) {
			GameDataManager.gameData.tools.Add (o);
			toolIconSlots [freeToolSlot].texture = GameDataManager.icons[o.icon];
			toolIconSlots [freeToolSlot].enabled = true;
			Select (freeToolSlot);
			freeToolSlot++;
		} else if (o.catagory == InteractiveObject.Catagory.Treasure) {
			GameDataManager.gameData.treasures.Add (o);
			treasureIndices.Add (freeTreasureSlot);
			treasureIconSlots [freeTreasureSlot].texture = GameDataManager.icons[o.icon];
			treasureIconSlots [freeTreasureSlot].enabled = true;
			freeTreasureSlot++;
		}
	}

	public List<InteractiveObject> Tools()
	{
		return GameDataManager.gameData.tools;
	}

	public List<InteractiveObject> Treasures()
	{
		return GameDataManager.gameData.treasures;
	}

	public InteractiveObject Current()
	{
		return GameDataManager.gameData.currObj;
	}

	public void Select(int o)
	{
		currObjIndex = o;
		GameDataManager.gameData.currObj = GameDataManager.gameData.tools[o];
		selectedIconSlot.texture = GameDataManager.icons[GameDataManager.gameData.currObj.icon];
		selectedIconSlot.enabled = true;
	}
}
