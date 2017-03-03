using UnityEngine;
using System.Collections;

public class ColorObjectForTimeBehavior : MonoBehaviour 
{
	public Color _color;
	public float _timeToColor;
	
	public enum ObjectType
	{
		tk2d,
		NGUI,
		Mesh,
		SkinnedMesh,
		Sprite
	}
	public ObjectType _type;

	private Color _originalColor;
	private Color _originalRimColor;

	private float _currentTime;
	
	private Object _obj;
	private Object obj
	{
		get
		{
			if(_obj == null)
			{
				switch(_type)
				{
					case ObjectType.tk2d:
						//_obj = GetComponent<tk2dSprite>();
						break;
					case ObjectType.NGUI:
						_obj = GetComponent<UIWidget>();
						break;
					case ObjectType.Mesh:
						_obj = GetComponent<MeshRenderer>();
						break;
					case ObjectType.SkinnedMesh:
						_obj = GetComponent<SkinnedMeshRenderer>();
						break;
					case ObjectType.Sprite:
						_obj = GetComponent<SpriteRenderer>();
						break;
				}
			}

			return _obj;
		}
	}

	private Color objectColor
	{
		get
		{
			switch(_type)
			{
				case ObjectType.tk2d:
					//return (obj as tk2dSprite).color;
				case ObjectType.NGUI:
					return (obj as UIWidget).color;
				case ObjectType.Mesh:
					return (obj as MeshRenderer).material.color;
				case ObjectType.SkinnedMesh:
					return (obj as SkinnedMeshRenderer).material.color;
				case ObjectType.Sprite:
					return (obj as SpriteRenderer).color;
			}

			return Color.white;
		}

		set
		{
			switch(_type)
			{
				case ObjectType.tk2d:
					//(obj as tk2dSprite).color = value;
					break;
				case ObjectType.NGUI:
					(obj as UIWidget).color = value;
					break;
				case ObjectType.Mesh:
					MeshRenderer l_mesh = (obj as MeshRenderer);
					for(int i = 0; i < l_mesh.materials.Length; i++)
					{
						l_mesh.materials[i].color = value;
					}
					break;
				case ObjectType.SkinnedMesh:
					SkinnedMeshRenderer l_skinnedMesh = (_obj as SkinnedMeshRenderer);
					for(int i = 0; i < l_skinnedMesh.materials.Length; i++)
					{
						l_skinnedMesh.materials[i].color = value;
					}
					break;
				case ObjectType.Sprite:
					(obj as SpriteRenderer).color = value;
					break;
			}
		}
	}

	private Color rimColor
	{
		get
		{
			switch(_type)
			{
				case ObjectType.Mesh:
					MeshRenderer l_mesh = (_obj as MeshRenderer);
					for(int i = 0; i < l_mesh.materials.Length; i++)
					{
						if(l_mesh.materials[i].HasProperty("_RimColor"))
						{
							return l_mesh.materials[i].GetColor("_RimColor");
						}
					}
					break;
				case ObjectType.SkinnedMesh:
					SkinnedMeshRenderer l_skinnedMesh = (_obj as SkinnedMeshRenderer);
					for(int i = 0; i < l_skinnedMesh.materials.Length; i++)
					{
						if(l_skinnedMesh.materials[i].HasProperty("_RimColor"))
						{
							return l_skinnedMesh.materials[i].GetColor("_RimColor");
						}
					}
					break;
			}
			
			return Color.white;
		}
		
		set
		{
			switch(_type)
			{
				case ObjectType.Mesh:
					MeshRenderer l_mesh = (_obj as MeshRenderer);
					for(int i = 0; i < l_mesh.materials.Length; i++)
					{
						if(l_mesh.materials[i].HasProperty("_RimColor"))
						{
							l_mesh.materials[i].SetColor("_RimColor", value);
						}
					}
					break;
				case ObjectType.SkinnedMesh:
					SkinnedMeshRenderer l_skinnedMesh = (_obj as SkinnedMeshRenderer);
					for(int i = 0; i < l_skinnedMesh.materials.Length; i++)
					{
						if(l_skinnedMesh.materials[i].HasProperty("_RimColor"))
						{
							l_skinnedMesh.materials[i].SetColor("_RimColor", value);
						}
					}
					break;
			}
		}
	}
	
	void OnEnable()
	{
		_originalColor = objectColor;
		objectColor = _color;

		if(_type == ObjectType.Mesh)
		{
			_originalRimColor = rimColor;
			rimColor = _color;
		}

		_currentTime = 0;

		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.FinalSoftUpdate, SoftUpdate);
	}
	
	void OnDisable()
	{
		objectColor = _originalColor;
		if(_type == ObjectType.Mesh)
		{
			rimColor = _originalRimColor;
		}

		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.FinalSoftUpdate, SoftUpdate);
	}
	
	void SoftUpdate(GameObject dispatcher) 
	{
		_currentTime += TimeManager.deltaTime;
		if(_currentTime >= _timeToColor)
		{
			this.enabled = false;
		}
	}
}