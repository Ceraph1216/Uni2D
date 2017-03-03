using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
	private Vector3 _spawnPoint;
	public Vector3 spawnPoint
	{
		get 
		{
			return _spawnPoint;
		}

		private set 
		{
			_spawnPoint = value;
		}
	}

	void Awake ()
	{
		spawnPoint = transform.position;
	}

	public void RemoveCheckpoint ()
	{
		gameObject.SetActive (false);
	}

	public void RestoreCheckpoint ()
	{
		gameObject.SetActive (true);
	}
}
