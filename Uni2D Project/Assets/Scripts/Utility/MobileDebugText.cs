using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class MobileDebugText : MonoBehaviour 
{
	public static MobileDebugText instance;

	private Text _text;

	void Awake ()
	{
		instance = this;
		_text = GetComponent<Text> ();
	}

	public void SetDebugText (string p_text)
	{
		_text.text = p_text;
	}
}
