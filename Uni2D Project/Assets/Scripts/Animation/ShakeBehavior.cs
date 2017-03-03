using UnityEngine;
using System.Collections;

public class ShakeBehavior : MonoBehaviour 
{
	private float _amountToMoveX;
	public float amountToMoveX
	{
		get
		{
			return _amountToMoveX;
		}
		
		set
		{
			_amountToMoveX = value;
			lastAmtX = _amountToMoveX;
		}
	}
	
	private float _amountToMoveY;
	public float amountToMoveY
	{
		get
		{
			return _amountToMoveY;
		}
		
		set
		{
			_amountToMoveY = value;
			lastAmtY = _amountToMoveY;
		}
	}
	
	private float _amountToMoveZ;
	public float amountToMoveZ
	{
		get
		{
			return _amountToMoveZ;
		}
		
		set
		{
			_amountToMoveZ = value;
			lastAmtZ = _amountToMoveZ;
		}
	}
	
	public bool moveX;
	public bool moveY;
	public bool moveZ;

	public float speed;
	public bool isUI;
	public bool ignoreTimeScale;
	public bool ignoreSoftPause;
	
	public bool alwaysResumeSamePosition;
	
	private float initialX;
	private float initialY;
	private float initialZ;
	
	private float lastAmtX;
	private float lastAmtY;
	private float lastAmtZ;
	
	private Transform myTransform;

	private bool hasAssignedInitialPosition;
	
	void Awake ()
	{
		myTransform = transform;
	}
	
	// Use this for initialization
	void OnEnable () 
	{
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
		
		lastAmtX = _amountToMoveX;
		lastAmtY = _amountToMoveY;
		lastAmtZ = _amountToMoveZ;
		
		// Set up update event listeners
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.LateSoftUpdate, SoftUpdate);

		if(ignoreSoftPause)
		{
			SoftPauseScript.instance.AddToHandler(Enums.UpdateType.LateSoftPause, SoftUpdate);
		}
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.LateSoftUpdate, SoftUpdate);

		if(ignoreSoftPause)
		{
			SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.LateSoftPause, SoftUpdate);
		}

		if(myTransform != null)
		{
			if(!isUI)
			{
				myTransform.position = new Vector3((moveX) ? initialX : myTransform.position.x, 
												   (moveY) ? initialY : myTransform.position.y, 
												   (moveZ) ? initialZ : myTransform.position.z);
			}
			else
			{
				myTransform.localPosition = new Vector3((moveX) ? initialX : myTransform.localPosition.x, 
				                                   		(moveY) ? initialY : myTransform.localPosition.y, 
				                                  		(moveZ) ? initialZ : myTransform.localPosition.z);
			}
		}
	}
	
	void SoftUpdate (GameObject dispatcher)
	{
		Vector3 position;
		if(!isUI)
		{
			position = transform.position;
		}
		else
		{
			position = transform.localPosition;
		}

		float amtX = 0;
		if(moveX)
		{
			if(position.x <= initialX - _amountToMoveX)
			{
				amtX = speed;
			}
			else if(position.x >= initialX + _amountToMoveX)
			{
				amtX = -speed;
			}
			else
			{
				amtX = lastAmtX;
			}
		}

		if(!ignoreTimeScale)
		{
			amtX *= Time.timeScale;
		}
		lastAmtX = amtX;
		
		float amtY = 0;
		if(moveY)
		{
			if(position.y <= initialY - _amountToMoveY)
			{
				amtY = speed;
			}
			else if(position.y >= initialY + _amountToMoveY)
			{
				amtY = -speed;
			}
			else
			{
				amtY = lastAmtY;
			}
		}

		if(!ignoreTimeScale)
		{
			amtY *= Time.timeScale;
		}
		lastAmtY = amtY;
		
		float amtZ = 0;
		if(moveZ)
		{
			if(position.z <= initialZ - _amountToMoveZ)
			{
				amtZ = speed;
			}
			else if(position.z >= initialZ + _amountToMoveZ)
			{
				amtZ = -speed;
			}
			else
			{
				amtZ = lastAmtZ;
			}
		}

		if(!ignoreTimeScale)
		{
			amtZ *= Time.timeScale;
		}
		lastAmtZ = amtZ;
		
		if(!isUI)
		{
			myTransform.Translate(amtX, amtY, amtZ);
		}
		else
		{
			myTransform.localPosition = new Vector3(myTransform.localPosition.x + amtX,
			                                        myTransform.localPosition.y + amtY,
			                                        myTransform.localPosition.z + amtZ);
		}
	}
}
