using UnityEngine;
using System.Collections;

public class EnableTween : MonoBehaviour 
{
	public UITweener tween;

	void OnEnable ()
	{
		tween.ResetToBeginning ();
		tween.enabled = true;
		tween.PlayForward ();
	}
}