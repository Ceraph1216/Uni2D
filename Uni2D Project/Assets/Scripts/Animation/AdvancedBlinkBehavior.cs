using UnityEngine;
using System.Collections;

public class AdvancedBlinkBehavior : MonoBehaviour 
{
	public bool isUI;
	public float timeToBlinkFor;
	public FluctuatingNumber timeBetweenBlinks;

	public bool startVisible;
	public bool endVisible;

	private MeshRenderer[] meshes;
	private SkinnedMeshRenderer[] skinnedMeshes;
	private UIWidget[] widgets;

	private float currentTime;
	private float currentBlinkTime;
	
	public delegate void BlinkEventHandler();
	public event BlinkEventHandler OnBlinkFinished;

	public bool visible
	{
		get
		{
			if(isUI)
			{
				if(widgets == null || widgets.Length <= 0 || widgets[0] == null || !widgets[0].gameObject.activeSelf)
				{
					GetVisuals();
				}

				return widgets[0].enabled;
			}
			else
			{
				if(meshes == null || meshes.Length <= 0 || meshes[0] == null || !meshes[0].gameObject.activeSelf)
				{
					GetVisuals();
				}
				
				return meshes[0].enabled;
			}
		}

		set
		{
			if(isUI)
			{
				if(widgets == null || widgets.Length <= 0 || widgets[0] == null || !widgets[0].gameObject.activeSelf)
				{
					GetVisuals();
				}
				
				for(int i = 0; i < widgets.Length; i++)
				{
					if(widgets[i] != null)
					{
						widgets[i].enabled = value;
					}
				}
			}
			else
			{
				if(meshes == null || meshes.Length <= 0 || meshes[0] == null || !meshes[0].gameObject.activeSelf)
				{
					GetVisuals();
				}

				for(int i = 0; i < meshes.Length; i++)
				{
					if(meshes[i] != null)
					{
						meshes[i].enabled = value;
					}
				}

				for(int i = 0; i < skinnedMeshes.Length; i++)
				{
					if(skinnedMeshes[i] != null)
					{
						skinnedMeshes[i].enabled = value;
					}
				}
			}
		}
	}

	void OnEnable()
	{
		currentTime = 0;
		visible = startVisible;

		ClearVisuals();
		GetVisuals();

		timeBetweenBlinks._timeToArriveAtEndValue = timeToBlinkFor;
		timeBetweenBlinks.Reset();

		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);
	}
	
	void OnDisable()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.EarlySoftUpdate, SoftUpdate);
		visible = endVisible;
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

			if(currentBlinkTime >= timeBetweenBlinks._value)
			{
				currentBlinkTime = 0;
				visible = !visible;
			}
		}
	}
	
	void GetVisuals()
	{
		if(isUI)
		{
			widgets = GetComponentsInChildren<UIWidget>();
		}
		else
		{
			meshes = GetComponentsInChildren<MeshRenderer>();
			skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
		}
	}

	public void ClearVisuals()
	{
		meshes = null;
		skinnedMeshes = null;
		widgets = null;
	}
}