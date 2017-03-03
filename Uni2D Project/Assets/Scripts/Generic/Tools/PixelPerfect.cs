using UnityEngine;
using System.Collections;

public class PixelPerfect : MonoBehaviour 
{

	public static float ToPixels(float unityUnits, Camera viewingCamera)
	{
		//return 0;
		//Debug.Log("HEIGHT: " + Screen.height );
		//Debug.Log("WIDTH: " + Screen.width );
		float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
		return valueInPixels;
	}
	
	public static float ToUnityUnits(float pixels, Camera viewingCamera)
	{
		//return 0;
		//Debug.Log("HEIGHT: " + Screen.height );
		//Debug.Log("WIDTH: " + Screen.width );
		float valueInUnits = pixels / (Screen.height / (viewingCamera.orthographicSize * 2));
		return valueInUnits;
	}
	
	public static float RoundToNearestPixel(float unityUnits, Camera viewingCamera)
	{
		//return 0;
		//Debug.Log("HEIGHT: " + Screen.height );
		//Debug.Log("WIDTH: " + Screen.width );
		float valueInPixels = (Screen.height / (viewingCamera.orthographicSize * 2)) * unityUnits;
		valueInPixels = Mathf.Round(valueInPixels);
		float adjustedUnityUnits = valueInPixels / (Screen.height / (viewingCamera.orthographicSize * 2));
		return adjustedUnityUnits;
	}
}