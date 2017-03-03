using UnityEngine;
using System.Collections;

namespace Enums
{
	public enum UpdateType
	{
		InitialSoftUpdate,
		InitialSoftPause,
		EarlySoftUpdate,
		EarlySoftPause,
		SoftUpdate,
		SoftPause,
		FixedSoftUpdate,
		FixedSoftPause,
		LateSoftUpdate,
		LateSoftPause,
		FinalSoftUpdate,
		FinalSoftPause
	}

	public enum PopupType
	{
		None,
		Pause,
		Message
	}

	public enum SecondaryPopupType
	{
		None,
		Dialogue,
		Message
	}

	public enum SpawnableObjectTypes
	{
		Obstacle,
		Pickup,
		Other
	}

	public enum ControlType
	{
		KeyboardMouse,
		Controller
	}

	public enum PlayerGroundState
	{
		OnGround,
		Rising,
		Falling,
		Landing,
		Stuck
	}

	public enum Direction
	{
		Up,
		Right,
		Down,
		Left
	}

	public enum GestureType
	{
		None,
		Swipe,
		DoubleTap,
		Press,
		Release
	}

	public enum PlayerAttackState
	{
		None,
		J1,
		J2,
		JJFinish,
		JKFinish,
		K1,
		K2,
		KKFinish,
		KJFinish
	}

	public enum PlayerStunnedState
	{
		None,
		Hit
	}
}