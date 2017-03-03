using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour 
{
	public static MessageManager instance;

	public Dictionary<string, MessageInfo> messageDictionary;
	public IncludedMessages includedMessages;
	public IncludedMessages mobileMessages;

	void Awake ()
	{
		instance = this;
		DontDestroyOnLoad (gameObject);
		messageDictionary = new Dictionary<string, MessageInfo> ();
		LoadMessages ();
	}

	private void LoadMessages ()
	{
		IncludedMessages l_usedMessages = includedMessages;

		if (PlatformManager.instance.isMobile) 
		{
			l_usedMessages = mobileMessages;
		}

		for (int i = 0; i < l_usedMessages.includedMessages.Length; i++) 
		{
			MessageInfo l_currentMessage = l_usedMessages.includedMessages [i];
			messageDictionary.Add (l_currentMessage.key, l_currentMessage);
		}
	}
}
