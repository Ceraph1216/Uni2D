using UnityEngine;
using System.Collections;

public class SetRendererLayerScript : MonoBehaviour 
{
	public string sortingLayerName;
	public int sortingOrder;
	private Renderer m_Renderer;
	
	void Awake()
	{
		// cache reference for performance.
		m_Renderer = GetComponent<Renderer>();
	}
	
	void Start () 
	{
		SetLayerInfo ( sortingLayerName, sortingOrder );
	}
	
	public void SetLayerInfo ( string l_NewLayerName, int l_NewOrder ) 
	{
		sortingLayerName = l_NewLayerName;
		sortingOrder = l_NewOrder;
		m_Renderer.sortingLayerName = l_NewLayerName;
		m_Renderer.sortingOrder = l_NewOrder;
	}
}