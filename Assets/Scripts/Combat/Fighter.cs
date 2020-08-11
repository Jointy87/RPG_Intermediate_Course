using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Core;
using RPGCourse.Saving;
using RPGCourse.Attributes;
using RPGCourse.Stats;
using GameDevTV.Utils;

namespace RPGCourse.Combat
{
	public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
	{
		//Config parameters
		[SerializeField] Transform rightHandTransform = null;
		[SerializeField] Transform leftHandTransform = null;
		[SerializeField] WeaponConfig defaultWeaponConfig = null;

		//Cache
		Health target;
		Mover mover;
		Animator animator;

		//States
		float timeSinceLastAttack = Mathf.Infinity;
		WeaponConfig currentWeaponConfig;
		LazyValue<Weapon> currentWeapon;

		private void Awake() 
		{
			mover = GetComponent<Mover>();
			animator = GetComponent<Animator>();
			currentWeaponConfig = defaultWeaponConfig;
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
			return AttachWeapon(defaultWeaponConfig);
		}

		public void EquipWeapon(WeaponConfig weapon)
		{
			currentWeaponConfig = weapon;
			currentWeapon.value = AttachWeapon(weapon);
		}

		private Weapon AttachWeapon(WeaponConfig weapon)
		{
			return weapon.Spawn(rightHandTransform, leftHandTransform, animator);
		}

		private void GetInRange()
		{
			if (!target || !target.IsAlive()) return;

			bool isInRange = Vector3.Distance
				(transform.position, target.transform.position) < currentWeaponConfig.FetchRange();

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

			if(timeSinceLastAttack >= currentWeaponConfig.FetchInterval())
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

			if(currentWeapon.value != null) currentWeapon.value.OnHit();

			if(currentWeaponConfig.HasProjectile())
			{
				currentWeaponConfig.LaunchProjectile
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

			if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position)) return false;

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
				yield return currentWeaponConfig.FetchDamage();
			}
		}

		public IEnumerable<float> FetchPercentageModifiers(Stat stat)
		{
			if(stat == Stat.DamageOutput)
			{
				yield return currentWeaponConfig.FetchPercentageBonus();
			}
		}

		public object CaptureState()
		{
			return currentWeaponConfig.name;
		}

		public void RestoreState(object state)
		{
			if(tag == "Player")
			{
				string weaponName = (string)state;
				WeaponConfig weaponToLoad = 
					UnityEngine.Resources.Load<WeaponConfig>(weaponName);
				EquipWeapon(weaponToLoad);
			}
			else EquipWeapon(defaultWeaponConfig);
		}
	}
}