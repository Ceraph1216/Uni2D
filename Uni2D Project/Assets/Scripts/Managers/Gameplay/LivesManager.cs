using UnityEngine;
using System.Collections;

public class LivesManager : MonoBehaviour 
{
	public static LivesManager instance;

	private int _lives;
	public int lives
	{
		get 
		{
			return _lives;
		}
		private set 
		{
			_lives = value;
		}
	}

	public int startingLives = 2;

	/*private int _startingLives;
	public int startingLives
	{
		get 
		{
			return _startingLives;
		}
		set 
		{
			_startingLives = value;
		}
	}*/

	private bool _hasContinued;
	public bool hasContinued
	{
		get 
		{
			return _hasContinued;
		}
		set 
		{
			_hasContinued = value;
		}
	}

	public GameObject[] livesUI;

	void Awake ()
	{
		instance = this;
	}

	void OnEnable ()
	{
		Initialize ();
	}

	private void Initialize ()
	{
		lives = startingLives;
		SetUI ();
		hasContinued = false;
	}

	public void AddLife ()
	{
		lives++;
		SetUI ();
	}

	public void LoseLife ()
	{
		lives--;
		SetUI ();

		if (lives <= 0) 
		{
			Die ();
		}
	}

	private void SetUI()
	{
		for (int i = 0; i < livesUI.Length; i++) 
		{
			bool l_isActive = false;
			if (i < lives) 
			{
				l_isActive = true;
			}

			livesUI [i].SetActive (l_isActive);
		}
	}

	private void Die ()
	{
		lives = 3;
		Player.instance.Respawn();

		/*SoftPauseScript.softPaused = true;
		Time.timeScale = 0f;

		PopupManager.instance.ShowMessage ("gameOver");*/
	}

	public void Revive ()
	{
		AddLife ();
		Player.instance.Respawn ();
//		SoftPauseScript.softPaused = false;
//		hasContinued = true;
	}

	public void Restart ()
	{
		CheckpointManager.instance.ResetCheckpoints ();
		Player.instance.Respawn ();
		Initialize();
	}
}
