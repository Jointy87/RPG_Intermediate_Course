using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Combat;
using RPGCourse.Core;

namespace RPGCourse.Control
{
	public class AIController : MonoBehaviour
	{
		//Config parameters
		[SerializeField] float chaseDistance = 5f;

		//Cache
		GameObject player;
		Fighter fighter;
		Health health;

		void Start() 
		{
			fighter = GetComponent<Fighter>();
			player = GameObject.FindWithTag("Player");
			health = GetComponent<Health>();
		}

		void Update()
		{
			if (!health.IsAlive()) return;
			
			ChaseAndAttackPlayer();

		}

		private void ChaseAndAttackPlayer()
		{
			if (InChaseDistance() && fighter.CanAttack(player))
			{
				fighter.Attack(player);
			}
			else
			{
				fighter.Cancel();
			}
		}

		private bool InChaseDistance()
		{
			float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
			return distanceToPlayer <= chaseDistance;
		}
	}
}
