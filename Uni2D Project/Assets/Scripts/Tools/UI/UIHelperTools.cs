using UnityEngine;
using System.Collections;

public class UIHelperTools : MonoBehaviour 
{
	private static Camera _uiCamera;

	void Awake()
	{
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += LevelLoaded;

		Init();
	}

	void LevelLoaded(UnityEngine.SceneManagement.Scene p_scene, UnityEngine.SceneManagement.LoadSceneMode p_sceneMode)
	{
		Init();
	}

	static void Init()
	{
		_uiCamera = GameObject.Find("UI Root").transform.Find("Camera").GetComponent<Camera>();
	}

	public static Vector3 MousePositionToNGUI(Transform l_parent)
	{
		return l_parent.InverseTransformPoint(_uiCamera.ScreenToWorldPoint(Input.mousePosition));
	}

	public static Vector3 WorldPositionToNGUI(Vector3 l_position)
	{
		// Relative ratio = Camera.main.pixelWidth (actual width) / 960 (desired width)
		float _relativeRatio = (Camera.main.pixelWidth / 960.0F);

		// Get the screen point
		Vector3 l_point = Camera.main.WorldToScreenPoint(l_position);

		// Account for different window sizes
		l_point = new Vector3(l_point.x / _relativeRatio, l_point.y / _relativeRatio, l_point.z);

		// Convert to NGUI coordinates
		l_point = l_point - new Vector3(480.0F, 320.0F, 0);

		return l_point;
	}
}