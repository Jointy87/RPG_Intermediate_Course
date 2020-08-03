using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Saving;
using RPGCourse.Stats;
using RPGCourse.Core;

namespace RPGCourse.Resources
{
	public class Health : MonoBehaviour, ISaveable
	{
		//Config parameters
		[SerializeField] float healthRegenPercentage = 75;

		//States
		bool isAlive = true;
		float statHealth;
		float healthPoints = -1f; //Upon death, healthPoints = 0. -1 can never happen in-game. By initializing as -1, at start you check if it's below 0 to know if it's the initialized value or loaded value to fix race condition.

		private void Start() 
		{
			BaseStats baseStats = GetComponent<BaseStats>();
			statHealth = baseStats.FetchStat(Stat.Health);

			if(healthPoints < 0) healthPoints = statHealth;
			if(baseStats != null) baseStats.onLevelUp += RestoreHealth;
		}

		public float FetchMaxhealth()
		{
			return statHealth;
		}

		public void TakeDamage(GameObject instigator, float damage)
		{
			if (!isAlive) return;

			print(gameObject.name + " took damage: " + damage);

			healthPoints = Mathf.Max(healthPoints - damage, 0); // takes highest value, in this case either health - damage, or 0
			if (healthPoints == 0)
			{
				Die();
				RewardExperience(instigator);
			}
		}

		private void Die()
		{
			if (!isAlive) return;

			isAlive = false;
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void RewardExperience(GameObject instigator)
		{
			if (!instigator.GetComponent<Experience>()) return;
			float expToReward = GetComponent<BaseStats>().FetchStat(Stat.ExpReward);
			instigator.GetComponent<Experience>().AddExperience(expToReward);
		}

		private void RestoreHealth()
		{
			float regenHealthPoints = statHealth * (healthRegenPercentage / 100);
			healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
		}
		
		public bool IsAlive()
		{
			return isAlive;
		}

		public float FetchHealth()
		{
			return healthPoints;
		}

		public object CaptureState()
		{
			return healthPoints;
		}

		public void RestoreState(object state)
		{
			healthPoints = (float)state;
			if(healthPoints == 0)
			{
				Die();
			}
			else if(healthPoints > 0 && !isAlive)
			{
				GetComponent<Animator>().SetTrigger("revive");
				isAlive = true;
			}
		}
	}
}
