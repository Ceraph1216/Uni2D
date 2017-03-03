using UnityEngine;
using UnityEditor;
using System.Collections;

public class MessageAsset
{
	[MenuItem("Assets/Create/Data Type/Message Info")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<MessageInfo> ();
	}
}