using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Core
{
	public class FaceCamera : MonoBehaviour
	{
		void Update()
		{
			transform.forward = Camera.main.transform.forward;
		}
	}
}
