using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using RPGCourse.Saving;

namespace RPGCourse.Stats
{
	public class Experience : MonoBehaviour, ISaveable
	{
		//States
		float expPoints = 0;

		public event Action onExperienceGained; //delegates are a good way to invert dependencies

		public void AddExperience(float value)
		{
			expPoints += value;
			onExperienceGained();
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
