using UnityEngine;
using System.Collections;

public class FollowTargetAtPercentage : MonoBehaviour 
{
	public bool useFinalUpdate;

	public float relativeSpeed;
	public float roundingFactor = 1f;
	public Transform target;
	public bool followX;
	public bool followY;
	public bool followZ;

	private Vector3 previousVector;
	private Vector3 deltaVector;

	private Transform myTransform;

	// Use this for initialization
	void Awake()
	{
		myTransform = transform;
	}

	void Start ()
	{
		Initialize();
	}

	void OnEnable ()
	{
		Initialize();

		// Set up update event listeners
		if(!useFinalUpdate)
		{
			SoftPauseScript.instance.AddToHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);
			SoftPauseScript.instance.AddToHandler(Enums.UpdateType.EarlySoftPause, SoftUpdate);
		}
		else
		{
			SoftPauseScript.instance.AddToHandler(Enums.UpdateType.FinalSoftUpdate, SoftUpdate);
			SoftPauseScript.instance.AddToHandler(Enums.UpdateType.FinalSoftPause, SoftUpdate);
		}
	}

	void OnDisable()
	{
		// Make sure the update function is removed from the delegate when the script is not running
		if(!useFinalUpdate)
		{
			SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);
			SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.EarlySoftPause, SoftUpdate);
		}
		else
		{
			SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.FinalSoftUpdate, SoftUpdate);
			SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.FinalSoftPause, SoftUpdate);
		}

		// This is only here because much of the codebase was built without the final update being a factor;
		// this way we ensure that it's always reset to the "default" state
		useFinalUpdate = false;
	}

	public void Initialize() 
	{
		if(target != null)
		{
			previousVector = target.position;
		}
	}

	// Update is called once per frame
	void SoftUpdate (GameObject dispatcher)
	{

		if(followX || followY || followZ)
		{
			deltaVector = new Vector3(0,0,0);
			if(followX)
			{
				deltaVector.x = target.position.x - previousVector.x;	
			}
			if(followY)
			{
				deltaVector.y = target.position.y - previousVector.y;	
			}
			if(followZ)
			{
				deltaVector.z = target.position.z - previousVector.z;	
			}

			if(!(previousVector.x == 0 && previousVector.y == 0 && previousVector.z == 0))
			{
				//myTransform.Translate(deltaVector);
				Vector3 newPos = myTransform.position;
				newPos.x += Mathf.Round ((deltaVector.x * relativeSpeed) * roundingFactor) / roundingFactor;
				newPos.y += Mathf.Round ((deltaVector.y * relativeSpeed) * roundingFactor) / roundingFactor;
				newPos.z += Mathf.Round ((deltaVector.z * relativeSpeed) * roundingFactor) / roundingFactor;
				myTransform.position = newPos;
			}

			previousVector = target.position;
		}
	}
}