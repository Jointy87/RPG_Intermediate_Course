using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Saving;

namespace RPGCourse.Resources
{
	public class Experience : MonoBehaviour, ISaveable
	{
		//States
		float expPoints = 0;

		public void AddExperience(float value)
		{
			expPoints += value;
		}

		public float FetchExperience()
		{
			return expPoints;
		}

		public object CaptureState()
		{
			return expPoints;
		}

		public void RestoreState(object state)
		{
			expPoints = (float)state;
		}
	}
}
