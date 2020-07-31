using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Stats
{
	[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
	public class Progression : ScriptableObject
	{
		//Config parameters
		[SerializeField] ProgressionCharacerClass[] characterClasses = null;

		[System.Serializable]
		class ProgressionCharacerClass
		{
			public CharacterClass characterClass;
			public ProgressionStat[] stats;
		}

		[System.Serializable]
		class ProgressionStat
		{
			public Stat stat;
			public float[] levels;
		}

		Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

		public float FetchStat(Stat incomingStat, CharacterClass incomingClass, int level)
		{
			BuildLookup();

			float[] levels = lookupTable[incomingClass][incomingStat];

			if (level > levels.Length)  return 0;
			return levels[level - 1];
		}

		public int FetchAmountOfLevels(Stat incomingStat, CharacterClass incomingClass)
		{
			BuildLookup();

			float[] levels = lookupTable[incomingClass][incomingStat];
			return levels.Length;
		}

		private void BuildLookup()
		{
			if(lookupTable != null) return;

			lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

			foreach (ProgressionCharacerClass progClass in characterClasses)
			{
				Dictionary<Stat, float[]> statLookupTable = new Dictionary<Stat, float[]>();

				foreach (ProgressionStat progStat in progClass.stats)
				{
					statLookupTable[progStat.stat] = progStat.levels;
				}

				lookupTable[progClass.characterClass] = statLookupTable;
			}
		}
	}
}
