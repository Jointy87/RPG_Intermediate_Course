using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Core;

namespace RPGCourse.Combat
{
	public class Fighter : MonoBehaviour, IAction 
	{
		//Config parameters
		[SerializeField] float weaponRange = 2f;
		[SerializeField] float tempAttackInterval = .5f;
		[SerializeField] float tempWeaponDamage = 10f;

		//Cache
		Transform target;
		Mover mover;
		float timeSinceLastAttack = 0;

		private void Start() 
		{
			mover = GetComponent<Mover>();
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;
			GetInRange();
		}

		private void GetInRange()
		{
			if (!target) return;

			bool isInRange = Vector3.Distance(transform.position, target.position) < weaponRange;

			if (!isInRange)
			{
				mover.MoveTo(target.position);
			}
			else
			{
				mover.Cancel();
				AttackBehaviour();
			}
		}

		private void AttackBehaviour()
		{
			if(timeSinceLastAttack >= tempAttackInterval)
			{
				GetComponent<Animator>().SetTrigger("attack");
				timeSinceLastAttack = 0;
			}

		}
		
		//Animation Event
		void Hit()
		{
			if (!target) return;
			target.GetComponent<Health>().TakeDamage(tempWeaponDamage);
		}

		public void Attack(CombatTarget combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);

			target = combatTarget.transform;
		}

		public void Cancel()
		{
			target = null;
		}
	}
}