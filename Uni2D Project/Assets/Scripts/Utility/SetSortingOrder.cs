using UnityEngine;
using System.Collections;

public class SetSortingOrder : MonoBehaviour
{
	public string layerName = "TopLayer";
	public int sortingOrder = 0;
	private SpriteRenderer sprite;
	
	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		
		if (sprite)
		{
			sprite.sortingOrder = sortingOrder;
			sprite.sortingLayerName = layerName;
		}
	}
}