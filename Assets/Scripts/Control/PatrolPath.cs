using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Control
{
	public class PatrolPath : MonoBehaviour
	{
		const float waypointGizmoRadius = .2f;

		//Called by Unity
		private void OnDrawGizmos() 
		{
			for(int waypointIndex = 0; waypointIndex < transform.childCount; waypointIndex++)
			{
				int nextWaypointIndex = GetNextIndex(waypointIndex);
				Gizmos.color = Color.red;
				Gizmos.DrawSphere(GetWaypointPosition(waypointIndex), waypointGizmoRadius);
				Gizmos.DrawLine(GetWaypointPosition(waypointIndex), GetWaypointPosition(nextWaypointIndex));
			}
		}

		public int GetNextIndex(int waypointIndex)
		{
			if(waypointIndex < transform.childCount - 1)
			{
				return waypointIndex + 1;
			}
			return 0;
		}

		public Vector3 GetWaypointPosition(int waypointIndex)
		{
			return transform.GetChild(waypointIndex).position;
		}
	}
}
