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
	public class PlayerController : MonoBehaviour
	{
		#region Private variables
		private ObjectMover mover = null;
		#endregion
		#region Lifecycle
		void Awake()
		{
			mover = GetComponent<ObjectMover>();
		}
		void Update()
		{
			mover.Turn(Input.GetAxis("Horizontal"));
			mover.Move(Input.GetAxis("Vertical"));
		}
		void OnControllerColliderHit(ControllerColliderHit hit)
		{
			Debug.Log($"{name} colliding with {hit.collider.gameObject.name}");
		}
		#endregion
	}
}
