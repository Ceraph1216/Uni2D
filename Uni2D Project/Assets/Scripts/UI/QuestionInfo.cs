using UnityEngine;
using System.Collections;

[System.Serializable]
public class Answer
{
	/// <summary>
	/// Whether or not the message needs to be removed from the list
	/// </summary>
	public bool dirty;
	public bool isCorrect;
	public string text;
}

[System.Serializable]
public class QuestionInfo : ScriptableObject
{
	public string questionName;
	public string key;
	public string prompt;
	public string answerExplanation;
	public string imageURL;
	public Answer[] answers;
}