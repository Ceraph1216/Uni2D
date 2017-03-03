using UnityEngine;
using System.Collections;

public class SetToTargetPositionScript : MonoBehaviour 
{
	public Transform target;

	private Transform myTransform;

	// Use this for initialization
	void Awake() 
	{
		myTransform = transform;
	}

	void OnEnable()
	{
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	// Update is called once per frame
	void SoftUpdate(GameObject dispatcher) 
	{
		myTransform.position = target.position;
	}
}
