using UnityEngine;
using System.Collections;

public class PopupCollection : MonoBehaviour 
{
	static PopupCollection mInstance;

	// The instance of the PopupCollection class.
	static public PopupCollection instance
	{
		get
		{
			return mInstance;
		}
	}

	// Primary
	public GameObject pausePopup;
	public GameObject messagePopup;

	// Secondary

	// Cached components
	private MessageController _messageController;
	public MessageController messageController
	{
		get 
		{
			return _messageController;
		}
		private set 
		{
			_messageController = value;
		}
	}

	void Awake()
	{
		mInstance = this;
		messageController = messagePopup.GetComponent<MessageController> ();
	}

	public GameObject[] getAllPopups()
	{
		// Returns an array of all primary popups
		GameObject[] popups = {pausePopup, messagePopup};
		return popups;
	}
	
	public GameObject[] getAllSecondaryPopups()
	{
		// Returns an array of all secondary popups
		GameObject[] secondaryPopups = {};
		return secondaryPopups;
	}
}
