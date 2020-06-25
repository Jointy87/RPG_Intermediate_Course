using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Core;

namespace RPGCourse.Combat
{
	public class Fighter : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float weaponRange = 2f;

		//Cache
		Transform target;
		Mover mover;

		private void Start() 
		{
			mover = GetComponent<Mover>();
		}

		private void Update()
		{
			GetInRange();
		}

		private void GetInRange()
		{
			if (!target) return;

			bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;

			if (target != null && !isInRange)
			{
				mover.MoveTo(target.position);
			}
			else
			{
				mover.StopMoving();
			}
		}

		public void Attack(CombatTarget combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			target = combatTarget.transform;
		}

		public void CancelAttack()
		{
			target = null;
		}
	}
}