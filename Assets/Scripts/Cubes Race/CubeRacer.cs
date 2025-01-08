using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubesRace
{
	/*
	 * This class defines the controllable entity in the game.
	 * We designed it to be a puppet: it exposes functionality
	 * but doesn't make decisions at all.
	 * This allows for a more flexible management, reducing
	 * the code needed to handle different scenarios.
	 */
	public class CubeRacer : MonoBehaviour
	{
		#region Public variables
		[Tooltip("Speed in m/s")]
		public float minSpeed = 2.0f;
		[Tooltip("Speed in m/s")]
		public float maxSpeed = 5.0f;
		#endregion
		#region Private variables
		private float currentSpeed = 2.5f;
		#endregion
		#region Public methods
		public void Move()
		{
			transform.position += Vector3.forward * currentSpeed * Time.deltaTime;
		}
		public void ResetRacer()
		{
			//	Start at Z = 0
			Vector3 pos = transform.position;
			pos.z = 0.0f;
			transform.position = pos;

			/*
			//	Alternative
			transform.position = new Vector3(transform.position.x, transform.position.y, 0);
			*/

			//	Calcualte random speed
			currentSpeed = Random.Range(minSpeed, maxSpeed);
		}
		#endregion
	}
}
