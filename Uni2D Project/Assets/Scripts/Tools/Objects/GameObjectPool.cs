using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// A general pool object for reusable game objects.
//
// It supports spawning and unspawning game objects that are
// instantiated from a common _prefab. Can be used pre_allocate
// objects to avoid c_alls to Instantiate during gameplay. Can
// also create objects on demand (which it does if no objects
// are _available in the pool).
public class GameObjectPool 
{
	// Delegates
	public delegate void Initialize(GameObject p_obj);
	public delegate void GenericFunction(GameObject p_obj);
	
	// The _prefab that the game objects will be instantiated from.
	private GameObject _prefab;
	
	// The list of _available game objects (initi_ally empty by default).
	private Stack _available;
	
	// The list of _all game objects created thus far (used for efficiently
	// unspawning _all of them at once, see UnspawnAll).
	private List<GameObject> _all;
	
	public string name;
	
	// the name of the prefab currently associated with the pool
	
	// An optional function that will be c_alled whenever a new object is instantiated.
	// The newly instantiated object is passed to it, which _allows users of the pool
	// to do custom initialization.
	//private Initialize initializationFunction; // Function
	
	// Indicates whether the pool's game objects should be activated/deactivated
	// recursively (i.e. the game object and _all its children) or non-recursively (just the
	// game object).
	//private bool setActiveRecursively;
	
	// Creates a pool.
	// The initialCapacity is used to initialize the .NET collections, and determines
	// how much space they pre-_allocate behind the scenes. It does not pre-populate the
	// collection with game objects. For that, see the PrePopulate function.
	// If an initialCapacity that is <= to zero is provided, the pool uses the default
	// initial capacities of its internal .NET collections.
	public GameObjectPool (GameObject p_prefab, int p_initialCapacity, string p_name = "GameObjectPool"/*, Initialize initializationFunction, bool setActiveRecursively*/)
	{
		name = p_name;
		this._prefab = p_prefab;
		if (p_initialCapacity > 0)
		{
			this._available = new Stack(p_initialCapacity);
			this._all = new List<GameObject>(p_initialCapacity);
			
			// Try initializing everything at the very beginning! Could be dangerous :X
			/*List<GameObject> l_objects = new List<GameObject>();
			
			for (int i = 0; i < p_initialCapacity; i++)
			{
				l_objects.Add(Spawn(new Vector3(0, 100, 0), Quaternion.identity));
			}
			
			for (int i = 0; i < p_initialCapacity; i++)
			{
				Unspawn(l_objects[i]);
			}*/

			//Debug.Log("Initializing " + p_prefab.name + "; capacity = " + p_initialCapacity);
		} 
		else 
		{
			// Use the .NET defaults
			this._available = new Stack();
			this._all = new List<GameObject>();
		}
		
		//this.initializationFunction = initializationFunction;
		//this.setActiveRecursively = setActiveRecursively;
	}
	
	// Spawn a game object with the specified position/rotation.
	public GameObject Spawn (Vector3 p_position = default(Vector3), 
	                         Quaternion p_rotation = default(Quaternion), 
	                         Vector3 p_scale = default(Vector3), 
	                         Transform p_parent = null)
	{
		// If no scale or a scale of zero is supplied to the function, it sets scale to 1.
		p_scale = p_scale == Vector3.zero ? Vector3.one : p_scale;
		
		GameObject l_result;
		
		if (_available.Count == 0)
		{
			// Create an object and initialize it.
			l_result = GameObject.Instantiate(_prefab, p_position, p_rotation) as GameObject;
			Transform l_resultTrans = l_result.transform;
			l_resultTrans.localScale = p_scale;
			
			if ( p_parent )
			{
				l_resultTrans.parent = p_parent;
				l_resultTrans.position = p_position;
				l_resultTrans.localScale = p_scale;
			}
			/*if(initializationFunction != null)
			{
				initializationFunction(l_result);
			}*/
			
			// Keep track of it.
			_all.Add(l_result);
		} 
		else 
		{
			l_result = _available.Pop() as GameObject;
			if (l_result)
			{
				
				// Get the l_result's transform and reuse for efficiency.
				Transform l_resultTrans = null;
				if (l_result)
				{
					l_resultTrans = l_result.transform;
					l_resultTrans.position = p_position;
					l_resultTrans.rotation = p_rotation;
					l_resultTrans.localScale = p_scale;
				}
				
				if ( p_parent && l_resultTrans )
				{
					l_resultTrans.parent = p_parent;
					l_resultTrans.position = p_position;
					l_resultTrans.localScale = p_scale;
				}
				
				if (l_result)
				{
					this.SetActive(l_result, true);
				}
			}
		}
		
		if (l_result)
		{
			l_result.SetActive(true);
		}
		return l_result;
	}
	
	// Unspawn the provided game object.
	// The function is idempotent. C_alling it more than once for the same game object is
	// safe, since it first checks to see if the provided object is already unspawned.
	// Returns true if the unspawn succeeded, false if the object was already unspawned.
	public bool Unspawn (GameObject p_obj)
	{
		if (!_available.Contains(p_obj))
		{ 
			// Make sure we don't insert it twice.
			_available.Push(p_obj);
			this.SetActive(p_obj, false);
			p_obj.SetActive(false);
			return true; // Object inserted back in stack.
		}
		return false; // Object already in stack.
	}
	
	// Pre-populates the pool with the provided number of game objects.
	public void PrePopulate (int p_count)
	{
		GameObject[] l_array  = new GameObject[p_count];
		for (int i = 0; i < p_count; i++)
		{
			l_array[i] = Spawn(Vector3.zero, Quaternion.identity);
			this.SetActive(l_array[i], false);
		}
		
		for (int j = 0; j < p_count; j++)
		{
			Unspawn(l_array[j]);
		}
	}
	
	// Unspawns _all the game objects created by the pool.
	public void UnspawnAll ()
	{
		for(var i = 0; i < _all.Count; i++)
		{
			GameObject l_obj  = _all[i] as GameObject;
			
			if (l_obj.activeInHierarchy)
			{
				Unspawn(l_obj);
			}
		}
	}
	
	// Unspawns _all the game objects and clears the pool.
	public void Clear ()
	{
		UnspawnAll();
		_available.Clear();
		_all.Clear();
	}
	
	// Returns the number of active objects.
	public int GetActiveCount ()
	{
		return _all.Count - _available.Count;
	}
	
	// Returns the number of _available objects.
	public int GetAvailableCount ()
	{
		return _available.Count;
	}
	
	// Returns the _prefab being used by this pool.
	public GameObject GetPrefab () 
	{
		return _prefab;
	}
	
	// Applies the provided function to some or _all of the pool's game objects.
	public void ForEach (GenericFunction p_func, bool p_activeOnly)
	{
		for (int i = 0; i < _all.Count; i++)
		{
			GameObject l_obj = _all[i] as GameObject;
			if(!p_activeOnly || l_obj.activeInHierarchy)
			{
				p_func(l_obj);
			}
		}
	}
	
	// Activates or deactivates the provided game object using the method
	// specified by the setActiveRecursively flag.
	public void SetActive (GameObject p_obj, bool p_val)
	{
		/* if(setActiveRecursively)
			obj.SetActiveRecursively(val);
		else*/
		p_obj.SetActive(p_val);
	}

	public void EmptyPool()
	{
		_all = new List<GameObject>(_all.Count);
		_available = new Stack(_available.Count);
	}

	public void DestroyPool()
	{
		ObjectDestroyer.instance.AddObjectsToDestroy(_all);

		EmptyPool();
	}
}