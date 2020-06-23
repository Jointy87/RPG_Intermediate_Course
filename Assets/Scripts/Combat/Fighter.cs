using UnityEngine;

namespace RPGCourse.Combat
{
	public class Fighter : MonoBehaviour
	{
		public void Attack(CombatTarget target)
		{
			print("Attacking " + target.name); 
		}
	}
}