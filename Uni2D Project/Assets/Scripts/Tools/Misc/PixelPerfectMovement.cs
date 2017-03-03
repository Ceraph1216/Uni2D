using UnityEngine;
using System.Collections;

/// <summary>
/// This script ensures that any movement that happens is pixel perfect - we do this by rounding numbers to the nearest .01 decimal place.
/// Simply attach this script to anything that is supposed to move to give it pixel perfect movement
/// </summary>
public class PixelPerfectMovement : MonoBehaviour 
{
	/// <summary>
	/// A list of UpdateTypes to run this script's updates on - this is useful for being able to quickly change the order of the 
	/// script execution from the editor
	/// </summary>
	public Enums.UpdateType[] _updateTypes = new Enums.UpdateType[] { Enums.UpdateType.FinalSoftUpdate,
																	  Enums.UpdateType.FinalSoftPause };

	// Cache the transform of the object to avoid the hidden GetComponent call associated with the shortcut
	private Transform _transform;

	void Awake() 
	{
		_transform = transform;	
	}

	void OnEnable()
	{
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

	void SoftUpdate(GameObject e)
	{
		// We stop pixel-perfect movement when the game is over because slowing the game down doesn't work properly when it's on
//		if(HealthManager.instance.numLivesRemaining > 0)
//		{
			_transform.position = new Vector3(ExtensionMethods.Round(_transform.position.x, 2),
											  ExtensionMethods.Round(_transform.position.y, 2),
											  ExtensionMethods.Round(_transform.position.z, 2));
//		}
	}
}