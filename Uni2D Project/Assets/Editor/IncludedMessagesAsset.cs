using UnityEngine;
using UnityEditor;
using System.Collections;

public class IncludedMessagesAsset
{
	[MenuItem("Assets/Create/Asset Lists/Included Messages")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<IncludedMessages> ();
	}
}