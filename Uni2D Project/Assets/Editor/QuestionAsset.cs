using UnityEngine;
using UnityEditor;
using System.Collections;

public class QuestionAsset
{
	[MenuItem("Assets/Create/Data Type/Question Info")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<QuestionInfo> ();
	}
}