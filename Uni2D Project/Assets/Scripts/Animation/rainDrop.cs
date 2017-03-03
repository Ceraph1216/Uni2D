using UnityEngine;
using System.Collections;

public class rainDrop : MonoBehaviour 
{
	public float speed;
	public float startDelay;
	public float endDelay;
	public float endY;			// Deprecated but don't want to erase values just yet
	public Transform endPointMarker;
	public ParticleSystem splashParticles;
	public int numSplashParticles;

	private Transform _transform;
	private ParticleSystem _particles;

	private Vector3 _resetPosition;
	private float _endY;
	private bool _hasReachedEnd;
	private float _delayTimer;

	// cache component references
	void Awake ()
	{
		_transform = transform;
		_particles = GetComponent<ParticleSystem>();
	}

	// Use this for initialization
	void Start () 
	{
		_resetPosition = _transform.position;
		_endY = endPointMarker.position.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		// If we're at our destination height
		if (_hasReachedEnd)
		{
			// Wait for the particle systems to finish
			if (_delayTimer < endDelay)
			{
				_delayTimer += Time.deltaTime;
				return;
			}

			Reset();
			return;
		}

		// Wait to start moving for the appropriate amount of time
		if (_delayTimer < startDelay)
		{
			_delayTimer += Time.deltaTime;
			return;
		}

		Move();
	}

	/// <summary>
	/// Reset all the necessary values to recycle this rain drop
	/// </summary>
	void Reset ()
	{
		_transform.position = _resetPosition;
		_hasReachedEnd = false;
		_delayTimer = 0;
		_particles.Play();
	}

	/// <summary>
	/// Move the rain particle down the screen by our speed
	/// </summary>
	void Move ()
	{
		// Move the rain drop down by our speed
		Vector3 l_newPosition = _transform.position;
		l_newPosition.y += speed * Time.deltaTime;

		// Check if the rain drop should end and make a splash effect
		if (l_newPosition.y <= _endY)
		{
			_hasReachedEnd = true;
			_delayTimer = 0;
			l_newPosition.y = _endY;
			_particles.Stop();
			splashParticles.Emit(numSplashParticles);
		}

		_transform.position = l_newPosition;
	}
}
