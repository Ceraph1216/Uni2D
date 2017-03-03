using UnityEngine;
using System.Collections;

public class BasicPlayerMovementScript : MonoBehaviour 
{
	public tk2dSprite mySprite;
	public LayerMask groundCheckMask;
	
	public Transform leftSensorTransform;
	public Transform rightSensorTransform;
	public Transform groundSensorTransform;

	public Transform hitboxes;

	public GameObject attackHitbox;

	public BoxCollider2D normalHitBox;
	public BoxCollider2D slideHitBox;

	private bool isHitR;
	private bool isHitL;
	private bool isHitG;
	private bool isHitForward;
	
	private Vector2 hitPointR = Vector2.zero;
	private Vector2 hitPointL = Vector2.zero;
	private Vector2 hitPointG = Vector2.zero;
	private Vector2 hitPointForward = Vector2.zero;

	private float _currentSlideCooldown;
	private float _currentSlideDuration;
	private float _currentLaunchDuration;
	private float _currentChargeTime;
	private float _gravityControlCooldown;
	private float _forwardHitX;

	private float _jumpMod
	{
		get 
		{
			if (PlatformManager.instance.isMobile) 
			{
				return 1.05f;
			} 

			return 1f;
		}
	}

	private bool _isSliding;
	public bool isSliding
	{
		get 
		{
			return _isSliding;
		}
		private set 
		{
			_isSliding = value;
		}
	}

	private bool _isLaunching
	{
		get
		{
			return _currentLaunchDuration > 0;
		}
	}
	private bool _isCharging;

	private Rigidbody2D myRigidbody;
	private Transform myTransform;
	private BoxCollider2D _hitbox;

	private Transform _follow;

