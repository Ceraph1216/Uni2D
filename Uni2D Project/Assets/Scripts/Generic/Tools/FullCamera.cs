using UnityEngine;
using System.Collections;

public class FullCamera : MonoBehaviour 
{
	public float xMargin = 0f;//0.05f;//0.015f;
	public float yMargin = 0f;//0f;
	public float xSmooth = 16f;//8f;//8f;//16f;//16f;
	public float ySmooth = 4f;//2f;
	float zoomSmooth = -7f;//50f;
	public Vector2 minXAndY = new Vector2( 1.5f, 0.5f );
	public Vector2 maxXAndY = new Vector2( 1.0f, 0.5f );
	Vector3 targetPosition;
	
	private Transform playerXform;
	private Transform cameraXform;
	private Camera camera;

	float zoomIncrement = 0f;
	float normal = -30f;//1.4f;
	float smooth = 10f;//2f;
	float maxZoomOut = -17.5f;//-30f;
	float maxZoomIn = -15f;//40.0f;
	float zoomSpeedToTargetFactor = -3.0f;//-1.75f;//0.3f;
	private float zoomTarget;
	private GameObject player;
	
	float minShake = -.1f;
	float maxShake = .1f;
	float shakeTimer = -1f;
	float shakeTime = 0.25f;

	public enum State { Normal, Trick };
	public State currentState;
	float trickZoom = -10.0f;

