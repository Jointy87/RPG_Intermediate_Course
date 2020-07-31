using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCourse.Stats
{
	public class ExperienceDisplay : MonoBehaviour
	{
		//Cache
		Experience exp;
		Text expDisplay;

		private void Awake() 
		{
			exp = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
			expDisplay = GetComponent<Text>();
		}


		void Update()
		{
			expDisplay.text = string.Format("{0:0}", exp.FetchExperience().ToString());
		}
	}
}
