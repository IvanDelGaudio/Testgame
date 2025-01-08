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

/*
 * This sample class has the goal to pinpoint the
 * basic challenges in character movement.
 * This does not pretend to be a fully functional
 * implementation.
 */

namespace CharacterMovement
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CapsuleCollider))]
	public class CustomCharacterMover : MonoBehaviour
	{
		#region Private enums
		private enum CollisionCheckMode
		{
			Ray,
			Capsule
		}
		#endregion
		#region Public variables
		public LayerMask hitLayers = Physics.AllLayers;
		#endregion
		#region Private variables
		private CapsuleCollider capsule = null;
		[SerializeField]
		private CollisionCheckMode collisionCheckMode = CollisionCheckMode.Capsule;
		#endregion
		#region Lifecycle
		void Awake()
		{
			capsule = GetComponent<CapsuleCollider>();
		}
		void Update()
		{
			/*
			 * Calculate the move direction in a single vector.
			 * This allows for a single call to the move function,
			 * which is usually the best approach, compared to
			 * multiple calls with components of the movement.
			 */
			Vector3 moveDirection = Vector3.zero;
			moveDirection += Input.GetAxis("Horizontal") * transform.right;
			moveDirection += Input.GetAxis("Vertical") * transform.forward;

			Move(moveDirection);
		}
		#endregion
		#region Public methods
		public void Move(Vector3 direction)
		{
			//	Scale direction by delta time to produce a smooth movement throughout frames
			Vector3 offset = direction * Time.deltaTime;

			//	Prepare results for the raycast/shapecast operation
			RaycastHit hitInfo;

			//	Prepare the ray for the raycast/shapecast operation (and the debug draw)
			Ray ray = new Ray(transform.TransformPoint(capsule.center), offset.normalized);

			//	Calculate the ray distance
			float distance;
			switch(collisionCheckMode)
			{
				case CollisionCheckMode.Ray:
					distance = offset.magnitude + capsule.radius;
					break;
				case CollisionCheckMode.Capsule:
					distance = offset.magnitude;
					break;
				default:
					throw new S.Exception($"This must not happen: make sure to handle the {collisionCheckMode} check mode.");
			}

			//	Calculate parameters for the capsule cast
			Vector3 capsuleHalfHeight = transform.up * (capsule.height * 0.5f - capsule.radius);
			Vector3 capsulePoint1 = ray.origin + capsuleHalfHeight;
			Vector3 capsulePoint2 = ray.origin - capsuleHalfHeight;

			//	Draw a line for debug purposes, showing the actual limits of the raycast operation
			Debug.DrawRay(ray.origin, ray.direction * distance, Color.green);

			//	Perform raycast/shapecast operation
			bool didCastHitAnything;
			switch(collisionCheckMode)
			{
				case CollisionCheckMode.Ray:
					didCastHitAnything = Physics.Raycast(
						ray,
						out hitInfo,
						distance,
						hitLayers
					);
					break;
				case CollisionCheckMode.Capsule:
					didCastHitAnything = Physics.CapsuleCast(
						capsulePoint1,
						capsulePoint2,
						capsule.radius,
						ray.direction,
						out hitInfo,
						offset.magnitude
					);
					break;
				default:
					throw new S.Exception($"This must not happen: make sure to handle the {collisionCheckMode} check mode.");
			}

			//	Check if the raycast/shapecast operation hit anything
			if(!didCastHitAnything)
				//	Apply offset only if it doesn't produce an overlap
				transform.position += offset;
		}
		#endregion
	}
}