	void OnEnable()
	{
		//SoftPauseScript.instance.SoftUpdate += SoftUpdate;
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.FixedSoftUpdate, SoftLateUpdate);
	}
	
	void Disable()
	{
		//SoftPauseScript.instance.SoftUpdate -= SoftUpdate;
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.FixedSoftUpdate, SoftLateUpdate);
	}
	
	// Update is called once per frame
	void SoftUpdate (GameObject dispatcher) 
	{
		ZoomUpdate();
	}

	// Update is called once per frame
	void SoftLateUpdate (GameObject dispatcher) 
	{
		FollowLateUpdate();
		ShakeLateUpdate();
	}

	void Awake() 
	{
		FollowAwake();
		ZoomAwake();
	}
	
	void Start() 
	{
		ZoomStart();
	}

	void ShakeLateUpdate() 
	{

		if ( shakeTimer >= 0 ) {
			Vector3 newZoomPos = cameraXform.position;
			newZoomPos.z = zoomTarget + UnityEngine.Random.Range( minShake, maxShake ); 
			cameraXform.position = newZoomPos;


			shakeTimer += Time.deltaTime;
//			Debug.Log("Shake Timer: " + shakeTimer);
			if ( shakeTimer >= shakeTime ) 
			{
				shakeTimer = -1.0f;
			}
		}
	}

	public void StartShake( float newMinShake, float newMaxShake, float newShakeTime ) 
	{
		shakeTimer = 0.0f;
		shakeTime = newShakeTime;
		minShake = newMinShake;
		maxShake = newMaxShake;
	}

	public void EndShake() 
	{
		shakeTimer = -1.0f;
	}

	public void SetZoomState( State newState ) 
	{
		currentState = newState;
	}

	//TODO: tweak deadzone for less motion during loops
	// Figure out camera transform to keep player centered
	void ZoomAwake() 
	{
		player = GameObject.FindGameObjectWithTag("cameraTarget");
		camera = GetComponent<Camera>();
		cameraXform = camera.transform;
	}
	
	void ZoomStart() 
	{
		zoomTarget = normal;
		currentState = State.Normal;
	}
	
	void ZoomUpdate () 
	{
		if (currentState == State.Normal) 
		{
			/*if ( PlayerStateManager.instance.currentBoostState != Enums.PlayerBoostState.Boosting ) 
			{
				zoomTarget = player.rigidbody2D.velocity.magnitude * zoomSpeedToTargetFactor;//maxZoomOut;
				//Debug.Log("TARGET: " + zoomTarget );
			}
			else 
			{
				zoomTarget = cameraXform.position.z;
			}*/
			zoomTarget = maxZoomIn;
			zoomTarget = Mathf.Clamp( zoomTarget, maxZoomOut, maxZoomIn );
		}
		else 
		{
			zoomTarget = trickZoom;
		}
		Vector3 newZoomPos = cameraXform.position;
		newZoomPos.z = zoomTarget;
		//newZoomPos.z = ExpEase.Out( newZoomPos.z, zoomTarget, zoomSmooth*Time.deltaTime ); 
		cameraXform.position = newZoomPos;
	}
	
	void FollowAwake ()
	{
		// Setting up the reference.
		playerXform = GameObject.FindGameObjectWithTag("cameraTarget").transform;
	}

	bool CheckXMargin ()
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
	//	Debug.Log( "X MARGIN: " + xMargin + " DISTANCE: " + Mathf.Abs(cameraXform.position.x - playerXform.position.x) );
		return Mathf.Abs(cameraXform.position.x - playerXform.position.x) > xMargin;
	}
	
	bool CheckYMargin ()
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(cameraXform.position.y - playerXform.position.y) > yMargin;
	}
	
	// switched to late update for smooth tracking
	void FollowLateUpdate () 
	{
		TrackPlayer();
	}
	
	void TrackPlayer () 
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = cameraXform.position.x;
		float targetY = cameraXform.position.y;

		// If the player has moved beyond the x margin...
		if ( CheckXMargin() ) 
		{
			//TODO: The problem is that this never fires.
	//		Debug.Log ("MARGIN PASSED");
			// ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
			targetX = Mathf.Lerp( cameraXform.position.x, playerXform.position.x, xSmooth * Time.deltaTime );
		}
		
		// If the player has moved beyond the y margin...
		if ( CheckYMargin() ) 
		{
			// ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
			targetY = Mathf.Lerp( cameraXform.position.y, playerXform.position.y, ySmooth * Time.deltaTime );
		}

		// The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
	//	Debug.Log("CLAMP -> TARGET X: " + ( targetX - cameraXform.position.x ) + "\nMIN: " + minXAndY.x + ", MAX: " + maxXAndY.x );
	//	Debug.Log("TEST POS: " + (minXAndY.x + cameraXform.position.x));
	//	Debug.Log("LOW CLAMP: " + ( playerXform.position.x + minXAndY.x ));
	//	Debug.Log("HIGH CLAMP: " + ( playerXform.position.x + maxXAndY.x ));
		targetX = Mathf.Clamp( targetX, playerXform.position.x + minXAndY.x, playerXform.position.x + maxXAndY.x );

		/*if ( PlayerStateManager.instance.currentBoostState == Enums.PlayerBoostState.Boosting ) {
			targetX = Mathf.Clamp( targetX, playerTransform.position.x + minXAndY.x, playerTransform.position.x + maxXAndY.x );
		}
		else {
			targetX = Mathf.Clamp( targetX, playerTransform.position.x + maxXAndY.x, playerTransform.position.x + minXAndY.x );
		}*/
		targetY = Mathf.Clamp( targetY, playerXform.position.y + minXAndY.y, playerXform.position.y + maxXAndY.y );
		
		// Set the camera's position to the target position with the same z component.
		targetPosition = new Vector3( targetX, targetY, cameraXform.position.z );
		Vector3 l_newPosition = Vector3.Lerp( transform.position, targetPosition, 10.0f * Time.deltaTime );
		l_newPosition.x = Mathf.Round(l_newPosition.x * 100f) / 100f;
		l_newPosition.y = Mathf.Round(l_newPosition.y * 100f) / 100f;
		cameraXform.position = l_newPosition;
		//cameraXform.position = new Vector3( targetX, targetY, cameraXform.position.z );

		//Debug.DrawLine( new Vector3( Camera.main.transform.position.x + minXAndY.x, Camera.main.transform.position.y + minXAndY.y, 0f ), new Vector3( Camera.main.transform.position.x + maxXAndY.x, Camera.main.transform.position.y + minXAndY.y, 0f ), Color.red );
		//Debug.DrawLine( new Vector3( Camera.main.transform.position.x + minXAndY.x, Camera.main.transform.position.y + maxXAndY.y, 0f ), new Vector3( Camera.main.transform.position.x + maxXAndY.x, Camera.main.transform.position.y + maxXAndY.y, 0f ), Color.blue );
	}
}