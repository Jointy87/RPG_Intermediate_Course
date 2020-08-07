using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
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
		
		//Cache
		Experience experience;

		//States
		LazyValue<int> currentLevel;

		public event Action onLevelUp;

		private void Awake() 
		{
			experience = GetComponent<Experience>();
			currentLevel = new LazyValue<int>(CalculateLevel);
		}

		private void OnEnable()
		{
			if (experience != null) experience.onExperienceGained += UpdateLevel;
		}
		
		private void OnDisable()
		{
			if (experience != null) experience.onExperienceGained -= UpdateLevel;
		}

		private void Start() 
		{
			currentLevel.ForceInit();
		}

		private int CalculateLevel()
		{
			if (!experience) return startingLevel;

			float currentXP = experience.FetchExperience();

			int penultimateLevel = progression.FetchAmountOfLevels(Stat.ExpToLevel, characterClass);

			for (int level = 1; level <= penultimateLevel; level++)
			{
				float xpToLevelUp = progression.FetchStat(Stat.ExpToLevel, characterClass, level);

				if (xpToLevelUp > currentXP)
				{
					return level;
				}
			}
			return penultimateLevel + 1;
		}

		public int FetchLevel()
		{
			if (currentLevel.value < 1) currentLevel.value = CalculateLevel();
			return currentLevel.value;
		}

		private void UpdateLevel() 
		{
			if(gameObject.tag != "Player") return;

			int newLevel = CalculateLevel();
			if(newLevel > currentLevel.value)
			{
				currentLevel.value = newLevel;
				SpawnLevelUpVFX();
				onLevelUp();
			}
		}

		public float FetchStat(Stat stat)
		{
			return (FetchBaseStat(stat) +
				FetchAdditiveModifier(stat)) * (1 + FetchPercentageModifier(stat) / 100);
		}

		private float FetchBaseStat(Stat stat)
		{
			return progression.FetchStat(stat, characterClass, FetchLevel());
		}

		private float FetchAdditiveModifier(Stat stat)
		{
			if (!shouldUseModifiers) return 0;
			float total = 0;
			//You can look for imodifier type because we've added it on top of fighter
			foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
			{
				foreach (float modifier in provider.FetchAdditiveModifiers(stat))
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

			foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
			{
				foreach (float modifier in provider.FetchPercentageModifiers(stat))
				{
					total += modifier;
				}
			}
			return total;
		}

		private void SpawnLevelUpVFX()
		{
			Instantiate(levelUpVFX, transform); //This overload decides parent
		}	
	}
}