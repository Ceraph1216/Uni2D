using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButton : MonoBehaviour 
{
	public AbilityManager.Ability abilityType;

	private GameObject _contents;

	void Awake ()
	{
		_contents = transform.GetChild (0).gameObject;
	}

	void Update ()
	{
		if (!PlatformManager.instance.isMobile) 
		{
			gameObject.SetActive (false);
			return;
		}

		if (AbilityManager.instance.HasAbility (abilityType)) 
		{
			_contents.SetActive (true);
			enabled = false;
		}
	}
}
