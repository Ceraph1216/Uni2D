using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
public class Basic2DGravity : MonoBehaviour 
{
	public float gravity;
	public float terminalVelocity;

	private Rigidbody2D myRigidbody;

	// Use this for initialization
	void Awake () 
	{
		myRigidbody = GetComponent<Rigidbody2D>();
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
	void SoftUpdate (GameObject dispatcher) 
	{
		// Get current velocity
		Vector3 newVelocity = myRigidbody.velocity;
		newVelocity.y += gravity;
		// Make sure we're not moving faster than terminal velocity
		if (newVelocity.y > terminalVelocity)
		{
			newVelocity.y = terminalVelocity;
		}
		// Apply new velocity
		myRigidbody.velocity = newVelocity;
		Debug.Log("velocity: " + myRigidbody.velocity.y);
	}
}
