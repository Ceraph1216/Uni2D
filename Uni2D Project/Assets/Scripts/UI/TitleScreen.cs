using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour 
{
	public GameObject titleScreen;

	public void OnClickPlay ()
	{
		if (SoftPauseScript.softPaused) 
		{
			return;
		}

		titleScreen.SetActive (false);
		PopupManager.instance.ShowMessage ("intro");
	}

	public void LoadGame ()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Level1Screen");
	}
}
