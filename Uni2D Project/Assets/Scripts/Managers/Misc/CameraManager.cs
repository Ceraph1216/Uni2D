using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
	static CameraManager mInstance;
	
	// The instance of the CameraManager class
	static public CameraManager instance
	{
		get
		{
			return mInstance;
		}
	}

	public Camera main;
	public Camera uiCamera;

	//public GameObject anchor;
	//public TweenAlpha flash;
	private Image flashSprite;
	private CameraShake cameraShake;

	//private float lastPos;

	// Use this for initialization
	void Awake () 
	{
		mInstance = this;
		//flashSprite = flash.GetComponent<Image>();
		cameraShake = Camera.main.GetComponent<CameraShake>();

		//lastPos = main.transform.position.x;
	}

	public void ShakeCamera(float l_intensity, float l_decay, float l_maxShake)
	{
		cameraShake.Shake(l_intensity, l_decay, l_maxShake);
	}

	/*public void FlashScreen(Color l_color, float time = 0.05F)
	{
		flashSprite.color = l_color;

		flash.duration = time;
		flash.ResetToBeginning();
		flash.PlayForward();
	}*/

	private void ResetPosition(Transform t)
	{
		t.GetComponent<ShakeForTimeBehavior>().OnFinished -= ResetPosition;
		t.position = new Vector3(t.position.x, 0, t.position.z);
	}

	/*void Update()
	{
		Debug.Log(main.transform.position.x - lastPos);
		lastPos = main.transform.position.x;
	}*/
}