using UnityEngine;
using System.Collections;

public class BouncingObject : MonoBehaviour 
{
	public float speed;

	private Transform _transform;

	private float _xSpeed;
	private float _ySpeed;

	void Awake ()
	{
		_transform = transform;

		_xSpeed = _ySpeed = speed;

		int l_flipChanceX = Random.Range(1,3);
		int l_flipChanceY = Random.Range(1,3);

		if (l_flipChanceX > 1)
		{
			_xSpeed = -_xSpeed;
		}

		if (l_flipChanceY > 1)
		{
			_ySpeed = -_ySpeed;
		}
	}

	void OnEnable ()
	{
		SoftPauseScript.instance.AddToHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);
	}

	void OnDisable ()
	{
		SoftPauseScript.instance.RemoveFromHandler(Enums.UpdateType.SoftUpdate, SoftUpdate);
	}
	
	// Update is called once per frame
	void SoftUpdate (GameObject p_dispatcher) 
	{
		CheckEdges();
		Move();
	}

	private void CheckEdges ()
	{
		// We're moving right
		if (_xSpeed > 0)
		{
			// If we hit the screen edge bounce back
			if (_transform.position.x > PlatformManager.instance.screenRight)
			{
				_xSpeed = -_xSpeed;
			}
		}
		else // Moving left
		{
			if (_transform.position.x < PlatformManager.instance.screenLeft)
			{
				_xSpeed = -_xSpeed;
			}
		}

		// We're moving up
		if (_ySpeed > 0)
		{
			if (_transform.position.y > PlatformManager.instance.screenTop)
			{
				_ySpeed = -_ySpeed;
			}
		}
		else // We're moving down
		{
			if (_transform.position.y < PlatformManager.instance.screenBottom)
			{
				_ySpeed = -_ySpeed;
			}
		}
	}

	private void Move ()
	{
		Vector3 l_newPosition = _transform.position;

		l_newPosition.x += _xSpeed * TimeManager.deltaTime;
		l_newPosition.y += _ySpeed * TimeManager.deltaTime;

		_transform.position = l_newPosition;
	}
}
