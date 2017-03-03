using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EquationEditorWindow : EditorWindow
{
	public static EquationEditorWindow equationEditorWindow;
	public static EquationInfo sentEquationInfo;
	private EquationInfo _equationInfo;

	private Vector2 scrollPos;

	// The various styles for our GUI
	private string titleStyle;
	private string removeButtonStyle;
	private string addButtonStyle;
	private string rootGroupStyle;
	private string subGroupStyle;
	private string arrayElementStyle;
	private string subArrayElementStyle;
	private string toggleStyle;
	private string foldStyle;
	private string enumStyle;
	private GUIStyle labelStyle;

//	private List<Answer> _answers;

	[MenuItem("Window/Data Type Editors/Equation Editor")]
	public static void Init()
	{
		equationEditorWindow = EditorWindow.GetWindow<EquationEditorWindow>(false, "Equation", true);
		equationEditorWindow.Show();
		equationEditorWindow.Populate();
	}

	void OnSelectionChange()
	{
		Populate();
		Repaint();
	}

	void OnEnable()
	{
		Populate();
	}

	void OnFocus()
	{
		Populate();
	}

	void Populate()
	{
		// Style Definitions
		titleStyle = "MeTransOffRight";
		removeButtonStyle = "TL SelectionBarCloseButton";
		addButtonStyle = "CN CountBadge";
		rootGroupStyle = "GroupBox";
		subGroupStyle = "ObjectFieldThumb";
		arrayElementStyle = "flow overlay box";
		subArrayElementStyle = "HelpBox";
		foldStyle = "Foldout";
		enumStyle = "MiniPopup";
		toggleStyle = "BoldToggle";

		labelStyle = new GUIStyle();
		labelStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.fontStyle = FontStyle.Bold;
		labelStyle.normal.textColor = Color.white;


		if (sentEquationInfo != null){
			EditorGUIUtility.PingObject( sentEquationInfo );
			Selection.activeObject = sentEquationInfo;
			sentEquationInfo = null;
		}

		Object[] selection = Selection.GetFiltered(typeof(EquationInfo), SelectionMode.Assets);
		if (selection.Length > 0){
			if (selection[0] == null) return;
			_equationInfo = (EquationInfo) selection[0];
		}

		/*if (_equationInfo.answers != null)
		{
			_answers = _equationInfo.answers.ToList();
		}
		else
		{
			_answers = new List<Answer>();
		}*/

	}

	public void OnGUI()
	{
		if (_equationInfo == null)
		{
			GUILayout.BeginHorizontal("GroupBox");
			GUILayout.Label("Select an equation file or create a new equation.","CN EntryInfo");
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("Create new equation"))
				ScriptableObjectUtility.CreateAsset<EquationInfo> ();
			return;
		}

		GUIStyle fontStyle = new GUIStyle();
		fontStyle.font = (Font) EditorGUIUtility.Load("EditorFont.TTF");
		fontStyle.fontSize = 30;
		fontStyle.alignment = TextAnchor.UpperCenter;
		fontStyle.normal.textColor = Color.white;
		fontStyle.hover.textColor = Color.white;

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		{
			SerializedObject l_serializedEquation = new SerializedObject(_equationInfo);

			// Draw the message data

			EditorGUILayout.BeginVertical();
			{
				EditorGUIUtility.labelWidth = 90;
				_equationInfo.equationName = EditorGUILayout.TextField("Name:", _equationInfo.equationName);
				_equationInfo.key = EditorGUILayout.TextField("Key:", _equationInfo.key);

				EditorGUILayout.LabelField ("Prompt:");
				Rect rect = GUILayoutUtility.GetRect(50, 70);
				EditorStyles.textField.wordWrap = true;
				_equationInfo.prompt = EditorGUI.TextArea(rect, _equationInfo.prompt);
				_equationInfo.answer = EditorGUILayout.IntField ("Answer: ", _equationInfo.answer);
				_equationInfo.answerUnits = EditorGUILayout.TextField("Answer Units:", _equationInfo.answerUnits);

//				ShowMessages();
			}EditorGUILayout.EndVertical();

			EditorGUIUtility.labelWidth = 150;
			EditorGUILayout.Space();

		}EditorGUILayout.EndScrollView();

		if (GUI.changed) 
		{
			Undo.RecordObject(_equationInfo, "Equation Editor Modify");
			EditorUtility.SetDirty(_equationInfo);
		}
	}

	/// <summary>
	/// Atlas selection callback.
	/// </summary>

	/*void OnSelectAtlas (Object obj)
	{
		SerializedObject l_serializedEquation = new SerializedObject(_equationInfo);
		l_serializedEquation.Update();
		SerializedProperty sp = l_serializedEquation.FindProperty("atlas");
		sp.objectReferenceValue = obj;
		l_serializedEquation.ApplyModifiedProperties();
		NGUITools.SetDirty(l_serializedEquation.targetObject);
		NGUISettings.atlas = obj as UIAtlas;

		Undo.RecordObject(_equationInfo, "Equation Editor Modify");
		EditorUtility.SetDirty(_equationInfo);
		Repaint();
	}*/

	/// <summary>
	/// Sprite selection callback function.
	/// </summary>

	/*void SelectSprite (string spriteName)
	{
		SerializedObject l_serializedEquation = new SerializedObject(_equationInfo);
		l_serializedEquation.Update();
		_equationInfo.spriteName = spriteName;
		l_serializedEquation.ApplyModifiedProperties();
//		NGUITools.SetDirty(l_serializedEquation.targetObject);
		NGUISettings.selectedSprite = spriteName;

		Undo.RecordObject(_equationInfo, "Equation Editor Modify");
		EditorUtility.SetDirty(_equationInfo);
		Repaint();
	}*/

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>

	/*public void PreviewSprite (Rect p_rect)
	{
		if (_equationInfo.atlas == null) return;

		UISpriteData l_spriteData = _equationInfo.atlas.GetSprite(_equationInfo.spriteName);

		if (l_spriteData == null) return;

		Material l_atlasMaterial = _equationInfo.atlas.spriteMaterial;
		Texture2D l_tex = l_atlasMaterial.mainTexture as Texture2D;

		if (l_tex == null) return;
		
		NGUIEditorTools.DrawSprite(l_tex, p_rect, l_spriteData, Color.white);
	}*/

	/*private void ShowMessages ()
	{
		GUIStyle l_darkStyle = new GUIStyle();
		l_darkStyle.normal.background = MakeTex(600, 1, new Color(0.5f, 0.5f, 0.5f));
		l_darkStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);

		for (int i = 0; i < _answers.Count; i++)
		{
			Answer l_currentAnswer = _answers[i];

			if(i%2 == 0)
			{
				EditorGUILayout.BeginVertical(l_darkStyle);
			}
			else
			{
				EditorGUILayout.BeginVertical();
			}

			EditorGUILayout.BeginHorizontal();
			{
				l_currentAnswer.isCorrect = EditorGUILayout.Toggle ("Correct Answer: ", l_currentAnswer.isCorrect, GUILayout.Width(110f));
				Rect rect = GUILayoutUtility.GetRect(50, 70);
				EditorStyles.textField.wordWrap = true;
				l_currentAnswer.text = EditorGUI.TextArea(rect, l_currentAnswer.text);

				if (GUILayout.Button("-", GUILayout.Width(20f)))
				{
					l_currentAnswer.dirty = true;
				}
			}EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		for (int i = 0; i < _answers.Count; i++)
		{
			Answer l_currentAnswer = _answers[i];

			if (l_currentAnswer.dirty)
			{
				_answers.Remove(l_currentAnswer);
			}
		}

		if (GUILayout.Button("Add Answer", GUILayout.Width(100f)))
		{
			Answer l_newAnswer = new Answer();
			_answers.Add(l_newAnswer);
		}

		_equationInfo.answers = _answers.ToArray();
		EditorUtility.SetDirty(_equationInfo);
		Repaint();
	}*/

	private Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] pix = new Color[width*height];

		for(int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();

		return result;
	}
}