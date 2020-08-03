using UnityEngine;
using System;
using System.Collections;
using RPGCourse.Saving;


namespace RPGCourse.SceneManagement
{
	public class SavingWrapper : MonoBehaviour
	{
		const string defaultSaveFile = "save";

		private void Awake() 
		{
			StartCoroutine(LoadLastScene());
		}

		IEnumerator LoadLastScene()
		{
			yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
			Fader fader = FindObjectOfType<Fader>();
			fader.FadeOutImmediate();
			yield return fader.FadeIn();
		}

		void Update() 
		{
			if(Input.GetKeyDown(KeyCode.S))
			{
				Save();
			}
			
			if(Input.GetKeyDown(KeyCode.L))
			{
				Load();
			}

			if(Input.GetKeyDown(KeyCode.Delete))
			{
				Delete();
			}

		}

		public void Save()
		{
			GetComponent<SavingSystem>().Save(defaultSaveFile);
		}

		public void Load()
		{
			GetComponent<SavingSystem>().Load(defaultSaveFile);
		}

		public void Delete()
		{
			GetComponent<SavingSystem>().Delete(defaultSaveFile);
		}
	}
}