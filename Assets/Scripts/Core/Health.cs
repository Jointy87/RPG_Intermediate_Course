using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Core
{
	public class Health : MonoBehaviour
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
			if (healthPoints == 0 && isAlive)
			{
				isAlive = false;
				GetComponent<Animator>().SetTrigger("die");
				GetComponent<ActionScheduler>().CancelCurrentAction();
			}
		}
	}
}
