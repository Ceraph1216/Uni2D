using UnityEngine;
using System.Collections;

public class GlobalMessageReceiver : MonoBehaviour 
{
	public static GlobalMessageReceiver instance;

	public TitleScreen titleScreen;
	public GameObject fadeOut;

	void Awake ()
	{
		instance = this;
	}

	public void OnIntroFinished ()
	{
		titleScreen.LoadGame ();
	}

	public void OnTutorialFinished ()
	{
		SoftPauseScript.softPaused = false;
		Time.timeScale = 1f;
	}

	public void OnAbilityFinished ()
	{
		InputManager.instance.ResetInputs ();
		SoftPauseScript.softPaused = false;
		Time.timeScale = 1f;
	}

	public void OnLevelComplete ()
	{
		ProgressManager.instance.AdvanceLevel ();
	}

	public void OnGameOverFinished ()
	{
		LivesManager.instance.Restart();
		ProgressManager.instance.Restart();
		SoftPauseScript.softPaused = false;
		Time.timeScale = 1f;
//		GameStateManager.SetGamePaused(false);
	}

	public void OnGameComplete ()
	{
		fadeOut.SetActive (true);
	}

	public void OnFadeFinished ()
	{
		// End game
	}

	private IEnumerator ShowMessageOnDelay(string p_message)
	{
		yield return new WaitForSeconds (.001f);

		PopupManager.instance.ShowMessage (p_message);
	}

	private void StartGame ()
	{
//		StartCoroutine(ShowMessageOnDelay("preGameQuestion"));
		PopupManager.instance.currentPopup = Enums.PopupType.None;
	}
}
