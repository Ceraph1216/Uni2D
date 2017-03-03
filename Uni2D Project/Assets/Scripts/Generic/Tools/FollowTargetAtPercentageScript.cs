using UnityEngine;
using System.Collections;

public class FollowTargetAtPercentageScript : MonoBehaviour 
{
	public Transform target;
	public bool followX;
	public bool followY;
	public bool followZ;
	public float relativeSpeed;

	public Vector3 previousVector;
	private Vector3 deltaVector;
	
	private Transform myTransform;
	private Camera camera;
	
	void Awake()
	{
		myTransform = transform;
		camera = Camera.main;
	}
	
	// Use this for initialization
	void Start () 
	{
		Initialize();
	}
	
	void OnEnable ()
	{
		// Set up update event listeners
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);
		Initialize();
	}
	
	void OnDisable()
	{
		// Make sure the update function is removed from the delegate when the script is not running
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	public void Initialize()
	{
		previousVector = target.position;
	}
	
	// Update is called once per frame
	void SoftUpdate (GameObject dispatcher)
	{
		deltaVector = new Vector3(0,0,0);
		if (followX)
		{
			deltaVector.x = target.position.x - previousVector.x;
		}
		if (followY)
		{
			deltaVector.y = target.position.y - previousVector.y;
		}
		if (followZ)
		{
			deltaVector.z = target.position.z - previousVector.z;
		}
		
		Vector3 pixelPerfectVector = new Vector3(PixelPerfect.RoundToNearestPixel((deltaVector.x  * relativeSpeed), camera), PixelPerfect.RoundToNearestPixel((deltaVector.y  * relativeSpeed), camera), PixelPerfect.RoundToNearestPixel((deltaVector.z  * relativeSpeed), camera));
		
		myTransform.Translate(pixelPerfectVector);
		
		previousVector = target.position;
	}
}
