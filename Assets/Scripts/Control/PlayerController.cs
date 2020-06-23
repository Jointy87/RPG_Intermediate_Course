using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Combat;

namespace RPGCourse.Control
{
	public class PlayerController : MonoBehaviour
	{
		void Update()
		{
			if(ControlCombat()) return;
			else if(ControlMovement()) return;
			print("Nothing to do dawg");
		}

		private bool ControlCombat()
		{
			RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
			foreach (RaycastHit hit in hits)
			{
				CombatTarget target = hit.transform.GetComponent<CombatTarget>();
				if(target == null) continue;

				if (Input.GetMouseButtonDown(0))
				{
					GetComponent<Fighter>().Attack(target);
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
					GetComponent<Mover>().MoveTo(hit.point);
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