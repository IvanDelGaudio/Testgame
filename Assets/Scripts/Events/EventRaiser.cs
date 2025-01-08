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
	public class EventRaiser : MonoBehaviour
	{
		#region Private sub-classes
		[S.Serializable]
		private class EventRaiserUnityEvent : UnityEngine.Events.UnityEvent<EventRaiser> { }
		#endregion
		#region Public delegates
		//	The delegate is the support type for a C# event
		public delegate void EventRaiserDelegate(EventRaiser caller);
		#endregion
		#region Public events
		//	A C# event can be invoked only by the class that declares it
		public event EventRaiserDelegate OnSomething = null;
		#endregion
		#region Private variables
		[SerializeField]
		private EventRaiserUnityEvent onSomethingUE;
		#endregion
		#region Lifecycle
		void Awake()
		{
			//	Create a tunnel that invokes the unity event when the C# event is raised
			OnSomething += caller => onSomethingUE.Invoke(caller);
		}
		#endregion
		#region Public methods
		[ContextMenu("Raise Something Event")]
		public void DoSomething()
		{
			//...
			/*
			//	Extended event checked invoke
			if(OnSomething != null)
				OnSomething(this);
			*/
			//	Contracted on-line event invocation
			OnSomething?.Invoke(this);
		}
		public void HandleSomething(EventRaiser caller)
		{
			Debug.Log($"OnSomething triggered and it was received by tiself");
		}
		#endregion
		#region Protected methods
		protected void RaiseSomethingFromSubClass()
		{
			//	Sub-classes cannot directly invoke a C# event declared on the base class
			OnSomething?.Invoke(this);
		}
		#endregion
	}
}
