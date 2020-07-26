using System.Collections;
using System.Collections.Generic;
using RPGCourse.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	//Config parameters	
	[SerializeField] float speed = 1;

	//Cache
	Health target = null;

	//States
	float damage = 0;

	private void Update()
	{
		if (target == null) return;

		transform.LookAt(GetAimLocation());
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	private Vector3 GetAimLocation()
	{
		CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();

		if(targetCollider == null) return target.transform.position;

		return target.transform.position + Vector3.up * targetCollider.height / 2;
	}

	public void SetTarget(Health incomingTarget, float weaponDamage)
	{
		target = incomingTarget;
		damage = weaponDamage;
	}

	private void OnTriggerEnter(Collider other) 
	{
		if(other.GetComponent<Health>() != target) return;
		target.TakeDamage(damage);
		Destroy(gameObject);
	}
}

