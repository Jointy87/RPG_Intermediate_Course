﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGCourse.Core;

namespace RPGCourse.Movement
{
	public class Mover : MonoBehaviour, IAction
	{
		//Cache
		NavMeshAgent nma;
		Health health;

		private void Start() 
		{
			nma = GetComponent<NavMeshAgent>();
			health = GetComponent<Health>();
		}

		void Update()
		{
			nma.enabled = health.IsAlive();
			UpdateAnimator();
		}

		public void StartMoveAction(Vector3 destination)
		{
			GetComponent<ActionScheduler>().StartAction(this);

			MoveTo(destination);
		}

		public void MoveTo(Vector3 destination)
		{
			nma.destination = destination;
			nma.isStopped = false;
		}

		private void UpdateAnimator()
		{
			Vector3 velocity = nma.velocity;
			Vector3 localVelocity = transform.InverseTransformDirection(velocity);  //converts global to local velocity
			float speed = localVelocity.z;
			GetComponent<Animator>().SetFloat("forwardSpeed", speed);
		}

		public void Cancel()
		{
			nma.isStopped = true;
		}
	}
}
