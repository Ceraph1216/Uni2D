using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingMessage : MonoBehaviour 
{
	public string messageKey;

	void OnEnable ()
	{
		SoftPauseScript.softPaused = true;
		Time.timeScale = 0f;
		PopupManager.instance.ShowMessage (messageKey);
	}
}
