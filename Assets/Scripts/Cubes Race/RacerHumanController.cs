using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubesRace
{
	/*
	 * This sub-class of RacerController takes care of controlling
	 * racers on behalf of an human player.
	 * This class is extremely simple thanks to the structure we
	 * desigend.
	 */
	public class RacerHumanController : RacerController
	{
		#region Protected methods
		protected override void Process()
		{
			if(Input.GetButton("Jump"))
				controlledRacer.Move();
		}
		#endregion
	}
}
