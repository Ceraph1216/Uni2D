using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class PlatformManager : MonoBehaviour 
{
	[DllImport("__Internal")]
	private static extern bool DetectMobile();

	public static PlatformManager instance;

	public bool isMobile;

	public Vector2 screenDimensions;

	public float screenLeft
	{
		get
		{
			return -(screenDimensions.x / 2);
		}
	}

	public float screenRight
	{
		get
		{
			return (screenDimensions.x / 2);
		}
	}

	public float screenTop
	{
		get
		{
			return (screenDimensions.y / 2);
		}
	}

	public float screenBottom
	{
		get
		{
			return -(screenDimensions.y / 2);
		}
	}

	void Awake ()
	{
		instance = this;

		#if UNITY_EDITOR
		isMobile = false;
		#else
		isMobile = DetectMobile();
		#endif

		screenDimensions.y = Camera.main.orthographicSize * 2f;
		screenDimensions.x = screenDimensions.y * Screen.width / Screen.height;
	}
}
