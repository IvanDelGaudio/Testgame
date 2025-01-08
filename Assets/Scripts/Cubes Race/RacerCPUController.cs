using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubesRace
{
	/*
	 * This sub-class of RacerController takes care of controlling
	 * racers on behalf of an AI.
	 * This class is extremely simple thanks to the structure we
	 * desigend.
	 */
	public class RacerCPUController : RacerController
	{
		#region Protected methods
		protected override void Process()
		{
			controlledRacer.Move();
		}
		#endregion
	}
}
