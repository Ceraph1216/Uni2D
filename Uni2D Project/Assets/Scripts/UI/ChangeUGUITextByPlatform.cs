using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class ChangeUGUITextByPlatform : MonoBehaviour 
{
	public string mobileText;
	public string webText;

	private Text _text;

	void Awake ()
	{
		_text = GetComponent<Text> ();
	}

	void OnEnable ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			_text.text = mobileText;
		} 
		else 
		{
			_text.text = webText;
		}
	}
}
