using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MessageController : MonoBehaviour 
{
	public UITweener tweenIn;
	public UITweener tweenOut;
	public Text messageLabel;

	private List<string> _messagesHolder;
	private List<string> _messages
	{
		get 
		{
			if (_messagesHolder == null) 
			{
				_messagesHolder = new List<string> ();
			}

			return _messagesHolder;
		}

		set 
		{
			_messagesHolder = value;
		}
	}

	private int _messageIndex;
	private MessageInfo _currentMessageInfo;
	private string _currentMessageSound;

	void Awake ()
	{
		_messages = new List<string> ();
	}

	public void ShowMessage (MessageInfo p_message)
	{
		_currentMessageInfo = p_message;
		for (int i = 0; i < p_message.messages.Length; i++) 
		{
			_messages.Add(p_message.messages[i].text);
		}

		_messageIndex = 0;

		tweenIn.ResetToBeginning ();
		tweenIn.enabled = true;
		tweenIn.PlayForward ();
	}

	public void TweenInFinished ()
	{
		string l_baseSound = _currentMessageInfo.key + "_" + _messageIndex;
		if (_currentMessageInfo.isMobileSpecific) 
		{
			l_baseSound = l_baseSound + "_mobile";
		}
		_currentMessageSound = l_baseSound + ".mp3";
		messageLabel.text = _messages [_messageIndex];
	}

	public void OnClickNext ()
	{
		_messageIndex++;

		if (_messageIndex >= _messages.Count) 
		{
			CloseMessage ();
			return;
		}

		string l_baseSound = _currentMessageInfo.key + "_" + _messageIndex;
		if (_currentMessageInfo.isMobileSpecific) 
		{
			l_baseSound = l_baseSound + "_mobile";
		}
		_currentMessageSound = l_baseSound + ".mp3";

		messageLabel.text = _messages [_messageIndex];
	}

	public void CloseMessage ()
	{
		messageLabel.text = "";

		tweenOut.ResetToBeginning ();
		tweenOut.enabled = true;
		tweenOut.PlayForward ();
	}

	public void TweenOutFinished ()
	{
		_messages = new List<string> ();
		GlobalMessageReceiver.instance.SendMessage(_currentMessageInfo.finishedMessage);
		PopupManager.instance.currentPopup = Enums.PopupType.None;
	}
}
