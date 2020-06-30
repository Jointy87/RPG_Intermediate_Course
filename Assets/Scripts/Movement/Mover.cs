using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGCourse.Core;

namespace RPGCourse.Movement
{
	public class Mover : MonoBehaviour, IAction
	{
		//Config paramters
		[SerializeField] float maxSpeed = 6f;
		
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

		public void StartMoveAction(Vector3 destination, float speedFraction)
		{
			GetComponent<ActionScheduler>().StartAction(this);

			MoveTo(destination, speedFraction);
		}

		public void MoveTo(Vector3 destination, float speedFraction)
		{
			nma.destination = destination;
			nma.isStopped = false;
			nma.speed = maxSpeed * speedFraction *Mathf.Clamp01(speedFraction);
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
