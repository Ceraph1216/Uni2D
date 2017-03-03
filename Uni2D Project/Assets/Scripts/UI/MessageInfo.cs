using UnityEngine;
using System.Collections;

[System.Serializable]
public class Message
{
	/// <summary>
	/// Whether or not the message needs to be removed from the list
	/// </summary>
	public bool dirty;
	public string text;
}

[System.Serializable]
public class MessageInfo : ScriptableObject
{
	public string messageName;
	public string key;
	public string finishedMessage;
	public bool isMobileSpecific;
	public Message[] messages;
}