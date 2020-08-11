using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGCourse.Core;
using RPGCourse.Saving;
using RPGCourse.Attributes;

namespace RPGCourse.Movement
{
	public class Mover : MonoBehaviour, IAction, ISaveable
	{
		//Config paramters
		[SerializeField] float maxSpeed = 6f;
		[SerializeField] float maxPathDistance = 25f;
		
		//Cache
		NavMeshAgent nma;
		Health health;
		Animator animator;

		private void Awake() 
		{
			nma = GetComponent<NavMeshAgent>();
			health = GetComponent<Health>();
			animator = GetComponent<Animator>();
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

		public bool CanMoveTo(Vector3 destination)
		{
			NavMeshPath path = new NavMeshPath();
			bool hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
			if (!hasPath || path.status != NavMeshPathStatus.PathComplete) return false;

			if (GetPathLength(path) > maxPathDistance) return false;

			return true;
		}

		private float GetPathLength(NavMeshPath path)
		{
			float totalDistance = 0;

			if (path.corners.Length < 2) return totalDistance;

			for (int corner = 0; corner < path.corners.Length - 1; corner++)
			{
				totalDistance = Vector3.Distance(path.corners[corner], path.corners[corner + 1]);
			}
			return totalDistance;
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
			animator.SetFloat("forwardSpeed", speed);
		}

		public void Cancel()
		{
			nma.isStopped = true;
		}

		public object CaptureState() //object is parent class of all objects, means you can return anything
		{
			return new SerializableVector3(transform.position);
		}

		public void RestoreState(object state)
		{
			NavMeshAgent nma = GetComponent<NavMeshAgent>(); //need this because this method is called between awake and start
			SerializableVector3 position = (SerializableVector3)state; //we're casting here
			nma.enabled = true;
			nma.Warp(position.ToVector());
		}
	}
}
