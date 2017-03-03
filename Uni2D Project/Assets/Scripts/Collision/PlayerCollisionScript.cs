using UnityEngine;
using System.Collections;

public class PlayerCollisionScript : MonoBehaviour 
{
	private Rigidbody2D myRigidbody;
	
	void Awake() 
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	void OnCollisionEnter2D(Collision2D p_collision)
	{
		// Check collision stuff
	}

	void OnTriggerEnter2D(Collider2D p_other)
	{
		if (p_other.CompareTag ("Hazard")) 
		{
			Player.instance.Die ();
		}

		if (p_other.CompareTag ("Checkpoint")) 
		{
			CheckpointManager.instance.GetCheckpoint(p_other.GetComponent<Checkpoint>());
		}

		if (p_other.CompareTag ("Goal")) 
		{
			ProgressManager.instance.HitGoal();
		}

		if (p_other.CompareTag ("Advance")) 
		{
			Player.instance.HitAdvance();
		}
			
		if (p_other.CompareTag ("BadAnswer")) 
		{
			Player.instance.HitBadAnswer();
		}

		if (p_other.CompareTag ("Ability")) 
		{
			AbilityPickup l_pickup = p_other.GetComponent<AbilityPickup> ();
			AbilityManager.Ability l_ability = l_pickup.ability;
			string l_key = l_pickup.messageKey;
			Player.instance.PickupAbility(l_ability, l_key);
			p_other.gameObject.SetActive (false);
		}
	}
}
