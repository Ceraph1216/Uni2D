using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods 
{
	public static Transform FindChildRecursive(this Transform myTransform, string name)   
	{
		// Check if the current transform is the child we're looking for; if so, return it
		if(myTransform.name == name)
		{
			return myTransform;
		}
		
		// Search through children for the child we're looking for
		for (int i = 0; i < myTransform.childCount; i++)
		{
			// The recursive step; repeat the search one step deeper in the hierarchy
			Transform found = myTransform.GetChild(i).FindChildRecursive(name);
			
			// A transform was returned by the search above that is not null,
			// it must be the child we're looking for
			if(found != null)
			{
				return found;
			}
		}
		
		// Child with name was not found
		return null;
	}

	public static Transform FindUppermostParent(this Transform myTransform)
	{
		if(myTransform.parent == null)
		{
			return myTransform;
		}
		else
		{
			return myTransform.parent.FindUppermostParent();
		}
	}

	public static int Sum(this int[] array)
	{
		int sum = 0;
		for(int i = 0; i < array.Length; i++)
		{
			sum += array[i];
		}

		return sum;
	}

	public static int MaxIndex(this int[] array)
	{
		int max = 0;
		for(int i = 1; i < array.Length; i++)
		{
			if(array[i] > array[max])
			{
				max = i;
			}
		}
		
		return max;
	}
	
	public static int MaxElement(this int[] array)
	{
		return array[array.MaxIndex()];
	}

	/*public static List<T> ShuffleList<T>(List<T> list)
	{
		list.Sort
		(
			delegate(T x, T y)
			{
				return Random.Range(-1, 2);
			}
		);

		return list;
	}*/

	public static List<int> ShuffleIntList(List<int> list)
	{
		list.Sort
		(
			delegate(int x, int y)
			{
				return Random.Range(-1, 2);
			}
		);
		
		return list;
	}

	public static List<float> ShuffleFloatList(List<float> list)
	{
		list.Sort
		(
			delegate(float x, float y)
			{
				return Random.Range(-1, 2);
			}
		);
		
		return list;
	}

	public static List<string> ShuffleStringList(List<string> list)
	{
		list.Sort
		(
			delegate(string x, string y)
			{
				return Random.Range(-1, 2);
			}
		);
		
		return list;
	}

	/*
	public static void Shuffle<T>(this List<T> list)
	{
		list.Sort
		(
			delegate(T x, T y)
			{
				return Random.Range(-1, 2);
			}
		);
	}*/

	public static float Round(float value, int digits)
	{
		float mult = Mathf.Pow(10.0F, (float)digits);
		return Mathf.Round(value * mult) / mult;
	}

	public static Vector3 UniformVector(float l_value)
	{
		return new Vector3(l_value, l_value, l_value);
	}

	public static float Distance2D(Vector3 l_source, Vector3 l_target)
	{
		return Mathf.Sqrt(Mathf.Pow((l_target.x - l_source.x), 2) + Mathf.Pow((l_target.y - l_source.y), 2));
	}

	public static Transform ClosestObjectWithTagToPoint(string l_tag, Vector3 l_point)
	{
		GameObject[] l_objs = GameObject.FindGameObjectsWithTag(l_tag);
		if(l_objs.Length > 0)
		{
			Transform l_closest = null;
			for(int i = 0; i < l_objs.Length; i++)
			{
				if(l_objs[i].activeSelf)
				{
					Transform l_this = l_objs[i].transform;
					if(l_closest == null)
					{
						l_closest = l_this;
					}
					else
					{
						float l_closestDist = Distance2D(l_point, l_closest.position);
						float l_thisDist = Distance2D(l_point, l_this.position);

						if(Mathf.Abs(l_thisDist) < Mathf.Abs(l_closestDist))
						{
							l_closest = l_this;
						}
					}
				}
			}

			return l_closest;
		}

		return null;
	}

	public static Transform ClosestObjectWithTagsToPoint(string[] l_tags, Vector3 l_point)
	{
		List<Transform> l_closestObjects = new List<Transform>();
		for(int i = 0; i < l_tags.Length; i++)
		{
			Transform l_transform = ClosestObjectWithTagToPoint(l_tags[i], l_point);
			if(l_transform != null)
			{
				l_closestObjects.Add(l_transform);
			}
		}
		
		if(l_closestObjects.Count >= 1)
		{
			Transform l_closest = null;
			for(int i = 0; i < l_closestObjects.Count; i++)
			{
				Transform l_this = l_closestObjects[i].transform;
				
				if(l_closest == null)
				{
					l_closest = l_this;
				}
				else
				{
					float l_closestDist = Distance2D(l_point, l_closest.position);
					float l_thisDist = Distance2D(l_point, l_this.position);
					
					if(Mathf.Abs(l_thisDist) < Mathf.Abs(l_closestDist))
					{
						l_closest = l_this;
					}
				}
			}
			
			return l_closest;
		}
		
		return null;
	}

	/*public static Vector3 MousePointToGunPoint(Vector3 l_position, Transform l_gunTransform = null)
	{
		Vector3 l_ret = l_position;
		l_ret.z = -CameraManager.instance.tapCamera.transform.position.z;
		l_ret = CameraManager.instance.tapCamera.ScreenToWorldPoint(l_ret);

		if(l_gunTransform != null)
		{
			l_ret.y = Mathf.Max(l_gunTransform.position.y, l_ret.y);
			l_ret.z = l_gunTransform.position.z;
		}

		return l_ret;
	}*/

	public static Vector3 TriggerImpactPoint(Transform l_source, Transform l_collider, LayerMask l_layer, bool l_backward = false)
	{
		RaycastHit hit;
		if (Physics.Raycast(l_source.position, l_source.forward * ((l_backward) ? -1 : 1), out hit, 1000000F, l_layer))
		{
			return hit.point;
		}

		return l_collider.position;
	}

	public static float AngleForObjectTowardsPoint(Transform l_object, Vector3 l_point)
	{
		return AngleBetweenPoints(l_object.position, l_point);
	}

	public static float AngleBetweenPoints(Vector3 l_point1, Vector3 l_point2)
	{
		float l_angle = (Mathf.Rad2Deg * Mathf.Atan2(l_point2.y - l_point1.y, l_point2.x - l_point1.x)) - 90;
		
		// Make sure the angle is in the 0-360 range
		while(l_angle >= 180)
		{
			l_angle -= 360;
		}
		
		while(l_angle < 0)
		{
			l_angle += 360;
		}
		
		return l_angle;
	}

	public static void TurnObjectTowardsPoint(ref Transform l_object, Vector3 l_point)
	{
		l_object.localRotation = Quaternion.Euler(0, 0, AngleForObjectTowardsPoint(l_object, l_point));
	}

	public static void SetLayerRecursive(GameObject l_object, int l_layer)
	{
		l_object.layer = l_layer;
		Transform l_transform = l_object.transform;
		for(int i = 0; i < l_transform.childCount; i++)
		{
			SetLayerRecursive(l_transform.GetChild(i).gameObject, l_layer);
		}
	}
}