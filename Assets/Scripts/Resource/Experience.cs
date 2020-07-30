using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Resources
{
	public class Experience : MonoBehaviour
	{
		//States
		float expPoints = 0;

		public void AddExperience(float value)
		{
			expPoints += value;
		}
	}
}
