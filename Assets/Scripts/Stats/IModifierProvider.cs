using System.Collections.Generic;

namespace RPGCourse.Stats
{
	public interface IModifierProvider
	{
		IEnumerable<float> FetchAdditiveModifiers(Stat stat);

		IEnumerable<float> FetchPercentageModifiers(Stat stat);
	}

}
