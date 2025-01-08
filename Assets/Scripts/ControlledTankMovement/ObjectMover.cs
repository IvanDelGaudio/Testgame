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

namespace ControlledTankMovement
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CharacterController))]
	public class ObjectMover : MonoBehaviour
	{
		#region Private variables
		private CharacterController cc = null;
		[SerializeField]
		private float moveSpeed = 2.0f;
		[SerializeField]
		private float rotationSpeed = 180.0f;
		#endregion
		#region Lifecycle
		void Awake()
		{
			cc = GetComponent<CharacterController>();
		}
		#endregion
		#region Public methods
		public void Move(float amount)
		{
			cc.Move(amount * moveSpeed * transform.forward * Time.deltaTime);
		}
		public void Turn(float direction)
		{
			transform.rotation *= Quaternion.AngleAxis(direction * rotationSpeed * Time.deltaTime, transform.up);
		}
		#endregion
	}
}
