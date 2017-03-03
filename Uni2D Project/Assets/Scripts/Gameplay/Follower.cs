using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour 
{
	private CircleCollider2D _collider;
	private DistanceJoint2D _joint;

	void Awake ()
	{
		_collider = GetComponent<CircleCollider2D> ();
		_joint = GetComponent<DistanceJoint2D> ();
	}

	void OnEnable ()
	{
		_collider.enabled = true;
	}

	public void SetFollowTarget (Rigidbody2D p_rigidbody)
	{
		_joint.connectedBody = p_rigidbody;
	}

	public void Disable ()
	{
		_collider.enabled = false;
	}

	public void TempDisable ()
	{
		_collider.enabled = false;

		StartCoroutine (EnableAfterDelay ());
	}

	private IEnumerator EnableAfterDelay ()
	{
		yield return new WaitForSeconds (2f);

		_collider.enabled = true;
	}
}
