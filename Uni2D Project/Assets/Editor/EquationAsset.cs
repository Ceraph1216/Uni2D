using UnityEngine;
using UnityEditor;
using System.Collections;

public class EquationAsset
{
	[MenuItem("Assets/Create/Data Type/Equation Info")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<EquationInfo> ();
	}
}