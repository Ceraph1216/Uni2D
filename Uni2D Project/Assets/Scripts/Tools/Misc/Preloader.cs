using UnityEngine;
using System.Collections;

public class Preloader : MonoBehaviour 
{
	void Awake ()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene ("TitleScreen");
	}
}
