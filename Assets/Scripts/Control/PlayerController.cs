using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCourse.Movement;
using RPGCourse.Combat;
using RPGCourse.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

namespace RPGCourse.Control
{
	public class PlayerController : MonoBehaviour
	{	
		//Config parameters
		[SerializeField] CursorMapping[] cursorMappings = null;
		[SerializeField] float maxPathDistance = 25f;

		//Cache
		Health health;
		Fighter fighter;

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
			if(InterfaceInteraction()) return;
			
			if(!health.IsAlive())
			{
				SetCursor(CursorType.None);
				return;
			} 

			if(ComponentInteraction()) return;
			if(MovementInteraction()) return;

			SetCursor(CursorType.None);
		}

		private bool InterfaceInteraction()
		{
			if(EventSystem.current.IsPointerOverGameObject()) 
			{
				SetCursor(CursorType.UI);
				return true;
			}
			else return false;
		}

		private bool ComponentInteraction()
		{
			RaycastHit[] hits = SortedRaycasts();
			foreach (RaycastHit hit in hits)
			{
				IRaycastable[] raycastables =  hit.transform.GetComponents<IRaycastable>();
				
				foreach(IRaycastable raycastable in raycastables)
				{
					if(raycastable.HandleRaycast(this))
					{
						SetCursor(raycastable.GetCursorType());
						return true;
					}
				}
			}
			return false;
		}

		private RaycastHit[] SortedRaycasts()
		{
			RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

			float[] distances = new float[hits.Length];

			for (int hit = 0; hit < hits.Length; hit++)
			{
				distances[hit] = hits[hit].distance;
			}

			Array.Sort(distances, hits);
			return hits;
		}

		private bool MovementInteraction()
		{
			Vector3 target;
			bool hasHit = RayCastNavMesh(out target);

			if(hasHit)
			{
				if (Input.GetMouseButton(0))
				{
					GetComponent<Mover>().StartMoveAction(target, 1f);
				} 
				SetCursor(CursorType.Movement);
				return true;
			}
			return false;
		}

		private bool RayCastNavMesh(out Vector3 target)
		{
			target = new Vector3();

			RaycastHit hit;
			bool hasRayHit = Physics.Raycast(GetMouseRay(), out hit);
			if(!hasRayHit) return false;

			NavMeshHit navMeshHit;
			if(!NavMesh.SamplePosition
				(hit.point, out navMeshHit, .3f, NavMesh.AllAreas)) return false;

			target = navMeshHit.position;

			NavMeshPath path = new NavMeshPath();
			bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
			if(!hasPath || path.status != NavMeshPathStatus.PathComplete) return false;

			if(GetPathLength(path) > maxPathDistance) return false;
			
			return true;
		}

		private float GetPathLength(NavMeshPath path)
		{
			float totalDistance = 0;

			if(path.corners.Length < 2) return totalDistance;

			for (int corner = 0; corner < path.corners.Length - 1; corner++)
			{
				totalDistance = Vector3.Distance(path.corners[corner], path.corners[corner + 1]);
			}
			return totalDistance;
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