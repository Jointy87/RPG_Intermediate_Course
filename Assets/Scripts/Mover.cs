using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
	//Config parameters
	[SerializeField] GameObject target;

	//Cache
	NavMeshAgent nma;
	void Start()
	{
		nma = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		MoveToTarget();
	}

	private void MoveToTarget()
	{
		if (!nma) { return; }

		else
		{
			nma.destination = target.transform.position;
		}
	}
}
