using UnityEngine;
using System.Collections;

public class ShakeForTimeBehavior : MonoBehaviour 
{
	public float delay;

	public bool moveX;
	public bool moveY;
	public bool moveZ;
	
	public float amountToMoveX;
	public float amountToMoveY;
	public float amountToMoveZ;
	
	public float timeToShake;
	public float speed;
	public bool isUI;
	public bool ignoreTimeScale;
	public bool ignoreSoftPause;
	
	public delegate void EventHandler(Transform t);
	public event EventHandler OnFinished;

	public bool alwaysResumeSamePosition;

	private ShakeBehavior shake;
	
	private Transform myTransform;
	private float initialX;
	private float initialY;
	private float initialZ;
	
	private float timeLeftToStop;

	private float delayTime;
	private bool hasEnabledShake;

	private bool hasAssignedInitialPosition;
	
	void Awake ()
	{
		shake = GetComponent<ShakeBehavior>();
		
		myTransform = transform;
	}
	
	// Use this for initialization
	void OnEnable () 
	{
		timeLeftToStop = timeToShake;
		
		shake.moveX = moveX;
		shake.moveY = moveY;
		shake.moveZ = moveZ;
		
		shake.amountToMoveX = amountToMoveX;
		shake.amountToMoveY = amountToMoveY;
		shake.amountToMoveZ = amountToMoveZ;

		if(!alwaysResumeSamePosition || !hasAssignedInitialPosition)
		{
			if(!isUI)
			{
				initialX = myTransform.position.x;
				initialY = myTransform.position.y;
				initialZ = myTransform.position.z;
			}
			else
			{
				initialX = myTransform.localPosition.x;
				initialY = myTransform.localPosition.y;
				initialZ = myTransform.localPosition.z;
			}

			hasAssignedInitialPosition = true;
		}

		shake.speed = speed;
		shake.isUI = isUI;
		shake.ignoreTimeScale = ignoreTimeScale;
		shake.ignoreSoftPause = ignoreSoftPause;

		delayTime = 0;
		hasEnabledShake = false;
		if(delay == 0)
		{
			shake.enabled = true;
			hasEnabledShake = true;
		}

		// Set up update event listeners
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);

		if(ignoreSoftPause)
		{
			SoftPauseScript.instance.AddToHandler(Enums.UpdateType.SoftPause, SoftUpdate);
		}
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);
		
		if(ignoreSoftPause)
		{
			SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.SoftPause, SoftUpdate);
		}

		if(OnFinished != null)
		{
			OnFinished(myTransform);
		}

		if(shake != null)
		{
			shake.enabled = false;
		}
	}
	
	void SoftUpdate (GameObject dispatcher)
	{
		if(!shake.enabled && !hasEnabledShake)
		{
			delayTime += TimeManager.deltaTime;
			if(delayTime >= delay)
			{
				shake.enabled = true;
				hasEnabledShake = true;
			}
		}

		if(shake.enabled)
		{
			timeLeftToStop -= TimeManager.deltaTime / (ignoreTimeScale ? Time.timeScale : 1);

			/*if(timeLeftToStop <= (timeToShake / 2.0F) && reductionsDone < 1)
			{
				shake.amountToMoveX /= 2.0F;
				shake.amountToMoveY /= 2.0F;
				shake.amountToMoveZ /= 2.0F;
				reductionsDone++;
			}
			else if(timeLeftToStop <= (timeToShake / 2.0F) && reductionsDone < 2)
			{
				shake.amountToMoveX /= 2.0F;
				shake.amountToMoveY /= 2.0F;
				shake.amountToMoveZ /= 2.0F;
				reductionsDone++;
			}
			else */if(timeLeftToStop <= 0)
			{
				shake.enabled = false;

				if(!isUI)
				{
					myTransform.position = new Vector3((moveX ? initialX : myTransform.position.x),
													   (moveY ? initialY : myTransform.position.y),
													   (moveZ ? initialZ : myTransform.position.z));
				}
				else
				{
					myTransform.localPosition = new Vector3((moveX ? initialX : myTransform.localPosition.x),
					                                        (moveY ? initialY : myTransform.localPosition.y),
					                                        (moveZ ? initialZ : myTransform.localPosition.z));
				}

				this.enabled = false;
			}
		}
	}
}
