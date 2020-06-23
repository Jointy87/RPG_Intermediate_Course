﻿using UnityEngine;
using UnityEngine.AI;

namespace RPGCourse.Movement
{
	public class Mover : MonoBehaviour
	{
		//Cache
		NavMeshAgent nma;

		private void Start() 
		{
			nma = GetComponent<NavMeshAgent>();
		}

		void Update()
		{
			UpdateAnimator();
		}

		public void MoveTo(Vector3 destination)
		{
			nma.destination = destination;
            nma.isStopped = false;
		}

		public void StopMoving()
		{
			nma.isStopped = true;
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = nma.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);  //converts global to local velocity
			float speed = localVelocity.z;
			GetComponent<Animator>().SetFloat("forwardSpeed", speed);
		}
	}
}
