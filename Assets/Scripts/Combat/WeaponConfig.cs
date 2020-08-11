using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Attributes;

namespace RPGCourse.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
	public class WeaponConfig : ScriptableObject
	{
		//Config parameters
		[SerializeField] float weaponRange = 2f;
		[SerializeField] float attackInterval = .5f;
		[SerializeField] float weaponDamage = 10f;
		[SerializeField] float percentageBonus = 0;
		[SerializeField] Weapon equippedWeapon = null;
		[SerializeField] AnimatorOverrideController animatorOverride = null;
		[SerializeField] bool isLeftHanded = false;
		[SerializeField] Projectile projectile = null;

		const string weaponName = "EquippedWeapon";

		public Weapon Spawn(Transform rightHand, Transform leftHand, Animator animator)
		{
			DestroyEquippedWeapon(rightHand, leftHand);

			Weapon spawnedWeapon = null;

			if(equippedWeapon != null)
			{
				Transform handTransform = GetHand(rightHand, leftHand);

				spawnedWeapon = Instantiate(equippedWeapon, handTransform);
				spawnedWeapon.gameObject.name = weaponName;
			}

			//If casting fails (in this case if it's normal animator) it will return null.
			//Either normal animator or animator override
			var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
			
			if(animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
			else if (overrideController != null) 
				animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;

			return spawnedWeapon;
		}

		private Transform GetHand(Transform rightHand, Transform leftHand)
		{
			Transform handTransform;

			if (isLeftHanded) handTransform = leftHand;
			else handTransform = rightHand;
			return handTransform;
		}

		public void LaunchProjectile(Transform rightHand, Transform leftHand, 
			Health target, GameObject instigator, float damageOutput)
		{
			Projectile projectileInstance = 
				Instantiate(projectile, GetHand(rightHand, leftHand).position, Quaternion.identity);
				
			projectileInstance.SetTarget(target, damageOutput, instigator);
		}

		private void DestroyEquippedWeapon(Transform rightHand, Transform leftHand)
		{
			Transform equippedWeapon = rightHand.Find(weaponName);
			if(equippedWeapon == null) equippedWeapon = leftHand.Find(weaponName);
			if(!equippedWeapon) return;
			equippedWeapon.name = "DestroyingWeapon";
			Destroy(equippedWeapon.gameObject);
		}

		public bool HasProjectile()
		{
			return projectile != null;
		}

		public float FetchRange()
		{
			return weaponRange;
		}

		public float FetchInterval()
		{
			return attackInterval;
		}

		public float FetchDamage()
		{
			return weaponDamage;
		}

		public float FetchPercentageBonus()
		{
			return percentageBonus;
		}
	}
}
