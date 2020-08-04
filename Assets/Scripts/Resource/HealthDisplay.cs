using RPGCourse.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCourse.Resources
{

	public class HealthDisplay : MonoBehaviour
	{
		//Config parameters
		[SerializeField] bool isPlayer = false;

		//Cache
		Health health;
		Fighter fighter;
		Text healthText;

		private void Awake() 
		{
			health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
			fighter = GameObject.FindGameObjectWithTag("Player").GetComponent <Fighter>();
			healthText = GetComponent<Text>();
		}

		private void Update() 
		{
			if(isPlayer)
			{
				healthText.text = string.Format("{0:0}/{1:0}", health.FetchHealth(), health.FetchMaxHealth());
			}
			else
			{
				if (fighter.FetchTarget() == null)
				{
					healthText.text = "N/A";
					return;
				} 
				
				healthText.text = string.Format("{0:0}/{1:0}", 
					fighter.FetchTarget().FetchHealth(), fighter.FetchTarget().FetchMaxHealth());
			}
			
		}
	}
}