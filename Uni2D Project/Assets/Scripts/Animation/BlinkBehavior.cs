using UnityEngine;
using System.Collections;

public class BlinkBehavior : MonoBehaviour 
{
	public bool isUI;
	public float timeToBlinkFor;
	public float timeBetweenBlinks;

	public bool startVisible;
	public bool endVisible;

	private MeshRenderer mesh;
	private UIWidget widget;

	private float currentTime;
	private float currentBlinkTime;
	
	public delegate void BlinkEventHandler();
	public event BlinkEventHandler OnBlinkFinished;

	// Use this for initialization
	void Awake() 
	{
		if(isUI)
		{
			widget = GetComponent<UIWidget>();
		}
		else
		{
			mesh = GetComponent<MeshRenderer>();
		}
	}
	
	void OnEnable()
	{
		currentTime = 0;

		if(isUI)
		{
			widget.enabled = startVisible;
		}
		else
		{
			mesh.enabled = startVisible;
		}

		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);

		if(isUI)
		{
			if(widget != null)
			{
				widget.enabled = endVisible;
			}
		}
		else
		{
			if(mesh != null)
			{
				mesh.enabled = endVisible;
			}
		}
	}
	
	// Update is called once per frame
	void SoftUpdate(GameObject dispatcher) 
	{
		currentTime += TimeManager.deltaTime;

		if(currentTime >= timeToBlinkFor)
		{
			if(OnBlinkFinished != null)
			{
				OnBlinkFinished();
			}

			this.enabled = false;
		}
		else
		{
			currentBlinkTime += TimeManager.deltaTime;

			if(currentBlinkTime >= timeBetweenBlinks)
			{
				currentBlinkTime = 0;
				if(isUI)
				{
					widget.enabled = !widget.enabled;
				}
				else
				{
					mesh.enabled = !mesh.enabled;
				}
			}
		}
	}
}