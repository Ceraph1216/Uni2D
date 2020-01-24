using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
	private const float SWIPE_THRESHHOLD = 2f;
	private const float CHARGE_PRESS_RADIUS = 0.75f;
	private const float DOUBLE_TAP_TIME = 0.25f;

	public static InputManager instance;

	private Enums.GestureType _currentGesture;

	private Vector2 _pressStartPoint;
	private Vector2 _touchPoint;
	private float _currentDoubleTapTime;

	private float _currentXAxis;
	private bool _isJumpDown;
	private bool _isSuperJumpDown;
	private bool _isSlideDown;
	private bool _isGravityDown;

	private Transform _playerTransform;

	void Awake ()
	{
		instance = this;

		_playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
	}

	void OnEnable ()
	{
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable ()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	private void SoftUpdate (GameObject p_dispatcher)
	{
		CheckGesture ();
	}

	public void OnRightDown ()
	{
		_currentXAxis = 1;
	}

	public void OnLeftDown ()
	{
		_currentXAxis = -1;
	}

	public void OnDirectionUp ()
	{
		_currentXAxis = 0;
	}

	public void OnJumpDown ()
	{
		_isJumpDown = true;
	}

	public void OnJumpUp ()
	{
		_isJumpDown = false;
	}

	public void OnSuperJumpDown ()
	{
		_isSuperJumpDown = true;
	}

	public void OnSuperJumpUp ()
	{
		_isSuperJumpDown = false;
	}

	public void OnSlideDown ()
	{
		_isSlideDown = true;
	}

	public void OnSlideUp ()
	{
		_isSlideDown = false;
	}

	public void OnGravityDown ()
	{
		_isGravityDown = true;
	}

	public void OnGravityUp ()
	{
		_isGravityDown = false;
	}

	public float GetMovement ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			return _currentXAxis;
		}
	
		float l_horizontal = Input.GetAxis ("Horizontal");

		/*if (l_horizontal != 0) 
		{
			if (!AbilityManager.instance.HasAbility (AbilityManager.Ability.Vector)) 
			{
				return 1;
			}
		}*/

		return l_horizontal;
	}

	public bool GetJump ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			bool l_jumpDown = _isJumpDown;

			if (l_jumpDown) 
			{
				_isJumpDown = false;
			}
			return l_jumpDown;
		}

		return Input.GetAxis ("Jump") > 0;
	}

	public bool GetSuperJumpCharging ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			return _isSuperJumpDown;
		}

		return Input.GetAxis ("SuperJump") > 0;
	}

	public bool GetSuperJumpEnd ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			return !_isSuperJumpDown;
		}

		return Input.GetAxis ("SuperJump") <= 0;
	}

	public bool GetSlide ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			bool l_slideDown = _isSlideDown;

			if (l_slideDown) 
			{
				_isSlideDown = false;
			}
			return l_slideDown;
		}

		return Input.GetAxis ("Slide") > 0;
	}

	public bool GetGraityFlip ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			bool l_gravityDown = _isGravityDown;

			if (l_gravityDown) 
			{
				_isGravityDown = false;
			}
			return l_gravityDown;
		}

		return Input.GetAxis ("Gravity") > 0;
	}

	public void ResetInputs ()
	{
		_currentXAxis = 0;
		_isGravityDown = false;
		_isJumpDown = false;
		_isSuperJumpDown = false;
		_isSlideDown = false;
	}

	private void CheckGesture ()
	{
		if (!PlatformManager.instance.isMobile) 
		{
			return;
		}

		if (_currentDoubleTapTime > 0) 
		{
			_currentDoubleTapTime -= Time.deltaTime;
		}

		if (Input.touchCount <= 0) 
		{
			_currentGesture = Enums.GestureType.None;
			return;
		}

		Touch l_currentTouch = Input.GetTouch (0);

		_touchPoint = l_currentTouch.position;

		if (l_currentTouch.phase == TouchPhase.Ended) 
		{
			_currentGesture = Enums.GestureType.Release;
			_currentDoubleTapTime = DOUBLE_TAP_TIME;
			return;
		}

		if (l_currentTouch.phase == TouchPhase.Began) 
		{
			_currentGesture = Enums.GestureType.Press;
			_pressStartPoint = l_currentTouch.position;

			if (_currentDoubleTapTime > 0) 
			{
				_currentGesture = Enums.GestureType.DoubleTap;
			}
		}

		if (l_currentTouch.phase == TouchPhase.Stationary) 
		{
			_currentGesture = Enums.GestureType.Press;
			return;
		}

		if (l_currentTouch.phase == TouchPhase.Moved) 
		{
			Vector2 l_deltaPosition = l_currentTouch.deltaPosition;

			if (Mathf.Abs(l_deltaPosition.y) > SWIPE_THRESHHOLD) 
			{
				_currentGesture = Enums.GestureType.Swipe;
				return;
			}

			_currentGesture = Enums.GestureType.Press;
			return;
		}
	}
}
