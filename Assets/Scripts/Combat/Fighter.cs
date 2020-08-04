using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Core;
using RPGCourse.Saving;
using RPGCourse.Resources;
using RPGCourse.Stats;
using GameDevTV.Utils;

namespace RPGCourse.Combat
{
	public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
	{
		//Config parameters
		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;
		[SerializeField] Weapon defaultWeapon = null;

		//Cache
		Health target;
		Mover mover;
		Animator animator;

		//States
		float timeSinceLastAttack = Mathf.Infinity;
		LazyValue<Weapon> currentWeapon;

		private void Awake() 
		{
			mover = GetComponent<Mover>();
			animator = GetComponent<Animator>();
			currentWeapon = new LazyValue<Weapon>(SetDefaultWeapon);
		}

		private void Start()
		{
			currentWeapon.ForceInit();
		}

		private void Update()
		{
			timeSinceLastAttack += Time.deltaTime;
			GetInRange();
		}

		private Weapon SetDefaultWeapon()
		{
			AttachWeapon(defaultWeapon);
			return defaultWeapon;
		}

		public void EquipWeapon(Weapon weapon)
		{
			currentWeapon.value = weapon;
			AttachWeapon(weapon);
		}

		private void AttachWeapon(Weapon weapon)
		{
			weapon.Spawn(rightHandTransform, leftHandTransform, animator);
		}

		private void GetInRange()
		{
			if (!target || !target.IsAlive()) return;

			bool isInRange = Vector3.Distance
				(transform.position, target.transform.position) < currentWeapon.value.FetchRange();

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

			if(timeSinceLastAttack >= currentWeapon.value.FetchInterval())
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

			float damageOutput = GetComponent<BaseStats>().FetchStat(Stat.DamageOutput);

			if(currentWeapon.value.HasProjectile())
			{
				currentWeapon.value.LaunchProjectile
					(rightHandTransform, leftHandTransform, target, gameObject, damageOutput);
				return;
			}
			
			target.TakeDamage(gameObject, damageOutput);
		}

		void Shoot() //Animation Event
		{
			Hit();
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

		public Health FetchTarget()
		{
			return target;
		}

		//using IEnumerable here so that we can return an empty list if stats don't match up. Or we can return multiple things if we were using 2 weapons at the same time
		public IEnumerable<float> FetchAdditiveModifiers(Stat stat)
		{
			if(stat == Stat.DamageOutput)
			{
				yield return currentWeapon.value.FetchDamage();
			}
		}

		public IEnumerable<float> FetchPercentageModifiers(Stat stat)
		{
			if(stat == Stat.DamageOutput)
			{
				yield return currentWeapon.value.FetchPercentageBonus();
			}
		}

		public object CaptureState()
		{
			return currentWeapon.value.name;
		}

		public void RestoreState(object state)
		{
			if(tag == "Player")
			{
				string weaponName = (string)state;
				Weapon weaponToLoad = 
					UnityEngine.Resources.Load<Weapon>(weaponName);
				EquipWeapon(weaponToLoad);
			}
			else EquipWeapon(defaultWeapon);
		}
	}
}