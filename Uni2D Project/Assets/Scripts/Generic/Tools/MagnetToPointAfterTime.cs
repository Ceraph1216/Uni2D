using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetToPointAfterTime : MonoBehaviour 
{
	[HideInInspector]
	public bool active;
	
	private Transform magnetPoint;
	private List<Transform> allHookPoints;
	private Transform currentHookPoint;
	private int currentHookIndex;
	private Transform myXform;
	private Camera uiCam;
	private Camera mainCam;
	private TrailRenderer trail;
	
	float timeToArriveAtUI = 1.5f;
	float timeToArriveAtHook = 0.1f;
	float timer = 0f;
	float uiTimer;

	float waitTime = 2.0f;
	float waitTimer = 0f;
	
	void Awake() 
	{
		myXform = transform;
		trail = GetComponent<TrailRenderer>();
		uiCam = GameObject.Find("UI Root/Camera").GetComponent<Camera>();
		mainCam = Camera.main;
		magnetPoint = GameObject.Find("MainCamera/CoinLocator").transform;
		allHookPoints = new List<Transform>();
		allHookPoints.Add(GameObject.Find("MainCamera/HookPoints/HookLocator0").transform);
		allHookPoints.Add(GameObject.Find("MainCamera/HookPoints/HookLocator1").transform);
		allHookPoints.Add(GameObject.Find("MainCamera/HookPoints/HookLocator2").transform);
		allHookPoints.Add(GameObject.Find("MainCamera/HookPoints/HookLocator3").transform);
		allHookPoints.Add(GameObject.Find("MainCamera/HookPoints/HookLocator4").transform);
		allHookPoints.Add(GameObject.Find("MainCamera/CoinLocator").transform);
	}
	
	public void Pickup() 
	{
		active = true;
		timer = 0f;
		waitTimer = 0f;
		trail.enabled = true;
		GetComponent<ParticleSystem>().enableEmission = true;
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<Rigidbody2D>().AddForce( new Vector2( UnityEngine.Random.Range( -100f, 300f ), UnityEngine.Random.Range( 0, 600f ) ) );
	}
	
	void OnEnable()
	{
		trail.enabled = false;
		currentHookIndex = 0;
		active = false;
		currentHookPoint = allHookPoints[currentHookIndex];
		SoftPauseScript.instance.AddToHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler (Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	void SoftUpdate (GameObject dispatcher) 
	{
		if (active) 
		{
			if ( waitTimer < waitTime ) 
			{
				waitTimer += Time.deltaTime;
			}
			else 
			{
				timer += Time.deltaTime;
				if ( currentHookIndex == allHookPoints.Count - 1 ) 
				{
					if ( timer < timeToArriveAtHook ) 
					{
						myXform.position = new Vector3( Mathf.Max( Mathf.Lerp( myXform.position.x, currentHookPoint.position.x, 8f*Time.deltaTime ), currentHookPoint.position.x ),
						                               Mathf.Min( Mathf.Lerp( myXform.position.y, currentHookPoint.position.y, 16f*Time.deltaTime ), currentHookPoint.position.y  ),
						                               myXform.position.z);
					}
					if ( timer >= timeToArriveAtHook && Vector3.Distance( myXform.position, currentHookPoint.position ) < .5 ) 
					{
						gameObject.GetComponent<TrailRenderer>().enabled = false;
					}
					if ( timer >= timeToArriveAtHook || 
					    Vector3.Distance( myXform.position, currentHookPoint.position ) < .1 ||
					    myXform.position.x < currentHookPoint.position.x ) 
					{
						gameObject.SetActive(false);
					}
				}
				else 
				{
					if ( timer < timeToArriveAtHook ) 
					{
						myXform.position = new Vector3( Mathf.Lerp( myXform.position.x, currentHookPoint.position.x, 8f*Time.deltaTime ),
						                               Mathf.Lerp( myXform.position.y, currentHookPoint.position.y, 8f*Time.deltaTime ),
						                               myXform.position.z);
					}
					if ( timer >= timeToArriveAtHook || 
					    Vector3.Distance( myXform.position, currentHookPoint.position ) < .3 && currentHookIndex < allHookPoints.Count - 1 ) 
					{
						timer = 0f;
						currentHookIndex += 1;
						currentHookPoint = allHookPoints[currentHookIndex];
					}
				}
			}
		}
	}
}