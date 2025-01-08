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
using CharacterMovement;

namespace AnimationTests
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public class CharacterRootMotionController : MonoBehaviour
	{
		#region Private variables
		private Animator animator = null;

		[Header("Motion logic")]
		[SerializeField]
		private CharacterControllerMover characterMover = null;

		[Header("Animator")]
		[SerializeField]
		private string animatorSpeedFloatParamName = "Move Speed";
		private int animatorSpeedFloatParam;
		[SerializeField]
		private string animatorAngleFloatParamName = "Angle";
		private int animatorAngleFloatParam;
		[SerializeField]
		[Min(0.0f)]
		private float angleSmoothReachTime = 0.25f;
		private float angleVelocity = 0.0f;
		#endregion
		#region Lifecycle
		void Awake()
		{
			animator = GetComponent<Animator>();

			//	Cache animator params hashes
			animatorSpeedFloatParam = Animator.StringToHash(animatorSpeedFloatParamName);
			animatorAngleFloatParam = Animator.StringToHash(animatorAngleFloatParamName);
		}
		void Update()
		{
			//	Get move direction
			Vector3 moveDirection = Vector3.zero;
			Transform cam = Camera.main.transform;
			moveDirection += Input.GetAxis("Horizontal") * cam.right;
			moveDirection += Input.GetAxis("Vertical") * cam.forward;
			moveDirection.y = 0.0f;

			//	Feed animator with motion parmeters
			/*
			 * Since we're not handling multiple walk paces, we don't need to smooth the
			 * speed parameter, as the ransition time between walk and idle will do the
			 * job for us.
			 */
			animator.SetFloat(animatorSpeedFloatParam, Mathf.Min(1.0f, moveDirection.magnitude));

			/*
			 * Rotation behaves differently from speed as it wraps around -180 and 180 degrees.
			 * For this reason I'm not going to use animator's internal smoothing but I'll
			 * handle smoothing myself, using a function which is specifically designed to handle
			 * angles.
			 */
			float fromAngle = animator.GetFloat(animatorAngleFloatParamName);
			float toAngle = Vector3.SignedAngle(characterMover.transform.forward, moveDirection, characterMover.transform.up);
			float newAngle = Mathf.SmoothDampAngle(fromAngle, toAngle, ref angleVelocity, angleSmoothReachTime);
			newAngle = Mathf.Repeat(newAngle + 180.0f, 360.0f) - 180.0f;	//	Ensure spring damp doesn't overshoot
			animator.SetFloat(animatorAngleFloatParam, newAngle);
		}
		void OnAnimatorMove()
		{
			//	No mover, no party
			if(characterMover == null)
				return;

			//	Impart movement
			characterMover.MoveDirect(animator.deltaPosition);
			characterMover.RotateDirect(animator.deltaRotation);
		}
		#endregion
	}
}
