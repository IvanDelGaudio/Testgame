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

namespace Misc
{
	using CameraManagement;
	using CharacterMovement;

	[DisallowMultipleComponent]
	public class SpawningTests : MonoBehaviour
	{
		#region Public variables
		public CharacterControllerMover myCharacterPrefab;
		public OrbitCamera myCameraPrefab;
		#endregion
		#region Private variables
		private CharacterControllerMover myCharacterInstance;
		private OrbitCamera myCameraInstance;
		#endregion
		#region Lifecycle
		void Start()
		{
			myCharacterInstance = Instantiate<CharacterControllerMover>(myCharacterPrefab);
			myCameraInstance = Instantiate<OrbitCamera>(myCameraPrefab);
			myCameraInstance.target = myCharacterInstance.transform;
		}
		#endregion
		#region Public methods
		#endregion
		#region Private methods
		[ContextMenu("Test")]
		private void CelanUp()
		{
			if(myCharacterInstance != null)
				Destroy(myCharacterInstance.gameObject);

			if(myCameraInstance != null)
				Destroy(myCameraInstance.gameObject);
		}
		#endregion
	}
}
