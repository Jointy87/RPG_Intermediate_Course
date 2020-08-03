using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCourse.Stats
{
	public class LevelDisplay : MonoBehaviour
	{
		//Cache
		BaseStats baseStats;
		Text text;

		void Awake()
		{
			baseStats = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseStats>();
		}

		private void Update() 
		{
			GetComponent<Text>().text = string.Format("{0:0}", baseStats.FetchLevel());
		}
	}
}
