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
		Health target;
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
			if (!target || !target.FetchAliveStatus()) return;

			bool isInRange = Vector3.Distance(transform.position, target.transform.position) < weaponRange;

			if (!isInRange)
			{
				mover.MoveTo(target.transform.position);
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
		
		void Hit() //Animation Event
		{
			if (!target) return;
			target.TakeDamage(tempWeaponDamage);
		}

		public void Attack(CombatTarget combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);

			target = combatTarget.GetComponent<Health>();
		}

		public void Cancel()
		{
			GetComponent<Animator>().SetTrigger("stopAttacking");
			target = null;
		}
	}
}