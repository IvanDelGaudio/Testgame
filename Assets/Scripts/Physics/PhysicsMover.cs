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
	public class PhysicsMover : MonoBehaviour
	{
		#region Public variables
		public float force = 10.0f;
		public float impulse = 10.0f;
		public Vector3 torque = Vector3.zero;
		#endregion
		#region Private variables
		private Rigidbody rb = null;

		bool doImpulse = false;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}
		void Update()
		{
			doImpulse |= Input.GetKeyDown(KeyCode.Space);
		}
		void FixedUpdate()
		{
			rb.AddForce(Vector3.forward * force, ForceMode.Force);

			if(doImpulse)
			{
				doImpulse = false;
				rb.AddForce(Vector3.up * impulse, ForceMode.Impulse);
			}

			rb.AddTorque(torque);
		}
		#endregion
	}
}
