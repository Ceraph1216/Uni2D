using UnityEngine;
using System.Collections;

public class StayAtScreenPoint : MonoBehaviour 
{
	Vector3 myScreenPoint;
	Transform myTransform;
	Camera camera;

	void Start () 
	{
		myTransform = transform;
		camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		myScreenPoint = camera.WorldToScreenPoint(myTransform.position);
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
		Vector3 correctWorldPoint = camera.ScreenToWorldPoint(myScreenPoint);

		myTransform.position = correctWorldPoint;
	}
}