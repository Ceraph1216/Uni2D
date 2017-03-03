using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class DebugSetText : MonoBehaviour 
{
	public string text;

	private Text _text;

	void Awake ()
	{
		_text = GetComponent<Text> ();
	}

	void OnEnable ()
	{
		_text.text = text;
	}
}
