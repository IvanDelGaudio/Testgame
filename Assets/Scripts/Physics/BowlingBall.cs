using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PhysicsTests
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Rigidbody))]
	public class BowlingBall : MonoBehaviour
	{
		#region Public variables
		public float shootForce = 10.0f;
		#endregion
		#region Private variables
		private Rigidbody rb;

		private bool canShoot = false;
		private bool shoot = false;
		private bool wasAsleep = false;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}
		void Start()
		{
			//	Initialize sleep state
			wasAsleep = rb.IsSleeping();
		}
		void Update()
		{
			//	Detect the shoot command
			shoot |= Input.GetKeyDown(KeyCode.Space); //shoot = shoot || Input.GetKeyDown(KeyCode.Space);
		}
		void FixedUpdate()
		{
			//	Prepare sleep check iteration
			bool isAsleep = rb.IsSleeping();

			//	Reset when goes asleep
			if(!wasAsleep && isAsleep)
			{	//	If new asleep but not previously asleep, reset the ball
				rb.MovePosition(Vector3.up * 0.2f);
				rb.MoveRotation(Quaternion.identity);
				rb.velocity = Vector3.zero;

				canShoot = true;
			}

			//	Process shooting logic
			if(canShoot && shoot)
			{
				rb.AddForce(Vector3.forward * shootForce, ForceMode.VelocityChange);
				canShoot = false;
			}

			//	Consume the shoot command
			shoot = false;

			//	Step sleep check
			wasAsleep = isAsleep;
		}
		#endregion
	}
}
