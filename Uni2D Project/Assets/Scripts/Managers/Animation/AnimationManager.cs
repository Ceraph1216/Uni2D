using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager : MonoBehaviour 
{
	/*private static List<tk2dSpriteAnimator> sprites;
	private static List<ParticleSystem> emitters;*/
	
	public static void pause()
	{
		/*fillArrays();
		for(int i = 0; i < sprites.Count; i++)
		{
			sprites[i].Pause();
		}
		
		for(int i = 0; i < emitters.Count; i++)
		{
			emitters[i].Pause();
		}*/
	}
	
	public static void resume()
	{
		/*for(int i = 0; i < sprites.Count; i++)
		{
			sprites[i].Resume();
		}
		
		for(int i = 0; i < emitters.Count; i++)
		{
			emitters[i].Play();
		}*/
	}
	
	private static void fillArrays()
	{
		/*sprites = new List<tk2dSpriteAnimator>();
		emitters = new List<ParticleSystem>();
		
		GameObject[] allObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
		for(int i = 0; i < allObjects.Length; i++)
		{
			GameObject thisObject = allObjects[i];
			populateArraysForObject(thisObject);
		}*/
	}
	
	private static void populateArraysForObject(GameObject thisObject)
	{
		/*if (thisObject.activeInHierarchy)
		{
			// Check for a sprite
			tk2dSpriteAnimator sprite = thisObject.GetComponent<tk2dSpriteAnimator>();
			if(sprite != null)
			{
				sprites.Add(sprite);
			}
			
			// Check for an emitter
			ParticleSystem emitter = thisObject.GetComponent<ParticleSystem>();
			if(emitter != null)
			{
				emitters.Add(emitter);
			}
			
			// Operate on this object's children
			for(int c = 0; c < thisObject.transform.childCount; c++)
			{
				populateArraysForObject(thisObject.transform.GetChild(c).gameObject);
			}
		}*/
	}
}