using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrailTargetAtDistance : MonoBehaviour 
{
	public Transform target;
	public int steps;

	private List<Vector3> _positions;

	private Transform _transform;

	void Awake ()
	{
		_transform = transform;
	}

	void OnEnable ()
	{
		_positions = new List<Vector3> ();

		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable ()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	private void SoftUpdate (GameObject p_dispatcher)
	{
		if (target == null) 
		{
			return;
		}
		UpdatePositions ();
	}

	private void UpdatePositions ()
	{
		if (_positions.Count < steps) 
		{
			_positions.Add (target.position);
			return;
		}

		_transform.position = _positions [0];
		_positions.Remove(_positions[0]);
		_positions.Add (target.position);
	}
}
