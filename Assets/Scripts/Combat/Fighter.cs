using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Core;

namespace RPGCourse.Combat
{
	public class Fighter : MonoBehaviour, IAction
	{
		//Config parameters
		[SerializeField] Transform handTransform = null;
		[SerializeField] Weapon defaultWeapon = null;

		//Cache
		Health target;
		Mover mover;
		Animator animator;

		//States
		float timeSinceLastAttack = Mathf.Infinity;
		Weapon currentWeapon = null;

		private void Awake() 
		{
			mover = GetComponent<Mover>();
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			EquipWeapon(defaultWeapon);
		}

		public void EquipWeapon(Weapon weaponType)
		{
			currentWeapon = weaponType;
			currentWeapon.Spawn(handTransform, animator);
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;
			GetInRange();
		}

		private void GetInRange()
		{
			if (!target || !target.IsAlive()) return;

			bool isInRange = Vector3.Distance
				(transform.position, target.transform.position) < currentWeapon.FetchRange();

			if (!isInRange)
			{
				mover.MoveTo(target.transform.position, 1f);
			}
			else
			{
				mover.Cancel();
				AttackBehaviour();
			}
		}

		private void AttackBehaviour()
		{
			transform.LookAt(target.transform.position);

			if(timeSinceLastAttack >= currentWeapon.FetchInterval())
			{
				TriggerAttackAnimation();
				timeSinceLastAttack = 0;
			}
		}

		private void TriggerAttackAnimation()
		{
			animator.ResetTrigger("stopAttacking");
			animator.SetTrigger("attack");
		}

		void Hit() //Animation Event
		{
			if (!target) return;
			target.TakeDamage(currentWeapon.FetchDamage());
		}

		public void Attack(GameObject combatTarget)
		{
			GetComponent<ActionScheduler>().StartAction(this);

			target = combatTarget.GetComponent<Health>();
		}

		public void Cancel()
		{
			TriggerStopAttackInAnimator();
			target = null;
		}

		private void TriggerStopAttackInAnimator()
		{
			animator.ResetTrigger("attack");
			animator.SetTrigger("stopAttacking");
		}

		public bool CanAttack(GameObject combatTarget)
		{
			if(!combatTarget) return false;

			Health targetToTest = combatTarget.GetComponent<Health>();
			return targetToTest != null && targetToTest.IsAlive();
		}
	}
}