using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Combat
{
	public class WeaponPickup : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Weapon weaponType = null;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				other.GetComponent<Fighter>().EquipWeapon(weaponType);
				Destroy(gameObject);
			}
		}
	}
}
