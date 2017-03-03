using UnityEngine;
using System.Collections;

[System.Serializable]
public class FluctuatingNumber 
{
	public float _value
	{
		get
		{
			float l_time = TimeManager.instance.totalGameTime - _timeOffset;
			if(l_time < _curveDelay)
			{
				return _startValue;
			}
			else if(l_time >= _curveDelay + _timeToArriveAtEndValue)
			{
				return _endValue;
			}
			else
			{
				float l_relativeTime = (l_time - _curveDelay) / _timeToArriveAtEndValue;
				float l_relativePoint = _valueCurve.Evaluate(l_relativeTime);

				return _startValue + ((_endValue - _startValue) * l_relativePoint);
			}
		}
	}

	public float _startValue;
	public float _endValue;
	public float _curveDelay;
	public float _timeToArriveAtEndValue;
	public AnimationCurve _valueCurve;

	private float _timeOffset;

	public void Reset()
	{
		_timeOffset = TimeManager.instance.totalGameTime;
	}
}