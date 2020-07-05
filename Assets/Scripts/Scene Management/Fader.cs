using System.Collections;
using UnityEngine;

namespace RPGCourse.SceneManagement
{
	public class Fader : MonoBehaviour
	{
		//Config parameter
		[SerializeField] float transitionTime;

		//Cache
		CanvasGroup canvasGroup;

		void Awake() 
		{
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public void FadeOutImmediate()
		{
			canvasGroup.alpha = 1;
		}

		public IEnumerator FadeOut()
		{
			while (canvasGroup.alpha < 1) 
			{
				canvasGroup.alpha += Time.deltaTime / transitionTime;
				yield return null;
			}
		}
		public IEnumerator FadeIn()
		{
			while (canvasGroup.alpha > 0)
			{
				canvasGroup.alpha -= Time.deltaTime / transitionTime;
				yield return null;
			}
		}
	}
}