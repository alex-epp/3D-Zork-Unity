using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageManager : MonoBehaviour {

	[SerializeField] Text feedbackText;
	[SerializeField] Text actionText;
	[SerializeField] RawImage actionImage;

	private float endTime = -1;

	public void Action(string txt, Texture icon)
	{
		actionText.text = txt;
		actionImage.texture = icon;

		actionText.enabled = txt.Length > 0;
		actionImage.enabled = icon != null;
	}

	public void Feedback(string txt, float timeout)
	{
		feedbackText.text = txt;
		endTime = timeout == -1 ? -1 : Time.time + timeout;
	}

	public void ClearAction()
	{
		Action ("", null);
	}

	public void ClearFeedback()
	{
		Feedback ("", -1);
	}

	void Update()
	{
		if (endTime != -1 && Time.time > endTime)
			ClearFeedback ();
	}
}
