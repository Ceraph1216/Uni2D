using UnityEngine;
using UnityEngine.UI;
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
	private Image[] images;
	private SpriteRenderer[] sprites;

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
				if(images == null || images.Length <= 0 || images[0] == null || !images[0].gameObject.activeSelf)
				{
					GetVisuals();
				}

				return images[0].enabled;
			}
			else
			{
				if(sprites == null || sprites.Length <= 0 || sprites[0] == null || !sprites[0].gameObject.activeSelf)
				{
					GetVisuals();
				}
				
				return sprites[0].enabled;
			}
		}

		set
		{
			if(isUI)
			{
				if(images == null || images.Length <= 0 || images[0] == null || !images[0].gameObject.activeSelf)
				{
					GetVisuals();
				}
				
				for(int i = 0; i < images.Length; i++)
				{
					if(images[i] != null)
					{
						images[i].enabled = value;
					}
				}
			}
			else
			{
				if(sprites == null || sprites.Length <= 0 || sprites[0] == null || !sprites[0].gameObject.activeSelf)
				{
					GetVisuals();
				}

				for(int i = 0; i < sprites.Length; i++)
				{
					if(sprites[i] != null)
					{
						sprites[i].enabled = value;
					}
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
			images = GetComponentsInChildren<Image>();
		}
		else
		{
			meshes = GetComponentsInChildren<MeshRenderer>();
			skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
			sprites = GetComponentsInChildren<SpriteRenderer>();
		}
	}

	public void ClearVisuals()
	{
		meshes = null;
		skinnedMeshes = null;
		images = null;
		sprites = null;
	}
}