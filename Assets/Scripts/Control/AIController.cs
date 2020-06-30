using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPGCourse.Combat;
using RPGCourse.Core;
using RPGCourse.Movement;
using System;

namespace RPGCourse.Control
{
	public class AIController : MonoBehaviour
	{
		//Config parameters
		[Tooltip("Size of radius within which player will be followed")]
		[SerializeField] float chaseDistance = 5f;
		[Tooltip("Time in s spent on player's last seen location")]
		[SerializeField] float suspicionTime = 10f;
		[Tooltip("Patrolpath gameObject, if used")]
		[SerializeField] PatrolPath patrolPath;
		[Tooltip("Range tolerance for detecting a waypoint")]
		[SerializeField] float waypointTolerance = .5f;
		[Tooltip("Time in s spent at a waypoint before moving on")]
		[SerializeField] float waypointDwellTime = 1f;
		[Range(0,1)][Tooltip("Max speed divided by this amount while patrolling")]
		[SerializeField] float patrolSpeedFraction = .3f;

		//Cache
		GameObject player;
		Fighter fighter;
		Health health;
		Mover mover;
		
		//States
		Vector3 guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeDwelledAtWaypoint = Mathf.Infinity;	
		int waypointIndex = 0;

		void Start() 
		{
			fighter = GetComponent<Fighter>();
			player = GameObject.FindWithTag("Player");
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();

			guardPosition = transform.position;
		}

		void Update()
		{
			if (!health.IsAlive()) return;

			UpdateTimers();
			ChaseAndAttackPlayer();
		}

		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeDwelledAtWaypoint += Time.deltaTime;
		}

		private void ChaseAndAttackPlayer()
		{
			if (InChaseDistance() && fighter.CanAttack(player))
			{
				timeSinceLastSawPlayer = 0;
				AttackBehaviour();
			}
			else if (timeSinceLastSawPlayer <= suspicionTime)
			{
				SuspicionBehaviour();
			}
			else
			{
				PatrolBehaviour();
			}
		}
		private void AttackBehaviour()
		{
			fighter.Attack(player);
		}

		private void SuspicionBehaviour()
		{
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}
		
		private void PatrolBehaviour()
		{
			Vector3 nextPosition = guardPosition;

			if(patrolPath != null)
			{
				if(AtWaypoint())
				{
					timeDwelledAtWaypoint = 0;
					CycleWaypoint();
				}

				nextPosition = GetCurrentWaypoint();
			}
			
			if (timeDwelledAtWaypoint >= waypointDwellTime)
			{
				mover.StartMoveAction(nextPosition, patrolSpeedFraction);
			}
		}

		private bool AtWaypoint()
		{
			float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
			return distanceToWaypoint < waypointTolerance;
		}

		private void CycleWaypoint()
		{
			waypointIndex = patrolPath.GetNextIndex(waypointIndex);
		}

		private Vector3 GetCurrentWaypoint()
		{
			return patrolPath.GetWaypointPosition(waypointIndex);
		}

		private bool InChaseDistance()
		{
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			return distanceToPlayer <= chaseDistance;
		}

		//Called by Unity
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}
	}
}
