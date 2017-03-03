using UnityEngine;
using System.Collections;

public class TailFollowObject : MonoBehaviour 
{
	public Transform followTarget;

	public float moveSpeed;
	public float minDistance;

	private Transform _transform;

	void Awake ()
	{
		_transform = transform;
	}

	void OnEnable ()
	{
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable ()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	private void SoftUpdate (GameObject p_dispatcher)
	{
		Vector2 l_newPosition = _transform.position;
		string l_debugOutput = "new position 1: " + l_newPosition;
		Vector2 l_followPosition = (Vector2)followTarget.position - (Vector2)followTarget.forward;
		/*if (PlatformManager.instance.isMobile)
		{
			l_followPosition = (Vector2)followTarget.position - (Vector2)followTarget.up;
			moveSpeed = 1f;
		}*/
		l_newPosition = Vector2.Lerp (_transform.position, l_followPosition, moveSpeed * TimeManager.deltaTime);
		l_debugOutput += "\nnew position 2: " + l_newPosition;
		l_debugOutput += "\ntarget position: " + l_followPosition;

		Vector2 l_heading = l_newPosition - l_followPosition;			// This vector points from this object to the object it's following
		float l_distance = l_heading.magnitude;
		l_debugOutput += "\nheading: " + l_heading + "\ndistance: " + l_distance;

		if (l_distance < minDistance) 
		{
			// make sure we never divide by 0
			if (l_distance == 0) 
			{
				l_distance = 0.0001f;
			}
			l_debugOutput += "\ndistance too close";
			Vector2 l_direction = l_heading / l_distance;						// This is the normalized direction from the player to the input point
			l_debugOutput += "\ndirection: " + l_direction;
			l_newPosition = (Vector2)followTarget.position + l_direction * minDistance;
			l_debugOutput += "\nnew position 3: " + l_newPosition;
		}

		_transform.position = l_newPosition;

		MobileDebugText.instance.SetDebugText (l_debugOutput);
	}
}
