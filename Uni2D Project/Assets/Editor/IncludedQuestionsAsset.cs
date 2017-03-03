using UnityEngine;
using UnityEditor;
using System.Collections;

public class IncludedQuestionsAsset
{
	[MenuItem("Assets/Create/Asset Lists/Included Questions")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<IncludedQuestions> ();
	}
}