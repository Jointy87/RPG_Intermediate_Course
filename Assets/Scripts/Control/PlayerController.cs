using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Combat;
using RPGCourse.Core;

namespace RPGCourse.Control
{
	public class PlayerController : MonoBehaviour
	{	
		//Cache
		Health health;
		Fighter fighter;

		void Awake() 
		{
			health = GetComponent<Health>();
			fighter = GetComponent<Fighter>();
		}

		void Update()
		{
			if (!health.IsAlive()) return;

			if (ControlCombat()) return;
			else if (ControlMovement()) return;
		}

		private bool ControlCombat()
		{
			RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
			foreach (RaycastHit hit in hits)
			{
				CombatTarget target = hit.transform.GetComponent<CombatTarget>();
				if(!target) continue;

				if(!fighter.CanAttack(target.gameObject)) continue;

				if (Input.GetMouseButtonDown(0))
				{
					fighter.Attack(target.gameObject);
				}
				return true;
			}
			return false;
		}

		private bool ControlMovement()
		{
			RaycastHit hit;
			bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

			if(hasHit)
			{
				if (Input.GetMouseButton(0))
				{
					GetComponent<Mover>().StartMoveAction(hit.point, 1f);
				} 
				return true;
			}
			return false;
		}

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}

}