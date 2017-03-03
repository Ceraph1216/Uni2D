using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EquationInfo))]
public class EquationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Open Equation Editor")) 
			EquationEditorWindow.Init();
	}
}