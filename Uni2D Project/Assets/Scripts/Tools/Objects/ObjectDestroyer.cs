using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectDestroyer : MonoBehaviour 
{
	private static ObjectDestroyer mInstance;

	private float timeToWaitBetweenDestroys = 0.5F;
	private float timeWaitedToDestroy;
	
	// The instance of the ObjectDestroyer class
	static public ObjectDestroyer instance
	{
		get
		{
			return mInstance;
		}
	}

	private List<GameObject> objectsToDestroy;

	// Use this for initialization
	void Awake() 
	{
		mInstance = this;
		this.enabled = false;
	}
	
	void OnEnable()
	{
		timeWaitedToDestroy = 0;

		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.SoftPause, SoftUpdate);
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.SoftPause, SoftUpdate);
	}
	
	// Update is called once per frame
	void SoftUpdate(GameObject dispatcher) 
	{
		timeWaitedToDestroy += TimeManager.deltaTime;
		if(timeWaitedToDestroy >= timeToWaitBetweenDestroys)
		{
			timeWaitedToDestroy = 0;

			if(objectsToDestroy.Count > 0)
			{
				GameObject obj = objectsToDestroy[objectsToDestroy.Count - 1];
				objectsToDestroy.Remove(obj);

				if(obj != null)
				{
					GameObject.Destroy(obj);
				}
			}

			if(objectsToDestroy.Count <= 0)
			{
				this.enabled = false;
			}
		}
	}

	public void AddObjectsToDestroy(List<GameObject> objects)
	{
		if(objectsToDestroy == null)
		{
			objectsToDestroy = new List<GameObject>();
		}

		for(int i = 0; i < objects.Count; i++)
		{
			objectsToDestroy.Add(objects[i]);
		}

		if(this != null && !this.enabled)
		{
			this.enabled = true;
		}
	}
}