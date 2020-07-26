using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	//Config parameters
	[SerializeField] Transform target = null;
	[SerializeField] float speed = 1;

	private void Update()
	{
		if (target == null) return;

		transform.LookAt(GetAimLocation());
		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

	private Vector3 GetAimLocation()
	{
		CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();

		if(targetCollider == null) return target.position;
		
		return target.position + Vector3.up * targetCollider.height / 2;
	}
}

