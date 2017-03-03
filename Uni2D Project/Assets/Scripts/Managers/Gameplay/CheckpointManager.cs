using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour 
{
	public static CheckpointManager instance;

	public Checkpoint startingCheckpoint;
	public Checkpoint currentCheckpoint;

	private Checkpoint[] _checkpoints;

	void Awake ()
	{
		instance = this;

		_checkpoints = GameObject.FindObjectsOfType<Checkpoint> ();
	}

	public void GetCheckpoint (Checkpoint p_checkpoint)
	{
		currentCheckpoint = p_checkpoint;
		p_checkpoint.RemoveCheckpoint ();
	}

	public void ResetCheckpoints ()
	{
		for (int i = 0; i < _checkpoints.Length; i++) 
		{
			_checkpoints [i].RestoreCheckpoint ();
		}
		currentCheckpoint = startingCheckpoint;
	}
}
