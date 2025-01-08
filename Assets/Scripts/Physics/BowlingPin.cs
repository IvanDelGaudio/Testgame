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
	public class BowlingPin : MonoBehaviour
	{
		#region Private variables
		[SerializeField]
		[Range(1.0f, 89.0f)]
		private float fallAngleTreshold = 35.0f;
		private Rigidbody rb;

		private bool wasAsleep = false;
		#endregion
		#region Lifecycle
		void Awake()
		{
			rb = GetComponent<Rigidbody>();
		}
		void Start()
		{
			//	Initialize sleep state
			wasAsleep = rb.IsSleeping();
		}
		void FixedUpdate()
		{
			//	Prepare sleep check iteration
			bool isAsleep = rb.IsSleeping();

			if(isAsleep && !wasAsleep)
			{   //	Stopped
				float stoppingAngle = Vector3.Angle(Vector3.up, transform.up);
				if(stoppingAngle >= fallAngleTreshold)
				{   //	Fell!!
					Debug.Log($"{name} just fell!");
					if(stoppingAngle > 160.0f)
						Debug.Log($"{name} is impossible!!!!");
				}
			}

			//	Step sleep check
			wasAsleep = isAsleep;
		}
		#endregion
	}
}
