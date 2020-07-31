using System.Collections;
using System.Collections.Generic;
using RPGCourse.Resources;
using UnityEngine;

namespace RPGCourse.Stats
{
	public class BaseStats : MonoBehaviour
	{
		//Config parameters
		[Range(1, 99)]
		[SerializeField] int startingLevel = 1;
		[SerializeField] CharacterClass characterClass;
		[SerializeField] Progression progression = null;

		//States
		int currentLevel = 1;

		private void Update() 
		{
			if(gameObject.tag == "Player")
			{
				print(FetchLevel());
			}
		}

		public float FetchStat(Stat stat)
		{
			return progression.FetchStat(stat, characterClass, startingLevel);
		}

		public int FetchLevel()
		{
			float currentXP =  GetComponent<Experience>().FetchExperience();
			int penultimateLevel = progression.FetchAmountOfLevels(Stat.ExpToLevel, characterClass);

			for (int level = 1; level < penultimateLevel; level++)
			{
				float xpToLevelUp = progression.FetchStat(Stat.ExpToLevel, characterClass, level);

				if(xpToLevelUp > currentXP)
				{
					return level;
				}				
			}
			return penultimateLevel + 1;
			
		}
	}
}