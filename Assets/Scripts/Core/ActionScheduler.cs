using UnityEngine;

namespace RPGCourse.Core
{
	public class ActionScheduler : MonoBehaviour
	{
		//Cache
		MonoBehaviour previousAction;
		MonoBehaviour currentAction;

		public void StartAction(MonoBehaviour action)
		{
			if (currentAction == action) return;
			
			if (currentAction != null)
			{
				print("Cancelling " + currentAction);
			}
			currentAction = action;
		}
	}
}

