using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UILabel))]
public class ChangeTextByPlatform : MonoBehaviour 
{
	public string mobileText;
	public string webText;

	private UILabel _label;

	void Awake ()
	{
		_label = GetComponent<UILabel> ();
	}

	void OnEnable ()
	{
		if (PlatformManager.instance.isMobile) 
		{
			_label.text = mobileText;
		} 
		else 
		{
			_label.text = webText;
		}
	}
}
