using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Stats
{
	public class BaseStats : MonoBehaviour
	{
		[Range(1, 99)]
		[SerializeField] int startingLevel = 1;
		[SerializeField] CharacterClass characterClass;
		[SerializeField] Progression progression = null;

		public float FetchStat(Stat stat)
		{
			return progression.FetchStat(stat, characterClass, startingLevel);
		}
	}
}