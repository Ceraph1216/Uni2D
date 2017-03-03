using UnityEngine;
using System.Collections;

public class ChangeParentOnEnable : MonoBehaviour
{
	public Enums.UpdateType[] _updateTypes = new Enums.UpdateType[] { Enums.UpdateType.SoftUpdate, Enums.UpdateType.SoftPause };

	public string parentName;

	private Transform _myTransform;

	private Vector3 _savedPosition;

	private bool _changeParent;

	void Awake() 
	{
		_myTransform = transform;	
		_savedPosition = _myTransform.position;
	}

	void OnEnable()
	{
		_changeParent = false;

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

	void SoftUpdate (GameObject dispatcher)
	{
		if (_changeParent)
		{
			Transform l_parentTransform = GameObject.Find(parentName).transform;

			_myTransform.parent = l_parentTransform;
//			_myTransform.position = _savedPosition;
			enabled = false;
		}
		else
		{
			_changeParent = true;
		}
	}
}