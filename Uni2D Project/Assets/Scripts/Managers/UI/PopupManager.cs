using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour 
{
	// Set up delegate for popup events
	public delegate void EventHandler(GameObject e);
	public event EventHandler ChangePopup;
	public event EventHandler ChangeSecondaryPopup;

	private Enums.PopupType _currentPopup = Enums.PopupType.None;
	public Enums.PopupType currentPopup
	{
		get
		{
			return _currentPopup;
		}

		set
		{
			// Make sure we flag the popup as changing when this is set so ShowPopupsBehavior knows when to show the new popup
			_currentPopup = value;
			if (ChangePopup != null)
			{
				ChangePopup(this.gameObject);
			}
		}
	}

	private Enums.SecondaryPopupType _currentSecondaryPopup = Enums.SecondaryPopupType.None;
	public Enums.SecondaryPopupType currentSecondaryPopup
	{
		get
		{
			return _currentSecondaryPopup;
		}

		set
		{
			// Make sure we flag the popup as changing when this is set so
			// ShowPopupsBehavior knows when to show the new popup
			_currentSecondaryPopup = value;
			if (ChangeSecondaryPopup != null)
			{
				ChangeSecondaryPopup(this.gameObject);
			}
		}
	}

	static PopupManager mInstance;

	// The instance of the PopupManager class.
	static public PopupManager instance
	{
		get
		{
			return mInstance;
		}
	}

	void Awake ()
	{
		mInstance = this;
	}

	public void ResetEvents()
	{
		ChangePopup = null;
		ChangeSecondaryPopup = null;
	}

	public void ShowMessage (string p_key)
	{
		currentPopup = Enums.PopupType.Message;

		PopupCollection.instance.messageController.ShowMessage (MessageManager.instance.messageDictionary [p_key]);
	}
}