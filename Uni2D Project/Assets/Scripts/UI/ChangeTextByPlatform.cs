using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ChangeTextByPlatform : MonoBehaviour 
{
	public string mobileText;
	public string webText;

	private TextMeshProUGUI _label;

	void Awake ()
	{
		_label = GetComponent<TextMeshProUGUI> ();
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
