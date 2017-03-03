using UnityEngine;
using System.Collections;

public class SetPlayerStateScript : MonoBehaviour 
{
	private Transform myTransform;
	public tk2dSpriteAnimator myAnimator;

	void Awake()
	{
		myTransform = transform;
	}

	void OnEnable()
	{
		Initialize();
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	public void Initialize() 
	{
		// Set up event listeners for state changes
		PlayerStateManager.instance.ChangeGroundState += ChangeGroundState;
		PlayerStateManager.instance.ChangeAttackState += ChangeAttackState;
		PlayerStateManager.instance.ChangeStunnedState += ChangeStunnedState;
		PlayerStateManager.instance.ChangeMoving += ChangeMoving;

		myAnimator.AnimationCompleted = OnAnimationComplete;
	}

	void OnDisable()
	{
		myAnimator.AnimationCompleted = OnAnimationComplete;

		PlayerStateManager.instance.ChangeGroundState -= ChangeGroundState;
		PlayerStateManager.instance.ChangeAttackState -= ChangeAttackState;
		PlayerStateManager.instance.ChangeStunnedState -= ChangeStunnedState;
		PlayerStateManager.instance.ChangeMoving -= ChangeMoving;
	}

	void SoftUpdate(GameObject dispatcher)
	{
		// We will probably need to check some things here
	}

	private void ChangeGroundState(GameObject dispatcher)
	{
		switch (PlayerStateManager.instance.currentGroundState)
		{
			case Enums.PlayerGroundState.OnGround:
			{
				if (PlayerStateManager.instance.isMoving)
				{
					myAnimator.Play("run");
				}
				else if (PlayerStateManager.instance.currentAttackState == Enums.PlayerAttackState.None)
				{
					myAnimator.Play("idle");
				}
				break;
			}
			case Enums.PlayerGroundState.Rising:
			{
				myAnimator.Play("jump");
				break;
			}
			case Enums.PlayerGroundState.Falling:
			{
				myAnimator.Play("fall");
				break;
			}
			case Enums.PlayerGroundState.Landing:
			{
				if (PlayerStateManager.instance.currentAttackState == Enums.PlayerAttackState.None)
				{
					myAnimator.Play("land");
				}
				break;
			}
		}
	}

	private void ChangeAttackState(GameObject dispatcher)
	{
		/*if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Rising || 
		    PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Falling)
		{
			switch (PlayerStateManager.instance.currentAttackState)
			{
				// Do air attacks
			}
		}
		else
		{
			switch (PlayerStateManager.instance.currentAttackState)
			{
				// Do ground attacks
			}
		}*/
	}

	private void ChangeStunnedState(GameObject dispatcher)
	{
		switch (PlayerStateManager.instance.currentStunnedState)
		{
			case Enums.PlayerStunnedState.Hit:
			{
				myAnimator.Play("hit");
				break;
			}
		}
	}

	private void ChangeMoving(GameObject dispatcher)
	{
		if (Player.instance.isSliding) 
		{
			return;
		}

		if (PlayerStateManager.instance.isMoving)
		{
			if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.OnGround)
			{
				myAnimator.Play("run");
			}
		}
		else
		{
			if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.OnGround)
			{
				myAnimator.Play("idle");
			}
		}
	}

	private void OnAnimationComplete(tk2dSpriteAnimator anim, tk2dSpriteAnimationClip clip)
	{
		// Take care of animation end
	}
}