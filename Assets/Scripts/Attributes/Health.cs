using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Saving;
using RPGCourse.Stats;
using RPGCourse.Core;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPGCourse.Attributes
{
	public class Health : MonoBehaviour, ISaveable
	{
		//Config parameters
		[SerializeField] float healthRegenPercentage = 75;
		[SerializeField] TakeDamageEvent onDamageTaken;
		[SerializeField] RestoreHealthEvent onHealthRestored;
		[SerializeField] UnityEvent onDeath;

		[System.Serializable]
		public class TakeDamageEvent : UnityEvent<float, Color> {}

		[System.Serializable]
		public class RestoreHealthEvent : UnityEvent<float, Color> { }

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
			if (baseStats != null) baseStats.onLevelUp += RestoreHealthAtLevelUp;
		}

		private void OnDisable() 
		{
			if (baseStats != null) baseStats.onLevelUp -= RestoreHealthAtLevelUp;
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

			healthPoints.value = Mathf.Max(healthPoints.value - damage, 0); // takes highest value, in this case either health - damage, or 0

			onDamageTaken.Invoke(damage, Color.red);

			float normalizedHealth = healthPoints.value / FetchMaxHealth();
			
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
			onDeath.Invoke();
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}

		private void RewardExperience(GameObject instigator)
		{
			if (!instigator.GetComponent<Experience>()) return;
			float expToReward = GetComponent<BaseStats>().FetchStat(Stat.ExpReward);
			instigator.GetComponent<Experience>().AddExperience(expToReward);
		}

		private void RestoreHealthAtLevelUp()
		{
			float regenHealthPoints = baseStats.FetchStat(Stat.Health) * (healthRegenPercentage / 100);
			float previousHealth = healthPoints.value;
			healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
			float healthRegained = healthPoints.value - previousHealth;

			onHealthRestored.Invoke(healthRegained, Color.green);
		}

		public void RestoreHealth(float amount)
		{
			healthPoints.value = Mathf.Min(baseStats.FetchStat(Stat.Health), healthPoints.value + amount);
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
			return healthPoints.value;
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
