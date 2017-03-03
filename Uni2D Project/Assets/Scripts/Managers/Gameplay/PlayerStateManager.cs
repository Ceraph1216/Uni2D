using UnityEngine;
using System.Collections;

public class PlayerStateManager : MonoBehaviour 
{
	// Set up delegate for state change events
	public delegate void EventHandler(GameObject e);
	public event EventHandler ChangeGroundState;
	public event EventHandler ChangeAttackState;
	public event EventHandler ChangeStunnedState;
	public event EventHandler ChangeMoving;
	
	static PlayerStateManager mInstance;
	
	// Whether there is an instance of the PlayerStateManager class present.
	
	static public bool isActive { get { return mInstance != null; } }
	
	// The instance of the PlayerStateManager class. Will create it if one isn't already around.
	
	static public PlayerStateManager instance
	{
		get
		{
			return mInstance;
		}
	}

	void Awake()
	{
		mInstance = this;
	}

	private float _currentStunTime;
	private float currentStunTime
	{
		get
		{
			return _currentStunTime;
		}
		set
		{
			if (value > _currentStunTime)
			{
				_currentStunTime = value;
			}
		}
	}

	private bool _isMoving;
	public bool isMoving 
	{
		get
		{
			return _isMoving;
		}
		set
		{
			_isMoving = value;
			if (ChangeMoving != null)
			{
				ChangeMoving(this.gameObject);
			}
		}
	}

	public bool isDashing {get; set;}

	public bool inControl {get; set;}

	private Enums.PlayerGroundState _currentGroundState = Enums.PlayerGroundState.OnGround;
	public Enums.PlayerGroundState currentGroundState
	{
		get
		{
			return _currentGroundState;
		}
		
		set
		{
			_currentGroundState = value;

			if (ChangeGroundState != null)
			{
				ChangeGroundState(this.gameObject);
			}
		}
	}

	private Enums.PlayerAttackState _currentAttackState = Enums.PlayerAttackState.None;
	public Enums.PlayerAttackState currentAttackState
	{
		get
		{
			return _currentAttackState;
		}
		
		set
		{
			_currentAttackState = value;
			if (value != Enums.PlayerAttackState.None) // TODO: Decide if we want people to be able to dash cancel an attack
			{
				inControl = false;
			}
			else
			{
				inControl = true;
			}

			if (ChangeAttackState != null)
			{
				ChangeAttackState(this.gameObject);
			}
		}
	}

	private Enums.PlayerStunnedState _currentStunnedState = Enums.PlayerStunnedState.None;
	public Enums.PlayerStunnedState currentStunnedState
	{
		get
		{
			return _currentStunnedState;
		}
		
		set
		{
			_currentStunnedState = value;
			if (value != Enums.PlayerStunnedState.None)
			{
				inControl = false;
			}
			else
			{
				inControl = true;
			}

			if (ChangeStunnedState != null)
			{
				ChangeStunnedState(this.gameObject);
			}
		}
	}
	
	public void ResetEvents()
	{
		ChangeGroundState = null;
		ChangeAttackState = null;
		ChangeStunnedState = null;
		ChangeMoving = null;
	}

	void OnEnable()
	{
		inControl = true;
		isMoving = false;
		isDashing = false;
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void SoftUpdate(GameObject dispatcher)
	{
		if (!inControl)
		{
			if (_currentStunTime > 0)
			{
				_currentStunTime -= Time.deltaTime;
			}
			else 
			{
				inControl = true;
			}
		}
	}
}