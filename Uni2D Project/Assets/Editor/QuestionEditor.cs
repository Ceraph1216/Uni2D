using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(QuestionInfo))]
public class QuestionEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Question Editor")) 
			QuestionEditorWindow.Init();
	}
}