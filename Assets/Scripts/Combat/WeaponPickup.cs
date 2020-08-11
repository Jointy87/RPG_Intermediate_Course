using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Control;
using RPGCourse.Attributes;

namespace RPGCourse.Combat
{
	public class WeaponPickup : MonoBehaviour, IRaycastable
	{
		//Config parameters
		[SerializeField] WeaponConfig weaponType = null;
		[SerializeField] float healthToRestore = 0;
		[SerializeField] float respawnTime = 5f;

		private void OnTriggerEnter(Collider other)
		{
			if (other.tag == "Player")
			{
				PickUp(other.gameObject);
			}
		}

		private void PickUp(GameObject character)
		{
			if(weaponType != null) character.GetComponent<Fighter>().EquipWeapon(weaponType);
			if(healthToRestore > 0) character.GetComponent<Health>().RestoreHealth(healthToRestore);
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
				PickUp(callingController.gameObject);
			}
			return true;
		}

		public CursorType GetCursorType()
		{
			return CursorType.Pickup;
		}
	}
}
