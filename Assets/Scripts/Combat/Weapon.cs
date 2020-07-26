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

		public void Spawn(Transform handTransform, Animator animator)
		{
			if(equippedPrefab != null)
			{
				Instantiate(equippedPrefab, handTransform);
			}
			if(animatorOverride != null)
			{
				animator.runtimeAnimatorController = animatorOverride;
			}	
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
