using UnityEngine;
using System.Collections;

public class ShowPopupsBehavior : MonoBehaviour 
{
	/// <summary>
	/// 0 = off / 1 = low / 5 = mid / 9 = high	
	/// </summary>
	private int debugLevel = 0;
	
	private GameObject currentPrimaryPopup;
	private GameObject previousPrimaryPopup;
	
	private GameObject currentSecondaryPopup;
	private GameObject previousSecondaryPopup;
	
	// Use this for initialization
	void Awake()
	{
		Initialize();
	}
	
	public void Initialize() 
	{
		// Set up event listeners for popup changes
		PopupManager.instance.ChangePopup += ChangePopup;
		PopupManager.instance.ChangeSecondaryPopup += ChangeSecondaryPopup;
	}
	
	void ChangePopup (GameObject dispatcher)
	{
		// Check which popup type was just changed to and set the current popup to that 
		switch(PopupManager.instance.currentPopup)
		{
			case Enums.PopupType.Pause:
				currentPrimaryPopup = PopupCollection.instance.pausePopup;
				break;
			case Enums.PopupType.Message:
				currentPrimaryPopup = PopupCollection.instance.messagePopup;
				break;
		}
		
		// Make sure the previous popup is hidden before showing the new one
		if (PopupManager.instance.currentPopup != Enums.PopupType.None) 
		{
			// This often attempts to hide pop-ups already hidden creating extra debug output from below
			HidePreviousPopup();
			ShowCurrentPopup();
		}
		// If the new popup type is none just hide all popups
		else
		{
			HideAllPopups();

			/*AudioSource currentMusic = SoundManager.GetCurrentAudioSource();
			if(currentMusic != null && currentMusic.volume > 0)
			{
				BPRSoundManager.instance.PlayCurrentMusic();
			}*/

			//BPRSoundManager.instance.ResumeAudio();
		}
	}
	
	void ChangeSecondaryPopup (GameObject dispatcher)
	{
		// Check which secondary popup should be shown
		/*switch(PopupManager.instance.currentSecondaryPopup)
		{
			case Enums.SecondaryPopupType.Dialogue:
				currentSecondaryPopup = PopupCollection.instance.dialoguePopup;
				break;
			case Enums.SecondaryPopupType.Message:
				currentSecondaryPopup = PopupCollection.instance.messagePopup;
				break;
		}*/
		
		// Hide any other secondary popups before showing the new one
		HidePreviousSecondaryPopup();
		ShowCurrentSecondaryPopup();

		if(PopupManager.instance.currentSecondaryPopup != Enums.SecondaryPopupType.None)
		{
			// This often attempts to hide pop-ups already hidden creating extra debug output from below
			HidePreviousSecondaryPopup();
			ShowCurrentSecondaryPopup();
		}
		// If the new secondary popup type is none just hide all secondary popups
		else
		{
			HideAllSecondaryPopups();
		}
	}
	
	void HidePreviousPopup()
	{
		if (debugLevel >= 5) {Debug.Log("hiding: " + previousPrimaryPopup);}
		
		// Get a list of all the children and disable them. Leave the parent active so the game can find it.
		if (previousPrimaryPopup == null) 
		{
			return;
		}
		previousPrimaryPopup.SetActive(false);
	}
	
	void ShowCurrentPopup()
	{
		if (debugLevel >= 5) {Debug.Log("showing: " + currentPrimaryPopup);}
		
		// Get a list of all the children and make sure they are enabled
		currentPrimaryPopup.SetActive(true);
		
		// We're done with the currentPrimaryPopup so flag it as the previous popup
		previousPrimaryPopup = currentPrimaryPopup;
	}
	
	void HidePreviousSecondaryPopup()
	{
		if (debugLevel >= 5) 
		{
			Debug.Log("hiding: " + previousSecondaryPopup);
		}
		
		// Get a list of all the children and disable them. Leave the parent active so the game can find it.
		previousSecondaryPopup.SetActive(false);
	}
	
	void ShowCurrentSecondaryPopup()
	{
		if (debugLevel >= 5) 
		{
			Debug.Log("showing: " + currentSecondaryPopup);
		}
		
		// Get a list of all the children and make sure they are enabled
		currentSecondaryPopup.SetActive(true);
		
		// We're done with the currentPrimaryPopup so flag it as the previous popup
		previousSecondaryPopup = currentSecondaryPopup;
	}
	
	void HideAllPopups()
	{
		if (debugLevel >= 5) {Debug.Log("hiding all popups");}
		
		// Get the comprehensive list of primary popups from PopupCollection and make sure they're all disabled
		GameObject[] _popups = PopupCollection.instance.getAllPopups();
		
		for (int p = 0; p <_popups.Length; p++)
		{
			GameObject _popup = _popups[p];
			_popup.SetActive (false);
		}
	}
	
	void HideAllSecondaryPopups()
	{
		if (debugLevel >= 5) {Debug.Log("hiding all secondary popups");}
		
		// Get the comprehensive list of secondary popups from PopupCollection and make sure they're all disabled
		GameObject[] _popups = PopupCollection.instance.getAllSecondaryPopups();
		
		for (int p = 0; p <_popups.Length; p++)
		{
			GameObject _popup = _popups[p];
			_popup.SetActive(false);
		}
	}
	
	// Home spun SetActive that makes sure everything is hand activated
	/*public void mySetActiveRecursively(GameObject rootObject, bool active) 
	{
		if(rootObject != null)
		{
			// Hacky solution to 
			if(rootObject.name != "shop alert")
			{
				//rootObject.SetActive(active);
			}
			 
			for(int i = 0; i < rootObject.transform.childCount; i++)
			{
				Transform child = rootObject.transform.GetChild(i);
				child.gameObject.SetActive(active);
				//mySetActiveRecursively(child.gameObject, active);
			}
		}
	}*/
}
