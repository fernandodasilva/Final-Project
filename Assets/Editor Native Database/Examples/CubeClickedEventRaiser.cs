using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENDB
{
	public class CubeClickedEventRaiser : GameEventRaiserBase
	{
        private Camera mainCamera = null;

		void Start () 
		{ // cache camera because Camera.main uses GameObject.Find
			mainCamera = Camera.main;
		}
		
		void Update ()
		{
			if (Input.GetMouseButtonDown(0)) // left mouse button clicked
			{
				Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if (Physics.Raycast(ray, out hit))
				{
					if (hit.collider.gameObject == this.gameObject) //ray hit the cube
						RaiseEvent(); //Raise the event
				}
			}
		}

	}
}
