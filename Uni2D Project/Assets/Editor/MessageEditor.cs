using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MessageInfo))]
public class MessageEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Message Editor")) 
			MessageEditorWindow.Init();
	}
}