using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPGCourse.Combat
{	
	public class Weapon : MonoBehaviour
	{
		//Config parameters
		[SerializeField] UnityEvent onHit;
		
		public void OnHit()
		{
			onHit.Invoke();
		}
	}
}
