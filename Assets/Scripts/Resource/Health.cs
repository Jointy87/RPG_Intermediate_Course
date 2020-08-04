using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Saving;
using RPGCourse.Stats;
using RPGCourse.Core;
using GameDevTV.Utils;

namespace RPGCourse.Resources
{
	public class Health : MonoBehaviour, ISaveable
	{
		//Config parameters
		[SerializeField] float healthRegenPercentage = 75;

		//Cache
		BaseStats baseStats;

		//States
		bool isAlive = true;
		LazyValue<float> healthPoints; 

		private void Awake() 
		{
			baseStats = GetComponent<BaseStats>();
			healthPoints = new LazyValue<float>(FetchMaxHealth);
		}

		private void OnEnable() 
		{
			if (baseStats != null) baseStats.onLevelUp += RestoreHealth;
		}

		private void OnDisable() 
		{
			if (baseStats != null) baseStats.onLevelUp -= RestoreHealth;
		}

		private void Start() 
		{
			healthPoints.ForceInit();
		}

		public float FetchMaxHealth()
		{
			return baseStats.FetchStat(Stat.Health);
		}

		public void TakeDamage(GameObject instigator, float damage)
		{
			if (!isAlive) return;

			print(gameObject.name + " took damage: " + damage);

			healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); // takes highest value, in this case either health - damage, or 0
			if (healthPoints.value == 0)
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
			float regenHealthPoints = baseStats.FetchStat(Stat.Health) * (healthRegenPercentage / 100);
			healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
		}
		
		public bool IsAlive()
		{
			return isAlive;
		}

		public float FetchHealth()
		{
			return healthPoints.value;
		}

		public object CaptureState()
		{
			return healthPoints;
		}

		public void RestoreState(object state)
		{
			healthPoints.value = (float)state;
			if(healthPoints.value == 0)
			{
				Die();
			}
			else if(healthPoints.value > 0 && !isAlive)
			{
				GetComponent<Animator>().SetTrigger("revive");
				isAlive = true;
			}
		}
	}
}
