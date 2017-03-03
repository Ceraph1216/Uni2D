using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SaveDataManager : MonoBehaviour 
{
	static SaveDataManager mInstance;
	
	// The instance of the SaveDataManager class
	static public SaveDataManager instance
	{
		get
		{
			return mInstance;
		}
	}

#if UNITY_EDITOR
	public bool eraseDataOnLoad;
#endif

	// Use this for initialization
	void Awake() 
	{
#if UNITY_EDITOR
		if(eraseDataOnLoad)
		{
			DeleteAll();
		}
#endif

		mInstance = this;
		if(!string.IsNullOrEmpty(ScreenManager.instance.previousLevelName))// && ScreenManager.instance.currentLevelIndex != Constants.instance.PRELOADER_SCREEN_INDEX)
		{
			LoadSaveData();
		}
	}
	
	public void LoadSaveData(bool musicDefault = true, bool soundDefault = true)
	{
		LoadOptions(musicDefault, soundDefault);
		LoadRecords();
		LoadUpgrades();
		LoadAchievements();
		//LoadStatistics();
	}

	private void LoadOptions(bool musicDefault = true, bool soundDefault = true)
	{
		/*OptionsManager.musicEnabled = GetBool(Constants.instance.MUSIC_ENABLED_KEY, musicDefault);
		OptionsManager.soundsEnabled = GetBool(Constants.instance.SOUNDS_ENABLED_KEY, soundDefault);
		OptionsManager.language = GetString(Constants.instance.LANGUAGE_KEY, OptionsManager.defaultLanguageForDevice);*/
	}

	private void LoadRecords()
	{
		// High scores
		/*int l_numLevels = LevelManager.instance.numLevels;
		for(int i = 1; i <= l_numLevels; i++)
		{
			ScoreManager.instance.SetTopScoreForLevel(GetInt(string.Format(Constants.instance.saveData.HIGH_SCORE_KEY, i.ToString()), 0), i);
		}*/
	}

	private void LoadUpgrades()
	{
		//UpgradeManager.instance.attackUpgrade = GetInt(Constants.instance.saveData.ATTACK_UPGRADE_KEY, 0);
		//UpgradeManager.instance.healthUpgrade = GetInt(Constants.instance.saveData.HEALTH_UPGRADE_KEY, 0);
		//UpgradeManager.instance.doubleCoinsUpgrade = GetBool(Constants.instance.saveData.DOUBLE_COINS_UPGRADE_KEY, false);
	}

	private void LoadAchievements()
	{
		//AchievementsManager.LoadAchievements();
	}

	/*private void LoadStatistics()
	{
		StatisticsManager.LoadStatistics();
	}*/

	//********************
	//      Getters
	//********************
	public int GetInt(string key, int defaultValue = -1, bool useEncryption = false)
	{
		// If the key is null or empty, return the default value
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to load an int with an empty key! defaultValue = " + defaultValue);
			return defaultValue;
		}

		// If we have the key, return its value; otherwise, return the default value
		if(!useEncryption)
		{
			if(PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetInt(key);
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
		else
		{
			if(SecurePlayerPrefs.HasKey(key))
			{
				int ret;
				if(int.TryParse(SecurePlayerPrefs.GetString(key, Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD), out ret))
				{
					return ret;
				}
				else
				{
					//Debug.LogWarning("WARNING! Data for key \"" + key + "\" was not an int! Returning default value: " + defaultValue);
					return defaultValue;
				}
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
	}
	
	public float GetFloat(string key, float defaultValue = -1, bool useEncryption = false)
	{
		// If the key is null or empty, return the default value
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to load a float with an empty key! defaultValue = " + defaultValue);
			return defaultValue;
		}
		
		// If we have the key, return its value; otherwise, return the default value
		if(!useEncryption)
		{
			if(PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetFloat(key);
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
		else
		{
			if(SecurePlayerPrefs.HasKey(key))
			{
				float ret;
				if(float.TryParse(SecurePlayerPrefs.GetString(key, Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD), out ret))
				{
					return ret;
				}
				else
				{
					//Debug.LogWarning("WARNING! Data for key \"" + key + "\" was not a float! Returning default value: " + defaultValue);
					return defaultValue;
				}
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
	}
	
	public string GetString(string key, string defaultValue = "null", bool useEncryption = false)
	{
		// If the key is null or empty, return the default value
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to load a string with an empty key! defaultValue = " + defaultValue);
			return defaultValue;
		}
		
		// If we have the key, return its value; otherwise, return the default value
		if(!useEncryption)
		{
			if(PlayerPrefs.HasKey(key))
			{
				return PlayerPrefs.GetString(key);
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
		else
		{
			if(SecurePlayerPrefs.HasKey(key))
			{
				return SecurePlayerPrefs.GetString(key, Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD);
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
	}
	
	public bool GetBool(string key, bool defaultValue = false, bool useEncryption = false)
	{
		// If the key is null or empty, return the default value
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to load a bool with an empty key! defaultValue = " + defaultValue);
			return defaultValue;
		}
		
		// If we have the key, return its value; otherwise, return the default value
		if(!useEncryption)
		{
			if(PlayerPrefs.HasKey(key))
			{
				return (PlayerPrefs.GetInt(key) == 1);
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
		else
		{
			if(SecurePlayerPrefs.HasKey(key))
			{
				int ret;
				if(int.TryParse(SecurePlayerPrefs.GetString(key, Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD), out ret))
				{
					return (ret == 1);
				}
				else
				{
					//Debug.LogWarning("WARNING! Data for key \"" + key + "\" was not a bool! Returning default value: " + defaultValue);
					return defaultValue;
				}
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
	}

	public T GetEnumType<T>(string key, T defaultValue, bool useEncryption = false) where T : struct, IComparable, IConvertible, IFormattable
	{
		// If T is not an enum, throw an exception
		if(!typeof(T).IsEnum)
		{
			throw new InvalidOperationException("type must be an Enum");
		}

		// If the key is null or empty, return the default value
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to load an enum with an empty key! defaultValue = " + defaultValue);
			return defaultValue;
		}
		
		// If we have the key, return its value; otherwise, return the default value
		if(!useEncryption)
		{
			if(PlayerPrefs.HasKey(key))
			{
				return (T) Enum.Parse(typeof(T), PlayerPrefs.GetString(key).Replace("Nasher", "Nashor"));
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
		else
		{
			if(SecurePlayerPrefs.HasKey(key))
			{
				return (T) Enum.Parse(typeof(T), 
				                      SecurePlayerPrefs.GetString(key, Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD).Replace("Nasher", "Nashor"));
			}
			else
			{
				//Debug.LogWarning("WARNING! Key \"" + key + "\" not found! Returning default value: " + defaultValue);
				return defaultValue;
			}
		}
	}

	//********************
	//      Setters
	//********************
	public void SetInt(string key, int data, bool saveAllData = false, bool useEncryption = false)
	{
		// If the key is null or empty, don't save anything and log an error
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to save an int with an empty key! data = " + data);
			return;
		}

		// Set the data, and save all data if told to
		if(!useEncryption)
		{
			PlayerPrefs.SetInt(key, data);
		}
		else
		{
			SecurePlayerPrefs.SetString(key, data.ToString(), Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD);
		}

		if(saveAllData)
		{
			Save();
		}
	}

	public void SetFloat(string key, float data, bool saveAllData = false, bool useEncryption = false)
	{
		// If the key is null or empty, don't save anything and log an error
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to save a float with an empty key! data = " + data);
			return;
		}
		
		// Set the data, and save all data if told to
		if(!useEncryption)
		{
			PlayerPrefs.SetFloat(key, data);
		}
		else
		{
			SecurePlayerPrefs.SetString(key, data.ToString(), Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD);
		}

		if(saveAllData)
		{
			Save();
		}
	}

	public void SetString(string key, string data, bool saveAllData = false, bool useEncryption = false)
	{
		// If the key is null or empty, don't save anything and log an error
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to save a string with an empty key! data = " + data);
			return;
		}
		
		// Set the data, and save all data if told to
		if(!useEncryption)
		{
			PlayerPrefs.SetString(key, data);
		}
		else
		{
			SecurePlayerPrefs.SetString(key, data, Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD);
		}

		if(saveAllData)
		{
			Save();
		}
	}

	public void SetBool(string key, bool data, bool saveAllData = false, bool useEncryption = false)
	{
		// If the key is null or empty, don't save anything and log an error
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to save a bool with an empty key! data = " + data);
			return;
		}
		
		// Set the data, and save all data if told to
		int boolData = (data ? 1 : 0);
		if(!useEncryption)
		{
			PlayerPrefs.SetInt(key, boolData);
		}
		else
		{
			SecurePlayerPrefs.SetString(key, boolData.ToString(), Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD);
		}

		if(saveAllData)
		{
			Save();
		}
	}

	public void SetEnumType<T>(string key, T data, bool saveAllData = false, bool useEncryption = false) where T : struct, IComparable, IConvertible, IFormattable
	{
		// If T is not an enum, throw an exception
		if(!typeof(T).IsEnum)
		{
			throw new InvalidOperationException("type must be an Enum");
		}

		// If the key is null or empty, don't save anything and log an error
		if (string.IsNullOrEmpty(key)) 
		{
			Debug.LogError("ERROR! Tried to save an enum with an empty key! data = " + data);
			return;
		}
		
		// Set the data, and save all data if told to
		string enumData = (string)(Convert.ChangeType(data, typeof(string)));
		if(!useEncryption)
		{
			PlayerPrefs.SetString(key, enumData);
		}
		else
		{
			SecurePlayerPrefs.SetString(key, enumData, Constants.instance.saveData.SAVE_DATA_ENCRYPTION_PASSWORD);
		}

		if(saveAllData)
		{
			Save();
		}
	}

	//********************
	//       Save
	//********************
	public void Save()
	{
		PlayerPrefs.Save();
		//Debug.LogWarning("Data saved!");
	}

	//********************
	//      Delete
	//********************
	public void DeleteKey(string key)
	{
		if(PlayerPrefs.HasKey(key))
		{
			PlayerPrefs.DeleteKey(key);
		}
		else
		{
			SecurePlayerPrefs.DeleteKey(key);
		}
	}

	public void DeleteAll()
	{
		PlayerPrefs.DeleteAll();
	}
}