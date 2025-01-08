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
	[RequireComponent(typeof(ObjectMover))]
	public class AIController : MonoBehaviour
	{
		#region Public variables
		public Transform target;
		#endregion
		#region Private variables
		private ObjectMover mover = null;
		[SerializeField]
		private float forwardTolerance = 5.0f;
		private float angle = 0.0f;
		#endregion
		#region Lifecycle
		void Awake()
		{
			mover = GetComponent<ObjectMover>();
		}
		void Update()
		{
			if(target == null)
				return;


			Vector3 distanceVector = (target.position - transform.position).normalized;
			angle = Vector3.SignedAngle(distanceVector, transform.forward, transform.up);

			mover.Turn(Mathf.Clamp(-angle / 45.0f, -1.0f, 1.0f));

			if(angle < forwardTolerance)
				mover.Move(1.0f);

		}
		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Debug.Log($"{name} colliding with {hit.collider.gameObject.name}");
		}
		#endregion
	}
}
