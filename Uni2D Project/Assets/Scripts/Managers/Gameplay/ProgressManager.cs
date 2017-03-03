using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressManager : MonoBehaviour 
{
	public static ProgressManager instance;

//	public Image progressBar;
	public int level;
	private bool _gameEnded;
	public bool gameEnded
	{
		get 
		{
			return _gameEnded;
		}
		private set 
		{
			_gameEnded = value;
		}
	}

	private bool _gameStarted;
	public bool gameStarted
	{
		get 
		{
			return _gameStarted;
		}
		private set 
		{
			_gameStarted = value;
		}
	}

	private float _progress;
	public float progress
	{
		get 
		{
			return _progress;
		}
		set 
		{
			_progress = value;

			/*if (_progress >= 100f)
			{
				_progress = 100f;
				Win ();
			}*/
//			progressBar.fillAmount = _progress / 100f;
		}
	}

	void Awake ()
	{
		instance = this;
//		progressBar.fillAmount = 0f;
	}

	public void HitGoal()
	{
		if (gameEnded) 
		{
			return;
		}

		Player.instance.HitGoal();
		Win ();
		gameEnded = true;
	}

	public void Win ()
	{
		string l_message = "levelComplete";

		if (level >= 8) 
		{
			l_message = "gameComplete";
		}
		PopupManager.instance.ShowMessage (l_message);
	}

	public void AdvanceLevel()
	{
		SoftPauseScript.softPaused = false;
		Time.timeScale = 1f;
		level++;
		UnityEngine.SceneManagement.SceneManager.LoadScene ("level" + level + "Screen");
		gameStarted = false;
		gameEnded = false;
	}

	public void Restart ()
	{
//		progress = 0;
		gameStarted = false;
	}

	public void StartGame ()
	{
		gameStarted = true;
		SoftPauseScript.softPaused = false;
		Time.timeScale = 1f;
	}
}
