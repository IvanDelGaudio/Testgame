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

namespace EventsTests
{
	[DisallowMultipleComponent]
	public class EventReceiver : MonoBehaviour
	{
		#region Private variables
		[SerializeField]
		private EventRaiser referenceToRaiser;
		#endregion
		#region Lifecycle
		void Awake()
		{
			if(referenceToRaiser != null)
				referenceToRaiser.OnSomething += HandleSomething;
		}
		#endregion
		#region Private methods
		private void HandleSomething(EventRaiser caller)
		{
			Debug.Log($"OnSomething event triggered on {caller.name} and it was received by {name}");
		}
		#endregion
	}
}
