using UnityEngine;
using System.Collections;

public class SetFramerateScript : MonoBehaviour 
{
	public int targetFrameRate;

	// Use this for initialization
	void Awake () 
	{
		Application.targetFrameRate = targetFrameRate;
	}
}
