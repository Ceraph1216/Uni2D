using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestionEditorWindow : EditorWindow
{
	public static QuestionEditorWindow questionEditorWindow;
	public static QuestionInfo sentQuestionInfo;
	private QuestionInfo _questionInfo;

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

	private List<Answer> _answers;

	[MenuItem("Window/Data Type Editors/Question Editor")]
	public static void Init()
	{
		questionEditorWindow = EditorWindow.GetWindow<QuestionEditorWindow>(false, "Question", true);
		questionEditorWindow.Show();
		questionEditorWindow.Populate();
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


		if (sentQuestionInfo != null){
			EditorGUIUtility.PingObject( sentQuestionInfo );
			Selection.activeObject = sentQuestionInfo;
			sentQuestionInfo = null;
		}

		Object[] selection = Selection.GetFiltered(typeof(QuestionInfo), SelectionMode.Assets);
		if (selection.Length > 0){
			if (selection[0] == null) return;
			_questionInfo = (QuestionInfo) selection[0];
		}

		if (_questionInfo.answers != null)
		{
			_answers = _questionInfo.answers.ToList();
		}
		else
		{
			_answers = new List<Answer>();
		}

	}

	public void OnGUI()
	{
		if (_questionInfo == null)
		{
			GUILayout.BeginHorizontal("GroupBox");
			GUILayout.Label("Select a question file or create a new question.","CN EntryInfo");
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("Create new question"))
				ScriptableObjectUtility.CreateAsset<QuestionInfo> ();
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
			SerializedObject l_serializedQuestion = new SerializedObject(_questionInfo);

			// Draw the message data

			EditorGUILayout.BeginVertical();
			{
				EditorGUIUtility.labelWidth = 90;
				_questionInfo.questionName = EditorGUILayout.TextField("Name:", _questionInfo.questionName);
				_questionInfo.key = EditorGUILayout.TextField("Key:", _questionInfo.key);
				_questionInfo.imageURL = EditorGUILayout.TextField("Image url:", _questionInfo.imageURL);

				EditorGUILayout.LabelField ("Prompt:");
				Rect rect = GUILayoutUtility.GetRect(50, 70);
				EditorStyles.textField.wordWrap = true;
				_questionInfo.prompt = EditorGUI.TextArea(rect, _questionInfo.prompt);

				EditorGUILayout.LabelField ("Post answer dialogue:");
				Rect rect2 = GUILayoutUtility.GetRect(50, 70);
				EditorStyles.textField.wordWrap = true;
				_questionInfo.answerExplanation = EditorGUI.TextArea(rect2, _questionInfo.answerExplanation);
				ShowMessages();
			}EditorGUILayout.EndVertical();

			EditorGUIUtility.labelWidth = 150;
			EditorGUILayout.Space();

		}EditorGUILayout.EndScrollView();

		if (GUI.changed) 
		{
			Undo.RecordObject(_questionInfo, "Question Editor Modify");
			EditorUtility.SetDirty(_questionInfo);
		}
	}

	/// <summary>
	/// Atlas selection callback.
	/// </summary>

	/*void OnSelectAtlas (Object obj)
	{
		SerializedObject l_serializedQuestion = new SerializedObject(_questionInfo);
		l_serializedQuestion.Update();
		SerializedProperty sp = l_serializedQuestion.FindProperty("atlas");
		sp.objectReferenceValue = obj;
		l_serializedQuestion.ApplyModifiedProperties();
		NGUITools.SetDirty(l_serializedQuestion.targetObject);
		NGUISettings.atlas = obj as UIAtlas;

		Undo.RecordObject(_questionInfo, "Question Editor Modify");
		EditorUtility.SetDirty(_questionInfo);
		Repaint();
	}*/

	/// <summary>
	/// Sprite selection callback function.
	/// </summary>

	/*void SelectSprite (string spriteName)
	{
		SerializedObject l_serializedQuestion = new SerializedObject(_questionInfo);
		l_serializedQuestion.Update();
		_questionInfo.spriteName = spriteName;
		l_serializedQuestion.ApplyModifiedProperties();
//		NGUITools.SetDirty(l_serializedQuestion.targetObject);
		NGUISettings.selectedSprite = spriteName;

		Undo.RecordObject(_questionInfo, "Question Editor Modify");
		EditorUtility.SetDirty(_questionInfo);
		Repaint();
	}*/

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>

	/*public void PreviewSprite (Rect p_rect)
	{
		if (_questionInfo.atlas == null) return;

		UISpriteData l_spriteData = _questionInfo.atlas.GetSprite(_questionInfo.spriteName);

		if (l_spriteData == null) return;

		Material l_atlasMaterial = _questionInfo.atlas.spriteMaterial;
		Texture2D l_tex = l_atlasMaterial.mainTexture as Texture2D;

		if (l_tex == null) return;
		
		NGUIEditorTools.DrawSprite(l_tex, p_rect, l_spriteData, Color.white);
	}*/

	private void ShowMessages ()
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

		_questionInfo.answers = _answers.ToArray();
		EditorUtility.SetDirty(_questionInfo);
		Repaint();
	}

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