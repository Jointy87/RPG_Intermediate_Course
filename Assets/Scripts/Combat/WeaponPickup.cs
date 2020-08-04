using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Control;

namespace RPGCourse.Combat
{
	public class WeaponPickup : MonoBehaviour, IRaycastable
	{
		//Config parameters
		[SerializeField] Weapon weaponType = null;
		[SerializeField] float respawnTime = 5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				PickUp(other.GetComponent<Fighter>());
			}
		}

		private void PickUp(Fighter fighter)
		{
			fighter.EquipWeapon(weaponType);
			StartCoroutine(HideForSeconds(respawnTime));
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

		public bool HandleRaycast(PlayerController callingController)
		{	
			if(Input.GetMouseButtonDown(0))
			{
				PickUp(callingController.GetComponent<Fighter>());
			}
			return true;
		}

		public CursorType GetCursorType()
		{
			return CursorType.Pickup;
		}
	}
}
