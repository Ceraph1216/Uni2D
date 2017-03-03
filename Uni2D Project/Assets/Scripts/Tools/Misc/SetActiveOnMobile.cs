using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveOnMobile : MonoBehaviour 
{
	public bool active;
	public bool setOnMobile;

	void Awake ()
	{
		if (PlatformManager.instance.isMobile && setOnMobile) 
		{
			gameObject.SetActive (active);
		} 
		else if (!PlatformManager.instance.isMobile && !setOnMobile) 
		{
			gameObject.SetActive (active);
		}
	}
}
