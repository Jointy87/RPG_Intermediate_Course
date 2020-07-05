using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Saving;

namespace RPGCourse.Core
{
	public class Health : MonoBehaviour, ISaveable
	{
		//Config parameters
		[SerializeField] float healthPoints = 100f;

		//Cache
		bool isAlive = true;

		public bool IsAlive()
		{
			return isAlive;
		}


		public void TakeDamage(float damage)
		{
			healthPoints = Mathf.Max(healthPoints - damage, 0); // takes highest value, in this case either health - damage, or 0
			if (healthPoints == 0)
			{
				Die();
			}
		}

		private void Die()
		{
			if (!isAlive) return;

			isAlive = false;
			GetComponent<Animator>().SetTrigger("die");
			GetComponent<ActionScheduler>().CancelCurrentAction();
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
