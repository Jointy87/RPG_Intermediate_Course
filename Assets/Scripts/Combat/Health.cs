using UnityEngine;

namespace RPGCourse.Combat
{
	public class Health : MonoBehaviour
	{
		[SerializeField] float health = 100f;

		public void TakeDamage(float damage)
		{
			health = Mathf.Max(health - damage, 0); // takes highest value, in this case either health - damage, or 0
		}
	}
}
