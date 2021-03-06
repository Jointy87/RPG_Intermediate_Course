﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Attributes;
using UnityEngine.Events;

namespace RPGCourse.Combat
{
	public class Projectile : MonoBehaviour
	{
		//Config parameters	
		[SerializeField] float speed = 1;
		[SerializeField] bool isHoming = false;
		[SerializeField] GameObject hitVFX = null;
		[SerializeField] float maxLifeTime = 10;
		[SerializeField] GameObject[] destroyOnHit = null;
		[SerializeField] float lifeAfterImpact = 1;
		[SerializeField] UnityEvent playHitAudio;

		//Cache
		Health target = null;

		//States
		float damage = 0;
		GameObject instigator = null;

		private void Start() 
		{
			if(target == null) return;
			transform.LookAt(GetAimLocation());
			Destroy(gameObject, maxLifeTime);
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

		public void SetTarget(Health incomingTarget, float weaponDamage, GameObject instigator)
		{
			target = incomingTarget;
			damage = weaponDamage;
			this.instigator = instigator;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.GetComponent<Health>() != target || !target.IsAlive()) return;
			speed = 0;
			target.TakeDamage(instigator, damage);
			playHitAudio.Invoke();

			if(hitVFX != null)
			{
				GameObject impactVFX = Instantiate(hitVFX, transform.position, transform.rotation);
			} 

			foreach(GameObject toDestroy in destroyOnHit)
			{
				Destroy(toDestroy);
			}
			Destroy(gameObject, lifeAfterImpact);
		}
	}

}
