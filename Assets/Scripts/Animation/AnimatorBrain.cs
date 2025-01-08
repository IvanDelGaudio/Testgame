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
using UnityEngine.Assertions;

namespace AnimationTests
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Animator))]
	public class AnimatorBrain : MonoBehaviour
	{
		#region Private enums
		private enum FloatSetMode { Hard, Smooth }
		#endregion
		#region Private variables
		private Animator animator = null;
		[SerializeField]
		private FloatSetMode moveSpeedSetMode = FloatSetMode.Hard;
		[SerializeField]
		private string moveSpeedFloatParamName = "Move Speed";
		[SerializeField]
		[Range(0f, 5f)]
		float reachTime = 0.2f;
		[SerializeField]
		private KeyCode walkKey = KeyCode.Space;
		#endregion
		#region Lifecycle
		void Awake()
		{
			animator = GetComponent<Animator>();
		}
		void Update()
		{
			switch(moveSpeedSetMode)
			{
				case FloatSetMode.Hard:
					if(Input.GetKeyDown(walkKey))
						animator.SetFloat(moveSpeedFloatParamName, 1.0f);
					else if(Input.GetKeyUp(walkKey))
						animator.SetFloat(moveSpeedFloatParamName, 0.0f);
					break;
				case FloatSetMode.Smooth:
					animator.SetFloat(moveSpeedFloatParamName, Input.GetKey(walkKey) ? 1.0f : 0.0f, reachTime, Time.deltaTime);
					break;
				default:
					Assert.IsTrue(false, $"Move Speed Set Mode \"{moveSpeedSetMode}\" is not handled.");
					break;
			}
		}
		#endregion
	}
}
