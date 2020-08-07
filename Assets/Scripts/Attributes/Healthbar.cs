using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCourse.Attributes
{
	public class Healthbar : MonoBehaviour
	{
		//Config parameters
		[SerializeField] Health health = null;
		[SerializeField] Image healthBar = null;
		[SerializeField] Canvas rootCanvas = null;

		private void Update()
		{
			ScaleBar();
		}

		private void ScaleBar()
		{
			float fraction = health.FetchHealth() / health.FetchMaxHealth();

			healthBar.transform.localScale = new Vector3(fraction, 1, 1);

			if(Mathf.Approximately(fraction, 0) || Mathf.Approximately(fraction, 1) ) 
				rootCanvas.enabled = false;
			else rootCanvas.enabled = true;
		}
	}
}
