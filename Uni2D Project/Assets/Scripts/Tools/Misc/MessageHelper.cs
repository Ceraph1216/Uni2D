using UnityEngine;
using System.Collections;

public class MessageHelper : MonoBehaviour 
{
	/// <summary>
	/// Broadcasts the supplied message to all GameObjects in the scene. NEVER use this in an update loop.
	/// </summary>
	/// <param name="p_message">Message to send.</param>
	public static void SendMessageToAll(string p_message)
	{
		GameObject[] gos = (GameObject[])GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (GameObject go in gos) 
		{
			if (go && go.transform.parent == null) 
			{
				go.gameObject.SendMessage (p_message);
			}
		}
	}
}
