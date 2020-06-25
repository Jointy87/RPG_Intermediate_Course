using UnityEngine;

namespace RPGCourse.Core
{
	public class ActionScheduler : MonoBehaviour
	{
		//Cache
		IAction currentAction;
		IAction previousAction;

		public void StartAction(IAction action)
		{
			if (currentAction == action) return;
			
			if (currentAction != null)
			{
				currentAction.Cancel();
			}
			currentAction = action;
		}
	}
}

