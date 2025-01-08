using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CubesRace
{
	/*
	 * This class is desigend to keep control of the game flow,
	 * decides the gameplay loop and places the relevant entities
	 * in their designated positions, as well as checks for win
	 * conditions to be met, responding to them.
	 */
	public class RaceManager : MonoBehaviour
	{
		#region Private variables
		[SerializeField]
		private CubeRacer[] cubeRacers = new CubeRacer[0];
		#endregion
		#region Lifecycle
		void Start()
		{
			//	In the beginning, reset all racers
			PlaceRacersToStart();
		}
		void Update()
		{
			//	Loop through racers and determine if there's a winner
			List<CubeRacer> winners = new List<CubeRacer>();

			foreach(CubeRacer racer in cubeRacers)
			{
				if(racer.transform.position.z >= transform.position.z)
				{   //	We have a winner!
					winners.Add(racer);
				}
			}

			//	Handle winners
			if(winners.Count > 0)
			{
				//	Print winner's name
				string winnerNames = winners[0].name;
				for(int i = 1; i < winners.Count; i++)
					winnerNames += " & " + winners[i].name;
				Debug.Log($"{winnerNames} WON!!!");

				PlaceRacersToStart();
			}
		}
		#endregion
		#region Private methods
		private void PlaceRacersToStart()
		{
			//	Loop through all the racers and reset them
			foreach(CubeRacer racerToReset in cubeRacers)
				racerToReset.ResetRacer();
		}
		#endregion
	}
}
