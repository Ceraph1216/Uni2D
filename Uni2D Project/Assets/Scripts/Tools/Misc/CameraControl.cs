using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour 
{
	public Transform target;
	public Vector3 offset;
	public float speed;
	public float maxDistance;

	private Transform _transform;

	void Awake ()
	{
		_transform = transform;
	}

	void OnEnable ()
	{
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.LateSoftUpdate, SoftUpdate);
	}

	void OnDisable ()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.LateSoftUpdate, SoftUpdate);
	}

	private void SoftUpdate (GameObject p_dispatcher)
	{
		Vector3 l_targetVector = target.position + offset;

		// Use an ease to calculate the position on the Lerp

		float l_time = Mathfx.Sinerp (0f, 1f, Time.deltaTime);
//		float time = Mathfx.Hermite(0.0, 1.0, Time.time);

		_transform.position = Vector3.Lerp(_transform.position, l_targetVector, l_time * speed);

		// Make sure the camera doesn't lag too far behind the target
		/*float l_distance = Mathf.Abs(Vector3.Distance(_transform.position, l_targetVector));

		if (l_distance > maxDistance)
		{
			float l_overPercent = (l_distance / maxDistance) / 100;

			_transform.position = Vector3.Lerp(_transform.position, l_targetVector, l_overPercent);
		}*/
	}
}
