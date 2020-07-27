using System.Collections;
using System.Collections.Generic;
using RPGCourse.Core;
using UnityEngine;

namespace RPGCourse.Combat
{
	public class Projectile : MonoBehaviour
	{
		//Config parameters	
		[SerializeField] float speed = 1;
		[SerializeField] bool isHoming = false;
		[SerializeField] GameObject hitVFX = null;

		//Cache
		Health target = null;

		//States
		float damage = 0;

		private void Start() 
		{
			if(target == null) return;
			transform.LookAt(GetAimLocation());
			Destroy(gameObject, 5f);
		}

		private void Update()
		{
			if (target == null) return;
			if(isHoming && target.IsAlive()) transform.LookAt(GetAimLocation());
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

		private Vector3 GetAimLocation()
		{
			CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();

			if (targetCollider == null) return target.transform.position;

			return target.transform.position + Vector3.up * targetCollider.height / 2;
		}

		public void SetTarget(Health incomingTarget, float weaponDamage)
		{
			target = incomingTarget;
			damage = weaponDamage;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Health>() != target || !target.IsAlive()) return;
			target.TakeDamage(damage);
			if(hitVFX != null)
			{
				GameObject impactVFX = Instantiate(hitVFX, transform.position, transform.rotation);
			} 
			Destroy(gameObject);
		}
	}

}
