using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static ScoreManager instance;

	private int _score;
	public int score
	{
		get
		{
			return _score;
		}
		private set
		{
			_score = value;
			scoreDisplay.text = _score.ToString();
		}
	}

	public UILabel scoreDisplay;

	void Awake ()
	{
		instance = this;
	}

	public void AddToScore (int p_value)
	{
		score += p_value;

		if (score < 0)
		{
			score = 0;
		}
	}
}
