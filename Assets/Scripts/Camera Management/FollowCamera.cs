using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraManagement
{
	/*
	 * Generic camera with both look at and follow functionality.
	 * It implements a controllable smoothing of both movement and
	 * rotation.
	 * It exposes target offset configuration to accomodate different
	 * setups.
	 */
	[RequireComponent(typeof(Camera))]
	[DisallowMultipleComponent]
	public class FollowCamera : MonoBehaviour
	{
		#region Public enums
		[System.Flags]
		public enum FollowType
		{
			LookAt			= 1 << 0,	//	0001
			Follow			= 1 << 1	//	0010
		}
		public enum OffsetSpace
		{
			World,
			TargetLocal
		}
		#endregion
		#region Public variables
		[Header("Focus")]
		public Transform target = null;
		public OffsetSpace offsetSpace = OffsetSpace.World;
		public Vector3 targetOffset = Vector3.up;
		[Header("Movement")]
		public FollowType followType = (FollowType)(~0x0);
		[Min(0.01f)]
		public float targetFollowDistance = 10.0f;
		[Min(0.01f)]
		[Tooltip("Camera will have a hard movement when nearer to the target than this distance")]
		public float minFollowDistance = 1.0f;
		[Min(0.01f)]
		[Tooltip("Camera will have a hard movement when farther from the target than this distance")]
		public float maxFollowDistance = 20.0f;
		[Tooltip("How long it will take to reach the target position when following")]
		public float followSmoothTime = 0.25f;
		[Tooltip("How long it will take to center the focus point when looking at")]
		public float lookAtSmoothTime = 1.0f;
		#endregion
		#region Private variables
		//	Cached reference to the camera component on this game object (assigned on awake)
		private Camera cam = null;

		//	Support variables for smooth damp functions
		private Vector3 moveVelocity = Vector3.zero;
		private Vector3 rotateVelocity = Vector3.zero;
		#endregion
		#region Lifecycle
		void Awake()
		{
			//	Store reference to camera
			cam = GetComponent<Camera>();
			//	Same as:
			//cam = GetComponent(typeof(Camera)) as Camera;
		}
		void LateUpdate()
		{
			//	Take current target's position in world space
			Vector3 focusPoint;

			//	Apply target offset
			switch(offsetSpace)
			{
				case OffsetSpace.World:
					focusPoint = target.position + targetOffset;
					break;
				case OffsetSpace.TargetLocal:
					focusPoint = target.transform.TransformPoint(targetOffset);
					break;
				default:
					Debug.LogError($"{offsetSpace} not supported");
					focusPoint = Vector3.zero;
					break;
			}

			if((followType & FollowType.Follow) != 0x0)
			{
				//	Find the direction (unit vector) from the target point to the camera
				Vector3 targetToCamera = (transform.position - focusPoint).normalized;
				Vector3 cameraTargetPoint = focusPoint + targetToCamera * targetFollowDistance;

				//	Move the camera towards the desired location
				/*
				 * Here we have a couple of alternatives:
				 * Lerp:
					transform.position = Vector3.Lerp(
						transform.position,
						cameraTargetPoint,
						Time.deltaTime * boostMultiplier
					);
				 * The transform will never really reach the target,
				 * we have too little control over the actual position
				 * of the camera.
				 * 
				 * MoveTowards:
					transform.position = Vector3.MoveTowards(
						transform.position,
						cameraTargetPoint,
						speedInMetersBySecond * Time.deltaTime
					);
				 * Much better, the transform moves towards its target
				 * at a controlled speed and it will never overshoot the
				 * target position. Unfortunately, it's a linear motion,
				 * without accelerations, which makes it feel not so good.
				 * 
				 * SmoothDamp:
				 * The smooth damp function is similar to the MoveTowards,
				 * but it adds acceleration to movement, making it a great
				 * asset when smoothing camera movement.
				 */
				transform.position = Vector3.SmoothDamp(
					transform.position,
					cameraTargetPoint,
					ref moveVelocity,
					followSmoothTime
				);

				//	Clamp distance
				/*
				 * To clamp the distance between a min and a max, we need to
				 * make this check AFTER the smoothing, or it will be broken
				 * by the interpolation.
				 * We check the actual distance after the smoothing and we
				 * clamp and re-apply it to the transform.
				 */
				float distance = Vector3.Distance(transform.position, focusPoint);
				distance = Mathf.Clamp(distance, minFollowDistance, maxFollowDistance);
				transform.position = focusPoint + targetToCamera * distance;
			}

			if((followType & FollowType.LookAt) != 0x0)
			{
				//	Calculate the direction between the target and the camera, oriented towards the target (B - A)
				Vector3 lookDirection = focusPoint - transform.position;
				//lookDirection.Normalize();	//	LookRotation already normalizes!!

				//	Apply look rotation to the current transform
				transform.rotation =
					SmoothDampRotation(
						transform.rotation,
						Quaternion.LookRotation(lookDirection, Vector3.up),
						ref rotateVelocity,
						lookAtSmoothTime
					);
			}
		}
		#endregion
		#region Private static methods
		private static Quaternion SmoothDampRotation(Quaternion from, Quaternion to, ref Vector3 currentVelocity, float smoothTime)
		{
			//	Quat -> Euler
			Vector3 fromEuler = from.eulerAngles;
			Vector3 toEuler = to.eulerAngles;

			//	Smooth damp angle, axis by axis
			Vector3 smoothedAngle = new Vector3(
				Mathf.SmoothDampAngle(fromEuler.x, toEuler.x, ref currentVelocity.x, smoothTime),
				Mathf.SmoothDampAngle(fromEuler.y, toEuler.y, ref currentVelocity.y, smoothTime),
				Mathf.SmoothDampAngle(fromEuler.z, toEuler.z, ref currentVelocity.z, smoothTime)
			);

			//	Euler -> Quat
			return Quaternion.Euler(smoothedAngle);
		}
		#endregion
	}


}
