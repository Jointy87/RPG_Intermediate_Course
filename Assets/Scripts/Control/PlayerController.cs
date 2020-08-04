using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Combat;
using RPGCourse.Resources;

namespace RPGCourse.Control
{
	public class PlayerController : MonoBehaviour
	{	
		//Config parameters
		[SerializeField] CursorMapping[] cursorMappings = null;

		//Cache
		Health health;
		Fighter fighter;

		enum CursorType {None, Movement, Combat}

		[System.Serializable]
		struct CursorMapping
		{
			public CursorType type;
			public Texture2D texture;
			public Vector2 hotspot;
		}

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

			SetCursor(CursorType.None);
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
				SetCursor(CursorType.Combat);
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
				SetCursor(CursorType.Movement);
				return true;
			}
			return false;
		}

		private static Ray GetMouseRay()
		{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}

		private void SetCursor(CursorType type)
		{
			CursorMapping mapping = GetCursorMapping(type);
			Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
		}

		private CursorMapping GetCursorMapping(CursorType type)
		{
			foreach(CursorMapping mapping in cursorMappings)
			{
				if(mapping.type == type) return mapping;
			}
			return cursorMappings[0];
		}
	}

}