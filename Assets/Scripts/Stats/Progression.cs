using UnityEngine;

namespace RPGCourse.Stats
{
	[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
	public class Progression : ScriptableObject
	{
		[System.Serializable]
		class ProgressionCharacerClass
		{
			[SerializeField] CharacterClass characterClass;
			[SerializeField] float[] health;
			[SerializeField] float[] exp;

			public CharacterClass FetchCharacterClass()
			{
				return characterClass;
			}

			public float FetchHealth(int value)
			{
				return health[value];
			}

			public float FetchExperience(int value)
			{
				return exp[value];
			}
		}

		[SerializeField] ProgressionCharacerClass[] characterClasses;

		public float FetchHealth(CharacterClass incomingClass, int level)
		{
			foreach(ProgressionCharacerClass character in characterClasses)
			{
				if(character.FetchCharacterClass() != incomingClass) continue;

				float healthValue = character.FetchHealth(level - 1);
				return healthValue;
			}
			return 0;
		}

		public float FetchExperience(CharacterClass incomingClass, int level)
		{
			foreach (ProgressionCharacerClass character in characterClasses)
			{
				if (character.FetchCharacterClass() != incomingClass) continue;

				float expValue = character.FetchExperience(level - 1);
				return expValue;
			}
			return 0;
		}
	}
}
