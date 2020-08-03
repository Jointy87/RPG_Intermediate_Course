using System;
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
		[SerializeField] GameObject levelUpVFX;
		[SerializeField] bool shouldUseModifiers = false;

		//States
		int currentLevel = 0; //Not a valid level but we do this to make sure we correctly initialize it via starting level or exp

		public event Action onLevelUp;

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
				SpawnLevelUpVFX();
				onLevelUp();
			}
		}

		private void SpawnLevelUpVFX()
		{
			Instantiate(levelUpVFX, transform); //This overload decides parent
		}

		public float FetchStat(Stat stat)
		{
			return (FetchBaseStat(stat) + 
				FetchAdditiveModifier(stat)) * (1 + FetchPercentageModifier(stat) / 100);
		}

		private float FetchBaseStat(Stat stat)
		{
			return progression.FetchStat (stat, characterClass, FetchLevel());
		}

		public int FetchLevel()
		{
			if(currentLevel < 1) currentLevel = CalculateLevel();
			return currentLevel;
		}

		private float FetchAdditiveModifier(Stat stat)
		{
			if(!shouldUseModifiers) return 0;
			float total = 0;
			//You can look for imodifier type because we've added it on top of fighter
			foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
			{
				foreach(float modifier in provider.FetchAdditiveModifiers(stat))
				{
					total += modifier;
				}
			}
			return total;
		}

		private float FetchPercentageModifier(Stat stat)
		{
			if (!shouldUseModifiers) return 0;
			float total = 0;

			foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
			{
				foreach(float modifier in provider.FetchPercentageModifiers(stat))
				{
					total += modifier;
				}
			}
			return total;
		}

		private int CalculateLevel()
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