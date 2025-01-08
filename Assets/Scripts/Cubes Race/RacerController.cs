using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubesRace
{
	/*
	 * This generic (abstract) class holds the common data and
	 * logic for all specialized controllers.
	 * We decided to make the Process method abstract, instead
	 * of virtual, to make sure that every derived class defines
	 * a behavior for it.
	 * Moreover, by exposing the Process method, instead of
	 * just making sub-classes implement the Update message,
	 * we can ensure that validity checks are made once and
	 * in a centralized manner and sub-classes do not need
	 * to worry about common validations.
	 * 
	 * DisallowMultipleComponent is set on this base class to
	 * prevent different controllers to be assigned to the
	 * same game object.
	 */
	[DisallowMultipleComponent]
	public abstract class RacerController : MonoBehaviour
	{
		#region Public variables
		public CubeRacer controlledRacer;
		#endregion
		#region Lifecycle
		protected virtual void Update()
		{
			//	Call Process only if a valid controlled racer is present
			if(controlledRacer != null)
				Process();
		}
		#endregion
		#region Protected methods
		/// <summary>
		/// Implement to define a frame-by-frame control logic.
		/// When this function is called, it is guaranteed that
		/// the controlledRacer is valid.
		/// </summary>
		protected abstract void Process();
		#endregion
	}
}
