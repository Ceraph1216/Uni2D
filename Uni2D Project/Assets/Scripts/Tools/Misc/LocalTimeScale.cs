using UnityEngine;
using System.Collections;

public class LocalTimeScale : MonoBehaviour 
{
	public float factor;

	void Awake()
	{
		Init();
	}

	void OnEnable()
	{
		Init();
	}

	void Init()
	{
		factor = 0;
	}
}