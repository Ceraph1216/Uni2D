using UnityEngine;
using System.Collections;

public class FollowTargetScript : MonoBehaviour 
{
	public Enums.UpdateType[] _updateTypes = new Enums.UpdateType[] { Enums.UpdateType.LateSoftUpdate, Enums.UpdateType.LateSoftPause };

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
	
	void OnEnable()
	{
		Initialize();

		for(int i = 0; i < _updateTypes.Length; i++)
		{
			SoftPauseScript.instance.AddToHandler(_updateTypes[i], SoftUpdate);
		}
	}

	void OnDisable()
	{
		for(int i = 0; i < _updateTypes.Length; i++)
		{
			SoftPauseScript.instance.RemoveFromHandler(_updateTypes[i], SoftUpdate);
		}
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
		deltaVector = target.position - previousVector;
		if(!followX)
		{
			deltaVector.x = 0;	
		}

		if(!followY)
		{
			deltaVector.y = 0;	
		}

		if(!followZ)
		{
			deltaVector.z = 0;	
		}

		myTransform.Translate(deltaVector, Space.World);
		previousVector = target.position;
	}
}