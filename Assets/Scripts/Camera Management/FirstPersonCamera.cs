using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManagement
{
	/*
	 * First person is a simple camera which only handles up-down
	 * look, as movement and left-right rotation are directly handled
	 * by the transform hierarchy (this camera is meant to be a child
	 * of the player character's transform).
	 */
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	public class FirstPersonCamera : MonoBehaviour
	{
		#region Public variables
		public float minVerticalAngle = -50.0f;
		public float maxVerticalAngle = 50.0f;
		#endregion
		#region Private variables
		private float currentVerticalAngle = 0.0f;
		#endregion
		#region Lifecycle
		void LateUpdate()
		{
			AddVerticalRotation(Input.GetAxis("Look Vertical"));
		}
		#endregion
		#region Public methods
		public void AddVerticalRotation(float amount)
		{
			//	Roate only vertically
			currentVerticalAngle = Mathf.Clamp(currentVerticalAngle + amount, minVerticalAngle, maxVerticalAngle);
			/*
			 * Quaternion operations:
			 *	- Quaternion * Quaternion: stacks the two rotations, applying them
			 *		one after another; returns a Quaternion
			 *	- Quaternion * Vector: rotates the vector by the rotation amount
			 *		defined by the quaternion; returns a Vector
			 */
			transform.localRotation = Quaternion.AngleAxis(-currentVerticalAngle, Vector3.right) *  Quaternion.identity;	/* Multiplication by Quaterion.identity is implicit and can be omitted */
		}
		#endregion
	}
}
