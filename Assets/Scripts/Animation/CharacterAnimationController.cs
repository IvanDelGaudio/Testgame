using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AnimationTests
{
	using CharacterMovement;

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public class CharacterAnimationController : MonoBehaviour
	{
		#region Private variables
		[Header("Motion logic")]
		[SerializeField]
		private CharacterControllerMover characterMover = null;

		[Header("Animation")]
		private Animator animator = null;
		[SerializeField]
		private string moveSpeedFloatParamName = "Move Speed";
		private int? _moveSpeedFloatParam = null;
		[SerializeField]
		[Min(0f)]
		private float moveSpeedReachTime = 0.025f;
		[SerializeField]
		private string angleFloatParamName = "Angle";
		private int? _angleFloatParam = null;
		[SerializeField]
		[Min(0f)]
		private float angleReachTime = 0.1f;
		#endregion
		#region Private properties
		private int moveSpeedFloatParam
		{
			get
			{
				if(
					_moveSpeedFloatParam == null ||
					!_moveSpeedFloatParam.HasValue
				)
					_moveSpeedFloatParam = Animator.StringToHash(moveSpeedFloatParamName);

				return _moveSpeedFloatParam.Value;
			}
		}
		private int angleFloatParam
		{
			get
			{
				if(
					_angleFloatParam == null ||
					!_angleFloatParam.HasValue
				)
					_angleFloatParam = Animator.StringToHash(angleFloatParamName);

				return _angleFloatParam.Value;
			}
		}
		#endregion
		#region Lifecycle
		void Awake()
		{
			animator = GetComponent<Animator>();
		}
		void Update()
		{
			Vector3 currentVelocity = characterMover.GetCurrentVelocity();
			currentVelocity.y = 0.0f;
			float moveSpeed = currentVelocity.magnitude;
			float angle = Vector3.SignedAngle(characterMover.transform.forward, currentVelocity, characterMover.transform.up);

			animator.SetFloat(moveSpeedFloatParam, moveSpeed, moveSpeedReachTime, Time.deltaTime);
			animator.SetFloat(angleFloatParam, angle, angleReachTime, Time.deltaTime);
		}
		#endregion
	}
}
