using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dSprite))]
public class TweenTK2DColor : MonoBehaviour 
{
	public Color startColor;
	public Color endColor;

	public float duration;
	public string finishMessage;

	private float _currentTime;
	private tk2dSprite _sprite;

	void Awake ()
	{
		_sprite = GetComponent<tk2dSprite> ();
	}

	void OnEnable ()
	{
		_currentTime = 0;
		_sprite.color = startColor;

		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftPause, SoftUpdate);
	}

	void OnDisable ()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftPause, SoftUpdate);
	}

	private void SoftUpdate (GameObject p_dispatcher)
	{
		if (_currentTime >= duration) 
		{
			Complete ();
			return;
		}
		float l_completePercentage = _currentTime / duration;

		Color l_newColor = Color.Lerp (startColor, endColor, l_completePercentage);

		_sprite.color = l_newColor;

		_currentTime += TimeManager.deltaTime;
	}

	private void Complete ()
	{
		_sprite.color = endColor;
		GlobalMessageReceiver.instance.SendMessage (finishMessage);
		enabled = false;
	}
}
