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
		//Cache
		bool isAlive = true;

		//States
		float healthPoints;

		private void Start() 
		{
			if(healthPoints != GetComponent<BaseStats>().FetchStat(Stat.Health)) return;
			else healthPoints = GetComponent<BaseStats>().FetchStat(Stat.Health);
		}

		public bool IsAlive()
		{
			return isAlive;
		}

		public float FetchHealthPercentage()
		{
			return 100 * (healthPoints / GetComponent<BaseStats>().FetchStat(Stat.Health));
		}


		public void TakeDamage(GameObject instigator, float damage)
		{
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
