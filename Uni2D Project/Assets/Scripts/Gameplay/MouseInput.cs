using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour 
{
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
		bool l_clicked = false;
		Vector3 l_clickedPosition = Input.mousePosition;
		if (PlatformManager.instance.isMobile)
		{
			if (Input.touchCount > 0)
			{
				l_clicked = (Input.GetTouch(0).phase == TouchPhase.Began);
				l_clickedPosition = Input.GetTouch(0).position;
			}
		}
		else
		{
			l_clicked = Input.GetMouseButtonDown(0);
		}

		if (!l_clicked)
		{
			return;
		}

		RaycastHit l_hit;
		Ray l_ray = Camera.main.ScreenPointToRay(l_clickedPosition);

		if (Physics.Raycast(l_ray, out l_hit)) 
		{
			GameObject l_objectHit = l_hit.transform.gameObject;

			// check object clicked
		}
	}
}
