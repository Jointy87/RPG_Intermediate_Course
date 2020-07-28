using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Combat
{
	public class WeaponPickup : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Weapon weaponType = null;
		[SerializeField] float respawnTime = 5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				other.GetComponent<Fighter>().EquipWeapon(weaponType);
				StartCoroutine(HideForSeconds(respawnTime));
			}
		}

		IEnumerator HideForSeconds(float time)
		{
			ShowPickup(false);			

			yield return new WaitForSeconds(time);

			ShowPickup(true);
		}

		private void ShowPickup(bool value)
		{
			GetComponent<CapsuleCollider>().enabled = value;

			foreach(Transform child in transform)
			{
				child.gameObject.SetActive(value);
			}
		}
	}
}
