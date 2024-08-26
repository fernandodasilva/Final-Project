using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ENDB
{
	public class RotationScript : MonoBehaviour
	{
		public bool rotating = false;
		public int speed = 33;

		void Update ()
		{
			if (rotating)
			{
				transform.Rotate(Vector3.up, speed * Time.deltaTime);
				transform.Rotate(Vector3.left, speed * Time.deltaTime);
			}
		}

		public void ToggleRotation ()
		{
			rotating = !rotating;
		}
	}
}