	void Awake ()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
		myTransform = transform;
		_hitbox = GetComponent<BoxCollider2D> ();
		_follow = GameObject.FindGameObjectWithTag("cameraTarget").transform;
	}

	void OnEnable () 
	{
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable () 
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	// Update is called once per frame
	void SoftUpdate (GameObject dispatcher) 
	{
		DrawRays();

		if (AbilityManager.instance.HasAbility (AbilityManager.Ability.Friction)) 
		{
			UpdateSlide();
		}

		if (AbilityManager.instance.HasAbility (AbilityManager.Ability.Elasticity)) 
		{
			UpdateSuperJump();
		}

		UpdateMovement();

		if (AbilityManager.instance.HasAbility (AbilityManager.Ability.Gravity)) 
		{
			UpdateGravityControls();
		}

		SetStates();
	}

	void DrawRays()
	{
		//---------------------------------------------------------------------
		// Use raycasting to find where the ground is in relation to the player
		// and what angle the ground they are on is.
		//---------------------------------------------------------------------
		
		// Set the direction and distance of the ground check
		Vector2 downDirection = new Vector2(0, (myTransform.localScale.y * -1f));// -myTransform.up;
		Vector2 forwardDirection = new Vector2(mySprite.scale.x, 0);
		float distanceSide = 0.25f;
		float distanceGround = 0.25f;
		float distanceFront = (normalHitBox.size.x / 2f) + 0.05f;

		// Get the start positions for the two forward checks
		Vector3 l_forwardStartT = myTransform.position;
		l_forwardStartT.y += (normalHitBox.size.y - 0.1f) * myTransform.localScale.y;
		Vector3 l_forwardStartB = myTransform.position;
		l_forwardStartB.y += 0.1f * myTransform.localScale.y;

		// Create debug visualizations of the rays being used
		Debug.DrawRay (rightSensorTransform.position, downDirection * distanceSide, Color.green);
		Debug.DrawRay (leftSensorTransform.position, downDirection * distanceSide, Color.green);
		Debug.DrawRay (groundSensorTransform.position, downDirection * distanceGround, Color.red);
		Debug.DrawRay (l_forwardStartT, forwardDirection * distanceFront, Color.blue);
		Debug.DrawRay (l_forwardStartB, forwardDirection * distanceFront, Color.blue);
		
		//		Debug.DrawRay (topSensorTransform.position, forwardDirection * distanceFront, Color.yellow);
		//		Debug.DrawRay (bottomSensorTransform.position, forwardDirection * distanceFront, Color.yellow);
		
		// Create a raycast for each of the 3 sensor points
		RaycastHit2D hitR = Physics2D.Raycast(rightSensorTransform.position, downDirection, distanceSide, groundCheckMask);
		RaycastHit2D hitL = Physics2D.Raycast(leftSensorTransform.position, downDirection, distanceSide, groundCheckMask);
		RaycastHit2D hitG = Physics2D.Raycast(groundSensorTransform.position, downDirection, distanceGround, groundCheckMask);
		RaycastHit2D hitFT = Physics2D.Raycast(l_forwardStartT, forwardDirection, distanceFront, groundCheckMask);
		RaycastHit2D hitFB = Physics2D.Raycast(l_forwardStartB, forwardDirection, distanceFront, groundCheckMask);
		//		RaycastHit2D hitT = Physics2D.Raycast(topSensorTransform.position, forwardDirection, distanceFront, groundCheckMask);
		//		RaycastHit2D hitB = Physics2D.Raycast(bottomSensorTransform.position, forwardDirection, distanceFront, groundCheckMask);
		
		isHitR = false;
		isHitL = false;
		isHitG = false;
		isHitForward = false;
		
		hitPointR = Vector2.zero;
		hitPointL = Vector2.zero;
		hitPointG = Vector2.zero;
		hitPointForward = Vector2.zero;
		
		// The right sensor hit something
		if (hitR != null && hitR.collider != null)
		{
			isHitR = true;
			hitPointR = hitR.point;
		}
		
		// The left sensor hit something
		if (hitL != null && hitL.collider != null)
		{
			isHitL = true;
			hitPointL = hitL.point;
		}
		
		// The middle sensor hit something
		if (hitG != null && hitG.collider != null)
		{
			isHitG = true;
			hitPointG = hitG.point;
		}

		// The front top sensor hit something
		if (hitFT != null && hitFT.collider != null)
		{
			isHitForward = true;
			hitPointForward = hitFT.point;
		}

		// The front bottom sensor hit something
		if (hitFB != null && hitFB.collider != null)
		{
			isHitForward = true;
			hitPointForward = hitFB.point;
		}
	}

	private void UpdateSlide ()
	{
		if (_currentSlideCooldown > 0)
		{
			_currentSlideCooldown -= Time.deltaTime;
			return;
		}

		if (_currentSlideDuration > 0)
		{
			_currentSlideDuration -= Time.deltaTime;

			Vector3 l_newVelocity = myRigidbody.velocity;
			l_newVelocity.x =  mySprite.scale.x * Constants.SLIDE_FORCE;
			myRigidbody.velocity = l_newVelocity;

			return;
		}

		if (isSliding)
		{
			isSliding = false;
			_currentSlideCooldown = Constants.SLIDE_COOLDOWN;
			normalHitBox.enabled = true;
			slideHitBox.enabled = false;
			return;
		}

		if (InputManager.instance.GetSlide())
		{
			if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.OnGround || _isCharging)
			{
				return;
			}

			isSliding = true;
			_currentSlideDuration = Constants.SLIDE_DURATION;
			normalHitBox.enabled = false;
			slideHitBox.enabled = true;
			Player.instance.animator.Play ("slide");
		}
	}

	private void UpdateSuperJump ()
	{
		if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.OnGround) 
		{
			return;
		}

		if (InputManager.instance.GetSuperJumpCharging()) 
		{
			if (!_isCharging) 
			{
				_isCharging = true;
				_currentChargeTime = Constants.SUPER_JUMP_CHARGE_TIME;
				Player.instance.animator.Play ("superJumpCharge");
			}


			if (_currentChargeTime > 0) 
			{
				_currentChargeTime -= Time.deltaTime;
			}
			else if (Player.instance.animator.CurrentClip.name.Contains("Charge"))
			{
				Player.instance.animator.Play ("superJumpReady");
			}
		}

		if (InputManager.instance.GetSuperJumpEnd()) 
		{
			if (!_isCharging) 
			{
				return;
			}

			_isCharging = false;

			// Execute super jump
			if (_currentChargeTime <= 0) 
			{
				Jump (Constants.SUPER_JUMP_FORCE);
			} 
			else // Let go early
			{
				_currentChargeTime = 0;
				Player.instance.animator.Play ("idle");
			}
		}
	}

	private void UpdateMovement ()
	{
		if (_isCharging || isSliding) 
		{
			return;
		}

		// Get current velocity
		Vector3 newVelocity = myRigidbody.velocity;

		// Listen for button presses
		if (AbilityManager.instance.HasAbility (AbilityManager.Ability.Vector)) 
		{
			if (InputManager.instance.GetJump ()) 
			{
				if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.OnGround) 
				{
					Jump (Constants.JUMP_FORCE * _jumpMod);
				}

				if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Stuck) 
				{
					Jump (Constants.JUMP_FORCE * _jumpMod);
					newVelocity = myRigidbody.velocity;
					newVelocity.x = Constants.RUN_SPEED * mySprite.scale.x;
					myRigidbody.gravityScale = Constants.PLAYER_GRAVITY * myTransform.localScale.y;

					myRigidbody.velocity = newVelocity;

					PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Rising;

					_currentLaunchDuration = Constants.LAUNCH_TIMER;
				}
			}
		}

		if (_currentLaunchDuration > 0)
		{
			_currentLaunchDuration -= Time.deltaTime;
			return;
		}
			
		if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Stuck)
		{
			myRigidbody.velocity = Vector3.zero;
			return;
		}

		// Apply horizontal movement
		newVelocity = myRigidbody.velocity;
		newVelocity.x =  InputManager.instance.GetMovement() * Constants.RUN_SPEED;

		// If our front sensor hit something
		if (isHitForward)
		{
			// If we're trying to move toward the point we hit cancel movement
			if (Mathf.Sign(newVelocity.x) == Mathf.Sign(mySprite.scale.x))
			{
				newVelocity.x = 0;
			}

			if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.OnGround &&
				PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.Stuck &&
				AbilityManager.instance.HasAbility(AbilityManager.Ability.Friction))
			{
				PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Stuck;
				myRigidbody.gravityScale = 0f;
				newVelocity = Vector2.zero;
				Player.instance.animator.Play ("stuck");

				// Flip our current direction
				if (mySprite.scale.x > 0) // facing right
				{
					SetDirection(Enums.Direction.Left);
				}
				else
				{
					SetDirection(Enums.Direction.Right);
				}

				Vector3 l_newPosition = myTransform.position;
				l_newPosition.x = hitPointForward.x + (0.1f * mySprite.scale.x);
				myTransform.position = l_newPosition;

				myRigidbody.velocity = newVelocity;
				return;
			}
		}
		myRigidbody.velocity = newVelocity;

		// Make sure the sprite is facing the right way
		if (mySprite.scale.x > 0) // facing right
		{
			if (InputManager.instance.GetMovement() < 0) // Moving left
			{
				SetDirection(Enums.Direction.Left);
			}
		}

		if (mySprite.scale.x < 0) // facing left
		{
			if (InputManager.instance.GetMovement() > 0) // Moving right
			{
				SetDirection(Enums.Direction.Right);
			}
		}
	}

	private void UpdateGravityControls ()
	{
		if (isSliding || _isCharging || _isLaunching ||
		    PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Stuck) 
		{
			return;
		}

		if (_gravityControlCooldown > 0) 
		{
			_gravityControlCooldown -= Time.deltaTime;
			return;
		}

		if (InputManager.instance.GetGraityFlip()) 
		{
			FlipGravity ();
			_gravityControlCooldown = Constants.GRAVITY_FLIP_COOLDOWN;
		}
	}

	private void SetStates ()
	{
		// Ignore if we're stuck to a wall
		if (PlayerStateManager.instance.currentGroundState == Enums.PlayerGroundState.Stuck)
		{
			return;
		}

		// Set in air states

		if (!isHitG)
		{
			if (myRigidbody.velocity.y > 0.5f)
			{
				if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.Rising)
				{
					PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Rising;
					_isCharging = false;
				}
			}
			else if (myRigidbody.velocity.y < -0.5f)
			{
				if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.Falling)
				{
					PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.Falling;
					_isCharging = false;
				}
			}
		}
		else
		{
			if (PlayerStateManager.instance.currentGroundState != Enums.PlayerGroundState.OnGround)
			{
				PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.OnGround;
			}
		}

		// Set running state
		if (Mathf.Abs(myRigidbody.velocity.x) > 0.5f && !PlayerStateManager.instance.isMoving)
		{
			PlayerStateManager.instance.isMoving = true;
		}
		else if (Mathf.Abs(myRigidbody.velocity.x) <= 0.5f && PlayerStateManager.instance.isMoving)
		{
			PlayerStateManager.instance.isMoving = false;
		}
	}

	private void SetDirection (Enums.Direction p_direction)
	{
		Vector3 newScale = mySprite.scale;
		newScale.x *= -1;

		Vector3 l_followPosition = _follow.localPosition; 
		l_followPosition.x = Mathf.Abs(l_followPosition.x);

		if (p_direction == Enums.Direction.Left)
		{
			l_followPosition.x *= -1f;
		}

		mySprite.scale = newScale;
		hitboxes.localScale = newScale;
		_follow.localPosition = l_followPosition;
	}

	public void FlipGravity ()
	{
		myRigidbody.gravityScale *= -1f;
		Vector3 l_newScale = myTransform.localScale;
		l_newScale.y *= -1f;
		myTransform.localScale = l_newScale;

		// Make the sprite appear to be in the same place once we flip upside down
		Vector3 l_newPosition = myTransform.position;
		l_newPosition.y -= normalHitBox.size.y * myTransform.localScale.y;
		myTransform.position = l_newPosition;
	}

	void Attack()
	{
		attackHitbox.SetActive(true);
	}

	private void Jump (float p_force)
	{
		Vector3 l_newVelocity = myRigidbody.velocity;
		l_newVelocity.y = p_force * myTransform.localScale.y;
		myRigidbody.velocity = l_newVelocity;
	}

	public void Respawn ()
	{
		PlayerStateManager.instance.currentGroundState = Enums.PlayerGroundState.OnGround;
	}
}
