using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour 
{
	public static float deltaTime
	{
		get
		{
			return Time.deltaTime;
		}
	}

	/// <summary>
	/// Uses the LocalTimeScale class to provide a local delta time for game objects in an additive fashion;
	/// for example, if an object has a LocalTimeScale factor of -0.5, the delta time returned for that object
	/// will be multiplied by (1 + -0.5), or 0.5 - making the delta time for that object half of what it would
	/// normally be. This class assumes that the LocalTimeScale component passed to it could be null, and simply
	/// returns the default delta time in that case
	/// </summary>
	/// <returns>The scale for object.</returns>
	/// <param name="local">Local.</param>
	public static float DeltaTimeForObject(LocalTimeScale local)
	{
		return deltaTime * (1 + ((local != null) ? local.factor : 0));
	}

	public float totalGameTime;
	public bool shouldUpdate;
	
	static TimeManager mInstance;
	
	// The instance of the TimeManager class
	static public TimeManager instance
	{
		get
		{
			return mInstance;
		}
	}

	// Use this for initialization
	void Awake () 
	{
		mInstance = this;
	}
	
	void OnEnable()
	{
		totalGameTime = 0;
		shouldUpdate = true;
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.InitialSoftUpdate, SoftUpdate);
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.InitialSoftUpdate, SoftUpdate);
	}
	
	void SoftUpdate (GameObject dispatcher)
	{
		if(shouldUpdate)
		{
			totalGameTime += deltaTime;
		}
	}
}