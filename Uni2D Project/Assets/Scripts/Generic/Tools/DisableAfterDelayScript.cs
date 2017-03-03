using UnityEngine;
using System.Collections;

public class DisableAfterDelayScript : MonoBehaviour 
{
	public float delay;
	private float currentDelay;

	void OnEnable()
	{
		Initialize();
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	void SoftUpdate (GameObject dispatcher) 
	{
		if (currentDelay > 0)
		{
			currentDelay -= Time.deltaTime;
		}
		else
		{
//			Debug.Log("disabling: " + gameObject.name);
			currentDelay = delay;
			gameObject.SetActive(false);
			enabled = false;
		}
	}

	public void Initialize()
	{
		currentDelay = delay;
	}
}
