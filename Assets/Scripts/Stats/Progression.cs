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

		public float FetchStat(Stat incomingStat, CharacterClass incomingClass, int level)
		{
			foreach(ProgressionCharacerClass character in characterClasses)
			{
				if(character.characterClass != incomingClass) continue;

				foreach(ProgressionStat progStat in character.stats)
				{
					if(progStat.stat != incomingStat) continue;
					if(level > progStat.levels.Length) continue;

					return progStat.levels[level - 1];
				}
			}
			return 0;
		}
	}
}
