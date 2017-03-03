using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PrefabManager : MonoBehaviour 
{
	public float timeToCheckClearingPrefabs;
	private float startTimeToCheck;

	private int numPoolsToCheck;
	private int lastPoolChecked;

	private bool isClearingPools;
	private bool hasFinishedClearingAPool;

	private Dictionary<string, GameObjectPool> objectPoolDictionary;
	
	static PrefabManager mInstance;

	// The instance of the PrefabManager class
	static public PrefabManager instance
	{
		get
		{
			return mInstance;
		}
	}

	// Use this for initialization
	void Awake() 
	{
		mInstance = this;
		//if(ScreenManager.instance.previousLevelIndex < 0 && ScreenManager.instance.currentLevelIndex != Constants.instance.match3.PRELOADER_SCREEN_INDEX)
		{
			Initialize();
		}
	}
	
	void LevelLoaded(UnityEngine.SceneManagement.Scene p_scene, UnityEngine.SceneManagement.LoadSceneMode p_sceneMode)
	{
		//if(ScreenManager.instance.previousLevelIndex >= 0)
		{
			ClearAllPools();
			InitializePools();
		}
	}
	
	public void Initialize()
	{
		InitializePools();
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += LevelLoaded; 
	}
	
	void InitializePools()
	{
		objectPoolDictionary = new Dictionary<string, GameObjectPool>();
	}

	public GameObjectPool ObjectPool(string name)
	{
		string l_actualName = name.Replace("(Clone)", "");
		if(!objectPoolDictionary.ContainsKey(l_actualName))
		{
			string[][] assetNames = AssetBundleManifest.assetNames;
			string[] bundleNames = AssetBundleManifest.bundleNames;

			string folder = "";

			for(int i = 0; i < assetNames.Length; i++)
			{
				string[] names = assetNames[i];
				for(int j = 0; j < names.Length; j++)
				{
					if(names[j] == l_actualName)
					{
						folder = bundleNames[i];

						continue;
					}
				}
			}
			
			GameObject prefab;

#if UNITY_EDITOR
			// Remove the "Resources" folder from the path when using asset bundles
			prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/Prefabs/GameObjects/" + folder + "/" + l_actualName + ".prefab");
#else
			prefab = Resources.Load<GameObject>("Prefabs/GameObjects/" + folder + "/" + l_actualName);
#endif

			if(prefab != null)
			{
				objectPoolDictionary.Add(l_actualName, new GameObjectPool(prefab, 1));
			}
			else
			{
				Debug.Log("WARNING!! GAME IS ABOUT TO CRASH BECAUSE IT COULDN'T LOAD PREFAB \"" + l_actualName + "\" FROM FOLDER \"" + folder + "\"");
			}
		}
		
		return objectPoolDictionary[l_actualName];
	}

	public void ClearAllPools()
	{
		if(objectPoolDictionary != null)
		{
			foreach(KeyValuePair<string, GameObjectPool> pool in objectPoolDictionary)
			{
				(pool.Value as GameObjectPool).EmptyPool();
			}
		}
	}

	public void ClearPoolsForBundle(string bundleName)
	{
		string[][] assetNames = AssetBundleManifest.assetNames;
		string[] bundleNames = AssetBundleManifest.bundleNames;

		int index = -1;
		for(int i = 0; i < assetNames.Length; i++)
		{
			if(bundleNames[i] == bundleName)
			{
				index = i;
				break;
			}
		}

		if(index >= 0)
		{
			string[] prefabs = assetNames[index];

			for(int i = 0; i < prefabs.Length; i++)
			{
				ObjectPool(prefabs[i]).DestroyPool();
			}
		}
		else
		{
			Debug.LogError("Name \"" + bundleName + "\" not found!");
			Debug.Break();
		}
	}
}