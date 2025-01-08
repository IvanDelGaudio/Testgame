using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Misc
{
	[AddComponentMenu("DBGA/Test")]
	public class Test : MonoBehaviour
	{
		[SerializeField]
		private Vector3 offset = Vector3.forward;
		[SerializeField]
		private Transform target;

		void Awake()
		{
			Vector3 pos = target.position;
			pos += offset;
			target.position = pos;

			/*	LOGGING	*/
			Debug.Log("Ciao");
			Debug.LogWarning("Ciao");
			Debug.LogError("Ciao");
			/*	=======	*/
		}
	}
}
