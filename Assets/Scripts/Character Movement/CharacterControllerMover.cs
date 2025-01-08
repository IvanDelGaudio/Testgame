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
using UnityEngine.EventSystems;

namespace CharacterMovement
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(CharacterController))]
	public class CharacterControllerMover : MonoBehaviour
	{
		#region Public enums
		public enum OrientMode : byte
		{
			None,
			Movement,
			LookDirection
		}
		public enum MotionSource : byte
		{
			Input,
			External
		}
		#endregion
		#region Private variables
		private CharacterController characterController = null;

		[Header("Anchors")]
		[SerializeField]
		private Transform renderRoot;

		[Header("Move params")]
		[SerializeField]
		private MotionSource motionSource = MotionSource.Input;
		[SerializeField]
		[Min(0.25f)]
		private float speed = 5.0f;
		[SerializeField]
		[Min(0.0f)]
		private float jumpSpeed = 10.0f;
		[SerializeField]
		private OrientMode orientMode = OrientMode.Movement;
		[SerializeField]
		[Min(0.0f)]
		private float orientReachTime = 0.15f;
		private Vector3 orientCurrentSpeed = Vector3.zero;

		[Header("Debug")]
		[SerializeField]
		bool useSimpleMove = false;
		[SerializeField]
		bool useCustomFloorDetection = false;

		[Header("Floor detection params")]
		[SerializeField]
		[Range(0f, 0.1f)]
		private float floorDetectionOffset = 0.001f;
		private float verticalSpeed = 0.0f;
		#endregion
		#region Private properties
		private bool isGrounded
		{
			get
			{
				if(useCustomFloorDetection)
				{
					Ray ray = new Ray(
						transform.TransformPoint(characterController.center) - transform.up * (0.5f * characterController.height - characterController.radius),
						Vector3.down
					);
					return Physics.SphereCast(
						ray,
						characterController.radius,
						characterController.skinWidth + floorDetectionOffset
					);
				}
				else
					return characterController.isGrounded;
			}
		}
		#endregion
		#region Lifecycle
		void Awake()
		{
			characterController = GetComponent<CharacterController>();
		}
		void Start()
		{
			//	Compensate the skin width by pushing the render model down and make it match the floor
			if(renderRoot != null)
				renderRoot.position -= transform.up * characterController.skinWidth;
		}
#if UINTY_EDITOR
	void OnGUI()
	{
		GUILayout.Box(isGrounded ? "Grounded" : "Not Grounded");
		GUILayout.Box($"Vertical speed: {verticalSpeed:0.00}m/s");
	}
#endif
		void FixedUpdate()
		{
			/*
			 * We're manipulating dilated modifications to the
			 * vertical speed, which is a physical quantity, in
			 * the fixed update to make it more stable.
			 */

			//	Reset vertical speed when touching ground
			if(isGrounded)
				verticalSpeed = 0.0f;

			//	Apply gravity
			verticalSpeed -= 9.81f * Time.deltaTime;
		}
		void Update()
		{
			//	Handle aim input
			if(Input.GetButtonDown("Fire2"))
				orientMode = OrientMode.LookDirection;
			else if(Input.GetButtonUp("Fire2"))
				orientMode = OrientMode.Movement;

			if(motionSource == MotionSource.Input)
			{
				//	Apply jump
				if(
					Input.GetButtonDown("Jump") &&
					isGrounded
				)
					verticalSpeed += jumpSpeed;

				//	Calculate move direction
				Vector3 moveDirection = Vector3.zero;
				Transform cam = Camera.main.transform;
				moveDirection += Input.GetAxis("Horizontal") * cam.right;
				moveDirection += Input.GetAxis("Vertical") * cam.forward;

				//	Planarize and keep original magnitude
				float moveMagnitude = moveDirection.magnitude;
				moveDirection.y = 0.0f;
				moveDirection.Normalize();
				moveDirection *= moveMagnitude;

				//	Calcualte final move velocity
				Vector3 moveVelocity = moveDirection * speed;

				//	Apply movement
				Move(moveVelocity);
			}
		}
		void LateUpdate()
		{
			Orient();
		}
		#endregion
		#region Public methods
		public void Move(Vector3 direction) => MoveDirect(direction * Time.deltaTime);
		public void MoveDirect(Vector3 direction)
		{
			//	Apply movement
			if(useSimpleMove)
				characterController.SimpleMove(direction / Time.deltaTime);
			else
			{
				direction += Vector3.up * verticalSpeed;
				characterController.Move(direction);
			}
		}
		public void RotateDirect(Quaternion rotation)
		{
			transform.rotation *= rotation;
		}
		public Vector3 GetCurrentVelocity()
		{
			return characterController.velocity;
		}
		#endregion
		#region Private methods
		private void Orient()
		{
			//	Prepare direction
			Vector3 direction = Vector3.zero;

			//	Orient
			switch(orientMode)
			{
				case OrientMode.None:
					return;
				case OrientMode.Movement:
					direction = GetCurrentVelocity();
					break;
				case OrientMode.LookDirection:
					direction = Camera.main.transform.forward;
					break;
				default:
					Debug.LogError($"Orient mode {orientMode} not handled.");
					break;
			}

			//	Planarize direction
			direction.y = 0.0f;

			//	Lightweight check for zero vector
			if(
				Mathf.Approximately(direction.x, 0.0f) &&
				Mathf.Approximately(direction.z, 0.0f)
			)
				return;

			//	Prepare target rotation
			Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);

			//	Smooth rotate to target rotation
			transform.rotation = SmoothDampRotation(
				transform.rotation, //	FROM
				targetRotation, //	TO
				ref orientCurrentSpeed, //	Speed (updated by the function itself)
				orientReachTime //	How long the rotation takes to reach the target
			);
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


			if((smoothedAngle - toEuler).sqrMagnitude < 0.05f)
			{
				// Check for small angle differences and directly set the angle if within threshold
				smoothedAngle = toEuler;
				currentVelocity = Vector3.zero;
			}
			else
				// Clamp the velocity to prevent overshooting
				currentVelocity = Vector3.ClampMagnitude(currentVelocity, 45.0f);

			//	Euler -> Quat
			return Quaternion.Euler(smoothedAngle);
		}
		#endregion
	}
}
