using UnityEngine;
using System.Collections;

public class SetFontFilterModeScript : MonoBehaviour 
{
	public Font[] fontsToChange;
	public FilterMode filterMode;

	// Use this for initialization
	void OnEnable() 
	{
		for(int i = 0; i < fontsToChange.Length; i++)
		{
			fontsToChange[i].material.mainTexture.filterMode = filterMode;
		}
	}
}