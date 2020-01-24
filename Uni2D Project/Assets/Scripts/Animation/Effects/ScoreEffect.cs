using UnityEngine;
using System.Collections;

public class ScoreEffect : MonoBehaviour 
{
	public float floatSpeed;
	public float fadeDelay;
	public float fadeDuration;

	private SpriteRenderer _sprite;
	private Transform _transform;

	private float _currentDelay;
	private float _currentDuration;
	private Color _startColor;
	private Color _endColor;

	void Awake ()
	{
		_sprite = GetComponent<SpriteRenderer> ();
		_startColor = _sprite.color;
		_transform = transform;
	}

	void OnEnable ()
	{
		Initialize ();

		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable ()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	private void Initialize ()
	{
		_sprite.color = _startColor;
		_endColor = _sprite.color;
		_endColor.a = 0;

		_currentDelay = 0;
		_currentDuration = 0;
	}

	private void SoftUpdate (GameObject p_dispatcher)
	{
		Move ();
		if (_currentDelay < fadeDelay) 
		{
			_currentDelay += TimeManager.deltaTime;
			return;
		}

		if (_currentDuration < fadeDuration) 
		{
			_sprite.color = Color.Lerp (_startColor, _endColor, (_currentDuration / fadeDuration));

			_currentDuration += TimeManager.deltaTime;
			return;
		}

		End ();
	}

	private void Move ()
	{
		Vector3 l_newPosition = _transform.position;
		l_newPosition.y += floatSpeed * TimeManager.deltaTime;
		_transform.position = l_newPosition;
	}

	private void End ()
	{
		_sprite.color = _endColor;
		enabled = false;
	}
}
