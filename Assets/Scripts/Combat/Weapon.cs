using RPGCourse.Core;
using UnityEngine;

namespace RPGCourse.Combat
{
	[CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/New Weapon", order = 0)]
	public class Weapon : ScriptableObject
	{
		//Config parameters
		[SerializeField] float weaponRange = 2f;
		[SerializeField] float attackInterval = .5f;
		[SerializeField] float weaponDamage = 10f;
		[SerializeField] GameObject equippedPrefab = null;
		[SerializeField] AnimatorOverrideController animatorOverride = null;
		[SerializeField] bool isLeftHanded = false;
		[SerializeField] Projectile projectile = null;

		//States
		GameObject spawnedWeapon = null;

		const string weaponName = "EquippedWeapon";

		public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
		{
			DestroyEquippedWeapon(rightHand, leftHand);

			if(equippedPrefab != null)
			{
				Transform handTransform = GetHand(rightHand, leftHand);

				GameObject spawnedWeapon = Instantiate(equippedPrefab, handTransform);
				spawnedWeapon.name = weaponName;
			}

			if (animatorOverride != null) animator.runtimeAnimatorController = animatorOverride;
		}

		private Transform GetHand(Transform rightHand, Transform leftHand)
		{
			Transform handTransform;

			if (isLeftHanded) handTransform = leftHand;
			else handTransform = rightHand;
			return handTransform;
		}

		public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
		{
			Projectile projectileInstance = 
			Instantiate(projectile, GetHand(rightHand, leftHand).position, Quaternion.identity);
			projectileInstance.SetTarget(target, weaponDamage);
		}

		public void DestroyEquippedWeapon(Transform rightHand, Transform leftHand)
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

	}
}
