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
		int currentLevel = 0; //Not a valid level but we do this to make sure we correctly initialize it via starting level or exp
		
		private void Start() 
		{
			currentLevel = CalculateLevel();
			Experience experience = GetComponent<Experience>();
			if(experience != null) experience.onExperienceGained += UpdateLevel;
		}

		private void UpdateLevel() 
		{
			if(gameObject.tag != "Player") return;

			int newLevel = CalculateLevel();
			if(newLevel > currentLevel)
			{
				currentLevel = newLevel;
			}
		}

		public float FetchStat(Stat stat)
		{
			return progression.FetchStat(stat, characterClass, FetchLevel());
		}

		public int FetchLevel()
		{
			if(currentLevel < 1) currentLevel = CalculateLevel();
			return currentLevel;
		}

		public int CalculateLevel()
		{
			Experience experience = GetComponent<Experience>();
			if (!experience) return startingLevel;

			float currentXP = experience.FetchExperience();
			 
			int penultimateLevel = progression.FetchAmountOfLevels(Stat.ExpToLevel, characterClass);

			for (int level = 1; level <= penultimateLevel; level++)
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