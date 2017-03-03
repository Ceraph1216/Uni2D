using UnityEngine;
using System.Collections;
public class CameraShake : MonoBehaviour
{
	public Enums.UpdateType[] _updates = new Enums.UpdateType[] { Enums.UpdateType.SoftUpdate };
	
	public float _absoluteMax;
	public float _universalFactor = 1;
	
	private float _intensity;
	private float _timeToStop;
	
	private Transform _myTransform;
	//private CameraFollowMouseBehavior _cameraMovement;
	
	//private Vector3 _originPosition;
	private Quaternion _originalRotation;
	private float _amountToDecayPerSecond;
	
	void Awake()
	{
		_myTransform = transform;
		//_cameraMovement = _myTransform.parent.GetComponent<CameraFollowMouseBehavior>();
		
		//_originPosition = _myTransform.position;
		_originalRotation = _myTransform.localRotation;
	}
	
	public void Shake(float l_intensity, float l_decay, float l_localMax)
	{
		_intensity = _intensity + l_intensity;
		
		// Apply the local max, if we want it
		if(l_localMax != 0)
		{
			_intensity = Mathf.Min(_intensity, l_localMax);
		}
		
		// Apply the absolute max
		_intensity = Mathf.Min((_intensity * _universalFactor), (_absoluteMax * _universalFactor));
		
		_timeToStop = l_decay;
		_amountToDecayPerSecond = _intensity / _timeToStop;
		
		this.enabled = true;
	}
	
	void OnEnable()
	{
		for(int i = 0; i < _updates.Length; i++)
		{
			SoftPauseScript.instance.AddToHandler(_updates[i], SoftUpdate);
		}
	}
	
	void OnDisable()
	{
		//_myTransform.position = _originPosition;
		_myTransform.localRotation = _originalRotation;
		
		for(int i = 0; i < _updates.Length; i++)
		{
			SoftPauseScript.instance.RemoveFromHandler(_updates[i], SoftUpdate);
		}
	}
	
	void SoftUpdate(GameObject dispatcher)
	{
		if (_intensity > 0)
		{
			//_myTransform.position = _originPosition + Random.insideUnitSphere * _intensity;
			_myTransform.localRotation = new Quaternion(
				_originalRotation.x + (Random.Range(-_intensity, _intensity) * 60 * 0.2F * TimeManager.deltaTime),
				_originalRotation.y + (Random.Range(-_intensity, _intensity) * 60 * 0.2F * TimeManager.deltaTime),
				_originalRotation.z + (Random.Range(-_intensity, _intensity) * 60 * 0.2F * TimeManager.deltaTime),
				_originalRotation.w + (Random.Range(-_intensity, _intensity) * 60 * 0.2F * TimeManager.deltaTime));

			_intensity -= _amountToDecayPerSecond * TimeManager.deltaTime;
		}
		else
		{
			this.enabled = false;
		}
	}
}