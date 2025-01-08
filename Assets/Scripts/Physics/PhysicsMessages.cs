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
	public class PhysicsMessages : MonoBehaviour
	{
		#region Lifecycle
		void OnCollisionEnter(Collision collision)
		{
			Debug.Log($"Collision started with {collision.gameObject.name}, collided with {collision.collider.name}, from body {collision.rigidbody.name}");
		}
		void OnCollisionStay(Collision collision)
		{
			Debug.Log($"Collision in act with {collision.gameObject.name}...");
		}
		void OnCollisionExit(Collision collision)
		{
			Debug.Log($"Collision concluded with {collision.gameObject.name}");
		}

		void OnTriggerEnter(Collider other)
		{
			Debug.Log($"{other.name} entered the trigger");
		}
		void OnTriggerStay(Collider other)
		{
			Debug.Log($"{other.name} stays in the trigger...");
		}
		void OnTriggerExit(Collider other)
		{
			Debug.Log($"{other.name} left the trigger");
		}
		#endregion
	}
}
