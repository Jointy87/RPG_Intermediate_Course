using System.Collections;
using System.Collections.Generic;
using RPGCourse.Resources;
using UnityEngine;

namespace RPGCourse.UI.DamageText
{
	public class DamageTextSpawner : MonoBehaviour
	{
		[SerializeField] DamageText dmgTextPrefab = null;

		public void SpawnDamageText(float damage, Color color)
		{
			var spawnedText = Instantiate(dmgTextPrefab, transform);
			spawnedText.SetText(damage, color);
			Destroy(spawnedText, 1f);
		}
	}
}
