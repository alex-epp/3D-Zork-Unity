using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightCone : MonoBehaviour {
	private Dictionary<string, int> viewing = new Dictionary<string, int>();
	[SerializeField] ApplicationModel applicationModel;
	void OnTriggerEnter(Collider other)
	{
		int i;
		if (!viewing.TryGetValue(other.tag, out i))
			viewing [other.tag] = 0;
		if (i == -1000)
			return;
		viewing [other.tag]++;
		if (viewing [other.tag] == 1)
			applicationModel.SetViewing (other.tag, true);
	}

	void OnTriggerExit(Collider other)
	{
		int i;
		if (!viewing.TryGetValue (other.tag, out i))
			return;
		if (i == -1000)
			return;
		viewing [other.tag]--;
		if (viewing [other.tag] <= 0) {
			applicationModel.SetViewing (other.tag, false);
			viewing [other.tag] = 0;
		}
	}

	public void UnregisterObject(string tag)
	{
		viewing [tag] = -1000;
		applicationModel.SetViewing (tag, false);
	}
}
