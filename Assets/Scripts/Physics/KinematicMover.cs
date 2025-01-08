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
	public class KinematicMover : MonoBehaviour
	{
		#region Public variables
		public Transform target;
		#endregion
		#region Private variables
		private Rigidbody rb = null;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}
		void FixedUpdate()
		{
			//rb.position = Vector3.zero;
			//rb.rotation = Quaternion.identity;

			rb.MovePosition(target.position);
			rb.MoveRotation(target.rotation);
		}
		#endregion
	}
}
