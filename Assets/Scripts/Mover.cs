using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
	//Cache
	NavMeshAgent nma;

	void Start()
	{
		nma = GetComponent<NavMeshAgent>();
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			MoveToCursor();
		}
	}

	private void MoveToCursor()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		bool hasHit = Physics.Raycast(ray, out hit);

		if(hasHit)
		{
			if (!nma) { return; }

            else
            {
                nma.destination = hit.point;
            }
		}
	}
}
