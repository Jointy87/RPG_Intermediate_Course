using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Stats
{
	public interface IModifierProvider
	{
		IEnumerable<float> FetchAdditiveModifiers(Stat stat);

		IEnumerable<float> FetchPercentageModifiers(Stat stat);
	}

}
