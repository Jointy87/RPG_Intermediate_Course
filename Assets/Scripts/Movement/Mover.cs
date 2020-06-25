using UnityEngine;
using UnityEngine.AI;
using RPGCourse.Combat;
using RPGCourse.Core;

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

		public void StartMoveAction(Vector3 destination)
		{
			GetComponent<ActionScheduler>().StartAction(this);
			GetComponent<Fighter>().CancelAttack();
			nma.isStopped = false;
			MoveTo(destination);
		}

		public void MoveTo(Vector3 destination)
		{
			nma.destination = destination;
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
