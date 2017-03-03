using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour 
{
	private List<GameObject> _gridObjects;

	public float gridWidth;

	private Transform _transform;

	void Awake ()
	{
		_transform = transform;
		_gridObjects = new List<GameObject> ();
	}

	public void AddToGrid (GameObject p_object)
	{
		p_object.transform.parent = _transform;

		_gridObjects.Add (p_object);

		Reposition ();
	}

	public void RemoveFromGrid (GameObject p_object)
	{
		_gridObjects.Remove (p_object);
		p_object.transform.parent = null;
		Reposition ();
	}

	public void Reposition ()
	{
		for (int i = 0; i < _gridObjects.Count; i++) 
		{
			GameObject l_currentObject = _gridObjects [i];

			Vector3 l_newPosition = Vector3.zero;
			l_newPosition.x = i * gridWidth;

			l_currentObject.transform.localPosition = l_newPosition;
		}
	}
}
