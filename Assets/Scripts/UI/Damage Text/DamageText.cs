using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPGCourse.UI.DamageText
{
	public class DamageText : MonoBehaviour
	{
		[SerializeField] Text damageText = null;

		public void SetText(float damage, Color color)
		{
			damageText.color = color;
			damageText.text = string.Format("{0:0}", damage);
		}
	}
}

