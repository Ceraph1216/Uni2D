using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquationInfo : ScriptableObject
{
	public string equationName;
	public string key;
	public string prompt;
	public int answer;
	public string answerUnits;
}
