using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectInputField : MonoBehaviour 
{
	public InputField inputField;

	public void OnClick ()
	{
		Debug.Log ("clicked input field");
		inputField.Select ();
	}
}
