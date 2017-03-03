using UnityEngine;
using System.Collections;

public class DisableOnEvent : MonoBehaviour 
{
	public void Disable ()
	{
		gameObject.SetActive (false);
	}
}
