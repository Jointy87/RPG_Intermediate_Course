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
			text = GetComponent<Text>();

			text.text = baseStats.FetchLevel().ToString();
		}

		public void PrintLevel()
		{
			text.text = baseStats.FetchLevel().ToString();
		}
	}
}
