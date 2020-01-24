using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkBehavior : MonoBehaviour 
{
	public bool isUI;
	public float timeToBlinkFor;
	public float timeBetweenBlinks;

	public bool startVisible;
	public bool endVisible;

	private SpriteRenderer sprite;
	private Image image;

	private float currentTime;
	private float currentBlinkTime;
	
	public delegate void BlinkEventHandler();
	public event BlinkEventHandler OnBlinkFinished;

	// Use this for initialization
	void Awake() 
	{
		if(isUI)
		{
			image = GetComponent<Image>();
		}
		else
		{
			sprite = GetComponent<SpriteRenderer>();
		}
	}
	
	void OnEnable()
	{
		currentTime = 0;

		if(isUI)
		{
			image.enabled = startVisible;
		}
		else
		{
			sprite.enabled = startVisible;
		}

		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);

		if(isUI)
		{
			if(image != null)
			{
				image.enabled = endVisible;
			}
		}
		else
		{
			if(sprite != null)
			{
				sprite.enabled = endVisible;
			}
		}
	}
	
	// Update is called once per frame
	void SoftUpdate(GameObject dispatcher) 
	{
		currentTime += TimeManager.deltaTime;

		if(currentTime >= timeToBlinkFor)
		{
			if(OnBlinkFinished != null)
			{
				OnBlinkFinished();
			}

			this.enabled = false;
		}
		else
		{
			currentBlinkTime += TimeManager.deltaTime;

			if(currentBlinkTime >= timeBetweenBlinks)
			{
				currentBlinkTime = 0;
				if(isUI)
				{
					image.enabled = !image.enabled;
				}
				else
				{
					sprite.enabled = !sprite.enabled;
				}
			}
		}
	}
}