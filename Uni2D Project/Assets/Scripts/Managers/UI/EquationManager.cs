using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquationManager : MonoBehaviour 
{
	public static EquationManager instance;

	public Dictionary<string, QuestionInfo> equationDictionary;
	public IncludedQuestions includedEquations;

	void Awake ()
	{
		instance = this;
		equationDictionary = new Dictionary<string, QuestionInfo> ();
		LoadEquations ();
	}

	private void LoadEquations ()
	{
		for (int i = 0; i < includedEquations.includedQuestions.Length; i++) 
		{
			QuestionInfo l_currentQuestion = includedEquations.includedQuestions [i];
			equationDictionary.Add (l_currentQuestion.key, l_currentQuestion);
		}
	}
}
