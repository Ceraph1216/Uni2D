using UnityEngine;
using UnityEditor;
using System.Collections;

public class IncludedEquationsAsset
{
	[MenuItem("Assets/Create/Asset Lists/Included Equations")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<IncludedEquations> ();
	}
}