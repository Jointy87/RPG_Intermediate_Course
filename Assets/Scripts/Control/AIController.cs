using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Combat;
using RPGCourse.Core;
using RPGCourse.Movement;
using RPGCourse.Saving;
using RPGCourse.Attributes;
using GameDevTV.Utils;
using UnityEngine.Events;
using System;

namespace RPGCourse.Control
{
	public class AIController : MonoBehaviour, ISaveable
	{
		//Config parameters
		[Tooltip("Size of radius within which player will be followed")]
		[SerializeField] float chaseDistance = 5f;
		[Tooltip("Time in s spent on player's last seen location")]
		[SerializeField] float suspicionTime = 10f;
		[Tooltip("Patrolpath gameObject, if used")]
		[SerializeField] PatrolPath patrolPath = null;
		[Tooltip("Range tolerance for detecting a waypoint")]
		[SerializeField] float waypointTolerance = .5f;
		[Tooltip("Time in s spent at a waypoint before moving on")]
		[SerializeField] float waypointDwellTime = 1f;
		[Range(0,1)][Tooltip("Max speed divided by this amount while patrolling")]
		[SerializeField] float patrolSpeedFraction = .3f;
		[SerializeField] float maxAggroTime = 5f;
		[SerializeField] float mobAggroRadius = 5f;

		//Cache
		GameObject player;
		Fighter fighter;
		Health health;
		Mover mover;
		
		//States
		LazyValue<Vector3> guardPosition;
		float timeSinceLastSawPlayer = Mathf.Infinity;
		float timeDwelledAtWaypoint = Mathf.Infinity;
		float timeSinceAggro = Mathf.Infinity;
		int waypointIndex = 0;
		Vector3 nextPosition;


		void Awake() 
		{
			fighter = GetComponent<Fighter>();
			player = GameObject.FindWithTag("Player");
			health = GetComponent<Health>();
			mover = GetComponent<Mover>();
			guardPosition = new LazyValue<Vector3>(SetInitialGuardPosition);
		}

		private void Start() 
		{
			guardPosition.ForceInit();
		}

		void Update()
		{
			if (!health.IsAlive()) return;

			UpdateTimers();
			ChaseAndAttackPlayer();
		}

		private Vector3 SetInitialGuardPosition()
		{
			return transform.position;
		}

		private void UpdateTimers()
		{
			timeSinceLastSawPlayer += Time.deltaTime;
			timeDwelledAtWaypoint += Time.deltaTime;
			timeSinceAggro += Time.deltaTime;
		}

		public void Aggrevate() //Called from event
		{
			timeSinceAggro = 0;
		}

		private void ChaseAndAttackPlayer()
		{
			if (IsAggrevated() && fighter.CanAttack(player))
			{
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
			timeSinceLastSawPlayer = 0;
			fighter.Attack(player);

			AggrevateNearbyEnemies();
		}

		private void AggrevateNearbyEnemies()
		{
			RaycastHit[] hits = 
				Physics.SphereCastAll(transform.position, mobAggroRadius, Vector3.up, 0);

			foreach (RaycastHit hit in hits)
			{
				AIController enemyAI = hit.collider.GetComponent<AIController>();
				if(!enemyAI) continue;
				enemyAI.Aggrevate();
			}
		}

		private void SuspicionBehaviour()
		{
			GetComponent<ActionScheduler>().CancelCurrentAction();
		}
		
		private void PatrolBehaviour()
		{
			nextPosition = guardPosition.value;

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

		private bool IsAggrevated()
		{
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			return distanceToPlayer <= chaseDistance || timeSinceAggro <= maxAggroTime;
		}

		//Called by Unity
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, chaseDistance);
		}

		[System.Serializable]
		struct AICSaveStruct
		{
			public float savedWaypointDwellTime;
			public float savedTimeSinceLastSawPlayer;
			public SerializableVector3 savedNextPosition;
		}

		public object CaptureState()
		{
			AICSaveStruct savingStruct = new AICSaveStruct();

			savingStruct.savedWaypointDwellTime = waypointDwellTime;
			savingStruct.savedTimeSinceLastSawPlayer = timeSinceLastSawPlayer;
			savingStruct.savedNextPosition = new SerializableVector3(nextPosition);
			return savingStruct;
		}

		public void RestoreState(object state)
		{
			AICSaveStruct savingStruct = (AICSaveStruct)state;
			
			waypointDwellTime = savingStruct.savedWaypointDwellTime;
			nextPosition = savingStruct.savedNextPosition.ToVector();

			if(timeSinceLastSawPlayer <= Mathf.Epsilon)
			{
				timeSinceLastSawPlayer = Mathf.Infinity;
			}
			else
			{
				timeSinceLastSawPlayer = savingStruct.savedTimeSinceLastSawPlayer;
			}
		}
	}
}
