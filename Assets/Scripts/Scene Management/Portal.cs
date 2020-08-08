using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using RPGCourse.Control;

namespace RPGCourse.SceneManagement
{
	public class Portal : MonoBehaviour
	{
		enum DestinationIdentifier { A, B }
		
		//Config paramters
		[SerializeField] int sceneIndex = -1;
		[SerializeField] Transform spawnPoint;
		[SerializeField] DestinationIdentifier destination;

		private void OnTriggerEnter(Collider other) 
		{
			if(other.tag == "Player")
			{
				StartCoroutine(SceneTransition());
			}
		}

		//For debugging race conditions, check yield returns. Is there something the player could do during that time that could trigger a race condition
		private IEnumerator SceneTransition()
		{
			if (sceneIndex < 0)
			{
				Debug.LogError("sceneIndex is not set");
				yield break;
			}

			Fader fader = FindObjectOfType<Fader>();
						
			transform.parent = null;
			DontDestroyOnLoad(gameObject);

			GameObject.FindWithTag("Player").GetComponent<PlayerController>().hasControl = false;
			yield return fader.FadeOut();

			SavingWrapper wrapper =  FindObjectOfType<SavingWrapper>();
			wrapper.Save();

			yield return SceneManager.LoadSceneAsync(sceneIndex); //Waits till after scene has l
			GameObject.FindWithTag("Player").GetComponent<PlayerController>().hasControl = false; //Disable again because new version of player is loaded

			wrapper.Load();

			Portal otherPortal = GetOtherPortal();
			UpdatePlayerLocation(otherPortal);

			wrapper.Save(); //so player location isn't by previous portal, which right away makes you go through portal again

			yield return new WaitForSeconds(.5f);
			fader.FadeIn(); 	//FadeIn is coroutine instead of IEnumerator, so runs independently without having to wait on it
			GameObject.FindWithTag("Player").GetComponent<PlayerController>().hasControl = true;

			Destroy(gameObject);
		}

		private Portal GetOtherPortal()
		{
			Portal[] portals = FindObjectsOfType<Portal>();

			foreach (Portal portal in FindObjectsOfType<Portal>())
			{
				if (portal == this) continue;
				if (destination != portal.destination) continue;
				return portal;
			}
			return null;
		}

		private void UpdatePlayerLocation(Portal otherPortal)
		{
			GameObject player = GameObject.FindWithTag("Player");
			player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
			player.transform.rotation = otherPortal.spawnPoint.rotation;
		}
	}
}
