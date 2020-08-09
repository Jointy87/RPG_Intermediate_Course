using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCourse.Core
{
	[RequireComponent(typeof(AudioSource))]
	public class AudioRandomizer : MonoBehaviour
	{
		//Config parameters
		[SerializeField] AudioClip[] clips;

		//Cache
		AudioSource source;

		private void Start() 
		{
			source = GetComponent<AudioSource>();
		}

		public void PlayClip()
		{
			int index = Random.Range(0, clips.Length -1);

			source.clip = clips[index];
			source.Play();

		}
	}
}
