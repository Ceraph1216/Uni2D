using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MessageEditorWindow : EditorWindow
{
	public static MessageEditorWindow messageEditorWindow;
	public static MessageInfo sentMessageInfo;
	private MessageInfo _messageInfo;

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

	private List<Message> _messages;

	[MenuItem("Window/Data Type Editors/Message Editor")]
	public static void Init()
	{
		messageEditorWindow = EditorWindow.GetWindow<MessageEditorWindow>(false, "Message", true);
		messageEditorWindow.Show();
		messageEditorWindow.Populate();
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
		
		
		if (sentMessageInfo != null){
			EditorGUIUtility.PingObject( sentMessageInfo );
			Selection.activeObject = sentMessageInfo;
			sentMessageInfo = null;
		}
		
		Object[] selection = Selection.GetFiltered(typeof(MessageInfo), SelectionMode.Assets);
		if (selection.Length > 0){
			if (selection[0] == null) return;
			_messageInfo = (MessageInfo) selection[0];
		}

		if (_messageInfo.messages != null)
		{
			_messages = _messageInfo.messages.ToList();
		}
		else
		{
			_messages = new List<Message>();
		}

	}

	public void OnGUI()
	{
		if (_messageInfo == null)
		{
			GUILayout.BeginHorizontal("GroupBox");
			GUILayout.Label("Select a message file or create a new message.","CN EntryInfo");
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("Create new message"))
				ScriptableObjectUtility.CreateAsset<MessageInfo> ();
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
			SerializedObject l_serializedMessage = new SerializedObject(_messageInfo);

			// Draw the message data

			EditorGUILayout.BeginVertical();
			{
				EditorGUIUtility.labelWidth = 90;
				_messageInfo.messageName = EditorGUILayout.TextField("Name:", _messageInfo.messageName);
				_messageInfo.key = EditorGUILayout.TextField("Key:", _messageInfo.key);
				_messageInfo.finishedMessage = EditorGUILayout.TextField("Send Message on Finished:", _messageInfo.finishedMessage);
				_messageInfo.isMobileSpecific = EditorGUILayout.Toggle ("Mobile Specific:", _messageInfo.isMobileSpecific);
				ShowMessages();
			}EditorGUILayout.EndVertical();

		EditorGUIUtility.labelWidth = 150;
		EditorGUILayout.Space();

		}EditorGUILayout.EndScrollView();

		if (GUI.changed) 
		{
			Undo.RecordObject(_messageInfo, "Message Editor Modify");
			EditorUtility.SetDirty(_messageInfo);
		}
	}

	/// <summary>
	/// Atlas selection callback.
	/// </summary>
	
	/*void OnSelectAtlas (Object obj)
	{
		SerializedObject l_serializedMessage = new SerializedObject(_messageInfo);
		l_serializedMessage.Update();
		SerializedProperty sp = l_serializedMessage.FindProperty("atlas");
		sp.objectReferenceValue = obj;
		l_serializedMessage.ApplyModifiedProperties();
		NGUITools.SetDirty(l_serializedMessage.targetObject);
		NGUISettings.atlas = obj as UIAtlas;

		Undo.RecordObject(_messageInfo, "Message Editor Modify");
		EditorUtility.SetDirty(_messageInfo);
		Repaint();
	}*/
	
	/// <summary>
	/// Sprite selection callback function.
	/// </summary>
	
	/*void SelectSprite (string spriteName)
	{
		SerializedObject l_serializedMessage = new SerializedObject(_messageInfo);
		l_serializedMessage.Update();
		_messageInfo.spriteName = spriteName;
		l_serializedMessage.ApplyModifiedProperties();
//		NGUITools.SetDirty(l_serializedMessage.targetObject);
		NGUISettings.selectedSprite = spriteName;

		Undo.RecordObject(_messageInfo, "Message Editor Modify");
		EditorUtility.SetDirty(_messageInfo);
		Repaint();
	}*/

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>
	
	/*public void PreviewSprite (Rect p_rect)
	{
		if (_messageInfo.atlas == null) return;

		UISpriteData l_spriteData = _messageInfo.atlas.GetSprite(_messageInfo.spriteName);

		if (l_spriteData == null) return;

		Material l_atlasMaterial = _messageInfo.atlas.spriteMaterial;
		Texture2D l_tex = l_atlasMaterial.mainTexture as Texture2D;

		if (l_tex == null) return;
		
		NGUIEditorTools.DrawSprite(l_tex, p_rect, l_spriteData, Color.white);
	}*/

	private void ShowMessages ()
	{
		GUIStyle l_darkStyle = new GUIStyle();
		l_darkStyle.normal.background = MakeTex(600, 1, new Color(0.5f, 0.5f, 0.5f));
		l_darkStyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);

		for (int i = 0; i < _messages.Count; i++)
		{
			Message l_currentMessage = _messages[i];

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
					Rect rect = GUILayoutUtility.GetRect(50, 70);
					EditorStyles.textField.wordWrap = true;
					l_currentMessage.text = EditorGUI.TextArea(rect, l_currentMessage.text);

					if (GUILayout.Button("-", GUILayout.Width(20f)))
					{
						l_currentMessage.dirty = true;
					}
				}EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical();
		}

		for (int i = 0; i < _messages.Count; i++)
		{
			Message l_currentMessage = _messages[i];

			if (l_currentMessage.dirty)
			{
				_messages.Remove(l_currentMessage);
			}
		}

		if (GUILayout.Button("Add Message", GUILayout.Width(100f)))
		{
			Message l_newMessage = new Message();
			_messages.Add(l_newMessage);
		}

		_messageInfo.messages = _messages.ToArray();
		EditorUtility.SetDirty(_messageInfo);
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