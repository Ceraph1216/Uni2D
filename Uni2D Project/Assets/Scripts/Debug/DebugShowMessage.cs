using UnityEngine;
using System.Collections;

public class DebugShowMessage : MonoBehaviour 
{
	public string key;

	void OnEnable ()
	{
		PopupManager.instance.ShowMessage (key);
	}
}
