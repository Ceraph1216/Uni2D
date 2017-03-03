using UnityEngine;
using UnityEditor;
using System.IO;

public static class ScriptableObjectUtility
{
    public static void CreateAsset<T> () where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T> ();
        
        string path = AssetDatabase.GetAssetPath (Selection.activeObject);
        if (path == "") 
        {
            path = "Assets/Resources/Data";
        } 
        else if (Path.GetExtension (path) != "") 
        {
            path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
        }
		
        string fileName;
		if (asset is MessageInfo) 
		{
			path = "Assets/Resources/Data/Messages";
			fileName = "New Message";
		}
		else if (asset is QuestionInfo) 
		{
			path = "Assets/Resources/Data/Questions";
			fileName = "New Question";
		}
		else if (asset is EquationInfo) 
		{
			path = "Assets/Resources/Data/Equations";
			fileName = "New Equation";
		}
		else
		{
			fileName = typeof(T).ToString();
		}
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath (path + "/" + fileName + ".asset");

        AssetDatabase.CreateAsset (asset, assetPathAndName);
        
        AssetDatabase.SaveAssets ();
        Selection.activeObject = asset;
        EditorUtility.FocusProjectWindow ();
		
		if (asset is MessageInfo) 
		{
			MessageEditorWindow.Init();
		}
		else if (asset is QuestionInfo) 
		{
			QuestionEditorWindow.Init();
		}
		else if (asset is EquationInfo) 
		{
			EquationEditorWindow.Init();
		}
    }
}
