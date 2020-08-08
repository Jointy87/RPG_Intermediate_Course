using System.Collections;
using UnityEngine;

namespace RPGCourse.SceneManagement
{
	public class Fader : MonoBehaviour
	{
		//Config parameter
		[SerializeField] float transitionTime;

		//Cache
		CanvasGroup canvasGroup = null;
		Coroutine activeCoroutine = null; 	

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
			return Fade(1); //not yield return becuase it returns a IEnumerator
		}

		public IEnumerator FadeIn()
		{
			return Fade(0);
		}

		public IEnumerator Fade(float target)
		{
			if(activeCoroutine != null) StopCoroutine(activeCoroutine);
			activeCoroutine = StartCoroutine(FadeRoutine(target)); //Coroutines have returntype coroutine that you can use
			yield return activeCoroutine;
		}

		private IEnumerator FadeRoutine(float target)
		{
			while (!Mathf.Approximately(canvasGroup.alpha, target))
			{
				//mathf.movetowards to get float to desired value using desired speed
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / transitionTime);
				yield return null;
			}
		}
	}
}