using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour 
{
	public static AbilityManager instance;

	public enum Ability
	{
		Vector,
		Friction,
		Elasticity,
		Gravity
	}

	public List<Ability> debugAbilities;
	private List<Ability> _activeAbilities;

	void Awake ()
	{
		#if UNITY_EDITOR 
		_activeAbilities = debugAbilities;
		#else
		_activeAbilities = new List<Ability> ();
		#endif
		instance = this;
	}

	public void AddAbility (Ability p_ability)
	{
		if (_activeAbilities.Contains (p_ability)) 
		{
			return;
		}

		_activeAbilities.Add (p_ability);
	}

	public bool HasAbility (Ability p_ability)
	{
		return _activeAbilities.Contains (p_ability);
	}
}
