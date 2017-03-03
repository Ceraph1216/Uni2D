using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScreenManager : MonoBehaviour 
{
	static ScreenManager mInstance;

	// The instance of the ScreenManager class.
	static public ScreenManager instance
	{
		get
		{
			return mInstance;
		}
	}

	public string previousLevelName;
	public string currentLevelName;

	private string levelName;
	private bool transitionValue;
	private bool asyncValue;

	public bool isChangingScreens;

	public Enums.PopupType popupToStartNextScreenOn;
	public Enums.SecondaryPopupType secondaryPopupToStartNextScreenOn;

	void Awake()
	{
		mInstance = this;

		previousLevelName = "";
		currentLevelName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

		popupToStartNextScreenOn = Enums.PopupType.None;
		secondaryPopupToStartNextScreenOn = Enums.SecondaryPopupType.None;

		UnityEngine.SceneManagement.SceneManager.sceneLoaded += LevelLoaded;
	}

	void LevelLoaded(UnityEngine.SceneManagement.Scene p_scene, UnityEngine.SceneManagement.LoadSceneMode p_sceneMode)
	{
		if(transitionValue)
		{

		}

		if(PopupManager.instance != null)
		{
			PopupManager.instance.currentPopup = Enums.PopupType.None;
			PopupManager.instance.currentSecondaryPopup = Enums.SecondaryPopupType.None;
		}

		isChangingScreens = false;
	}

	public void ChangeScreen(string level, bool transition, bool shouldAsync = false)
	{
		levelName = level;
		transitionValue = transition;
		asyncValue = shouldAsync;

		isChangingScreens = true;

		SoftPauseScript.instance.ClearAllUpdates();
		SoftPauseScript.softPaused = false;
		Time.timeScale = 1;

		AnimationManager.pause();

		// Make sure achievement progress is reported to Game Center/Google Play
		/*if(currentLevelIndex == Constants.instance.GAME_SCREEN_INDEX)
		{
			AchievementsManager.ReportProgressToSocialGaming();
			SaveDataManager.instance.Save();
		}*/

		if(transitionValue)
		{

			TransitionChangeScreen();
		}
		else
		{
			ExecuteChange(levelName, asyncValue);
		}
	}

	private void TransitionChangeScreen()
	{
		ExecuteChange(levelName, asyncValue);
	}

	private void ExecuteChange(string level, bool shouldAsync)
	{
		// If you leave in the middle of the tutorial sequence, this will prevent a bug
		//TutorialSequenceBehavior.instance.enabled = false;

		PrefabManager.instance.ClearAllPools();
		
		previousLevelName = currentLevelName;
		currentLevelName = level;

		bool isUnityPro = Application.HasProLicense();
		if(shouldAsync && isUnityPro)
		{
//			transitionManager.levelLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level);
//			transitionManager.levelLoad.allowSceneActivation = false;
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(level);
		}
	}
}