using UnityEngine;
using UnityEngine.UI;

namespace RPGCourse.Resources
{

	public class HealthDisplay : MonoBehaviour
	{
		//Cache
		Health health;
		Text healthText;

		private void Awake() 
		{
			health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
			healthText = GetComponent<Text>();
		}

		private void Update() 
		{
			healthText.text = string.Format("{0:0}%", health.FetchHealthPercentage().ToString());
		}
	}
}