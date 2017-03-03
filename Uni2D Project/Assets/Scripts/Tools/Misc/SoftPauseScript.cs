using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoftPauseScript : MonoBehaviour
{
	private static bool _softPaused = false;
	public static bool softPaused
	{
		get
		{
			return _softPaused;
		}
		
		set
		{
			bool oldVal = _softPaused;
			_softPaused = value;
			
			if(oldVal != _softPaused)
			{
				if(_softPaused)
				{
					AnimationManager.pause();
				}
				else
				{
					AnimationManager.resume();
				}
			}
		}
	}
	
	public delegate void EventHandler(GameObject e);
	private event EventHandler FixedSoftUpdate;
	private event EventHandler FixedSoftPause;

	private event EventHandler InitialSoftUpdate;
	private event EventHandler InitialSoftPause;

	private event EventHandler EarlySoftUpdate;
	private event EventHandler EarlySoftPause;

	private event EventHandler SoftUpdate;
	private event EventHandler SoftPause;

	private event EventHandler LateSoftUpdate;
	private event EventHandler LateSoftPause;

	private event EventHandler FinalSoftUpdate;
	private event EventHandler FinalSoftPause;

	/// <summary>
	/// Occurs at the end of an update cycle when not soft paused, and is not cleared out on scene changes
	/// </summary>
	public event EventHandler ContinuousFinalSoftUpdate;

	/// <summary>
	/// Occurs at the end of an update cycle when soft paused, and is not cleared out on scene changes
	/// </summary>
	public event EventHandler ContinuousFinalSoftPause;

	public event EventHandler EndOfFrameEvent;

	static SoftPauseScript mInstance;

	// Whether there is an instance of the SoftPauseScript class present.

	static public bool isActive { get { return mInstance != null; } }

	// The instance of the SoftPauseScript class. Will create it if one isn't already around.

	static public SoftPauseScript instance
	{
		get
		{
			return mInstance;
		}
	}

	GameObject myObject;

#if UNITY_EDITOR
	public List<string> currentUpdateFunctions;
#endif

	void Awake()
	{
		mInstance = this;
		myObject = this.gameObject;
		
#if UNITY_EDITOR
		currentUpdateFunctions = new List<string>();
#endif
	}

	void FixedUpdate ()
	{
		if(this.enabled)
		{
			if(!softPaused)
			{
				if(FixedSoftUpdate != null)
				{
					FixedSoftUpdate(myObject);
				}
			}
			else
			{
				if(FixedSoftPause != null)
				{
					FixedSoftPause(myObject);
				}
			}
		}
	}
	
	void Update () 
	{
		if(this.enabled)
		{
			if (!softPaused)
			{
				if (InitialSoftUpdate != null)
				{
					InitialSoftUpdate(myObject);
				}
				if (EarlySoftUpdate != null)
				{
					EarlySoftUpdate(myObject);
				}
				if (SoftUpdate != null)
				{
					SoftUpdate(myObject);
				}
			}
			else
			{
				if (InitialSoftPause != null)
				{
					InitialSoftPause(myObject);
				}
				if (EarlySoftPause != null)
				{
					EarlySoftPause(myObject);
				}
				if (SoftPause != null)
				{
					SoftPause(myObject);
				}
			}
		}
	}

	void LateUpdate ()
	{
		if(this.enabled)
		{
			if(!softPaused)
			{
				if(LateSoftUpdate != null)
				{
					LateSoftUpdate(myObject);
				}
				if(FinalSoftUpdate != null)
				{
					FinalSoftUpdate(myObject);
				}
				if(ContinuousFinalSoftUpdate != null)
				{
					ContinuousFinalSoftUpdate(myObject);
				}
			}
			else
			{
				if(LateSoftPause != null)
				{
					LateSoftPause(myObject);
				}
				if(FinalSoftPause != null)
				{
					FinalSoftPause(myObject);
				}
				if(ContinuousFinalSoftPause != null)
				{
					ContinuousFinalSoftPause(myObject);
				}
			}

			// End of frame event gets cleared out every frame - it is not meant to replace an update function
			if(EndOfFrameEvent != null)
			{
				EndOfFrameEvent(myObject);
			}

			EndOfFrameEvent = null;
		}
	}

	public void ClearAllUpdates()
	{
		FixedSoftUpdate = null;
		FixedSoftPause = null;

		InitialSoftUpdate = null;
		InitialSoftPause = null;

		EarlySoftUpdate = null;
		EarlySoftPause = null;
		
		SoftUpdate = null;
		SoftPause = null;
		
		LateSoftUpdate = null;
		LateSoftPause = null;
		
		FinalSoftUpdate = null;
		FinalSoftPause = null;
	}

	public bool IsClear()
	{
		return 
			FixedSoftUpdate == null &&
			FixedSoftPause == null &&

			InitialSoftUpdate == null &&
			InitialSoftPause == null &&

			EarlySoftUpdate == null &&
			EarlySoftPause == null &&
		
			SoftUpdate == null &&
			SoftPause == null &&
	
			LateSoftUpdate == null &&
			LateSoftPause == null &&
	
			FinalSoftUpdate == null &&
			FinalSoftPause == null;
	}

	public void AddToHandler(Enums.UpdateType s, EventHandler m)
	{
		switch(s)
		{
			case Enums.UpdateType.FixedSoftUpdate:
				FixedSoftUpdate += m;
				break;
			case Enums.UpdateType.FixedSoftPause:
				FixedSoftPause += m;
				break;
			case Enums.UpdateType.InitialSoftUpdate:
				InitialSoftUpdate += m;
				break;
			case Enums.UpdateType.InitialSoftPause:
				InitialSoftPause += m;
				break;
			case Enums.UpdateType.EarlySoftUpdate:
				EarlySoftUpdate += m;
				break;
			case Enums.UpdateType.EarlySoftPause:
				EarlySoftPause += m;
				break;
			case Enums.UpdateType.SoftUpdate:
				SoftUpdate += m;
				break;
			case Enums.UpdateType.SoftPause:
				SoftPause += m;
				break;
			case Enums.UpdateType.LateSoftUpdate:
				LateSoftUpdate += m;
				break;
			case Enums.UpdateType.LateSoftPause:
				LateSoftPause += m;
				break;
			case Enums.UpdateType.FinalSoftUpdate:
				FinalSoftUpdate += m;
				break;
			case Enums.UpdateType.FinalSoftPause:
				FinalSoftPause += m;
				break;
		}
#if UNITY_EDITOR
		currentUpdateFunctions.Add(m.Target.ToString());
#endif
	}

	public void RemoveFromHandler(Enums.UpdateType s, EventHandler m)
	{
		switch(s)
		{
			case Enums.UpdateType.FixedSoftUpdate:
				FixedSoftUpdate -= m;
				break;
			case Enums.UpdateType.FixedSoftPause:
				FixedSoftPause -= m;
				break;
			case Enums.UpdateType.InitialSoftUpdate:
				InitialSoftUpdate -= m;
				break;
			case Enums.UpdateType.InitialSoftPause:
				InitialSoftPause -= m;
				break;
			case Enums.UpdateType.EarlySoftUpdate:
				EarlySoftUpdate -= m;
				break;
			case Enums.UpdateType.EarlySoftPause:
				EarlySoftPause -= m;
				break;
			case Enums.UpdateType.SoftUpdate:
				SoftUpdate -= m;
				break;
			case Enums.UpdateType.SoftPause:
				SoftPause -= m;
				break;
			case Enums.UpdateType.LateSoftUpdate:
				LateSoftUpdate -= m;
				break;
			case Enums.UpdateType.LateSoftPause:
				LateSoftPause -= m;
				break;
			case Enums.UpdateType.FinalSoftUpdate:
				FinalSoftUpdate -= m;
				break;
			case Enums.UpdateType.FinalSoftPause:
				FinalSoftPause -= m;
				break;
		}

#if UNITY_EDITOR
		currentUpdateFunctions.Remove(m.Target.ToString());
#endif
	}
}