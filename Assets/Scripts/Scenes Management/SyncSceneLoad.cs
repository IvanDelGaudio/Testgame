using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using S = System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SceneMGMT
{
	[DisallowMultipleComponent]
	public class SyncSceneLoad : MonoBehaviour
	{
		#region Public variables
		public string sceneToLoad = "Cameras Playground";
		public LoadSceneMode loadMode = LoadSceneMode.Single;
		#endregion
		#region Public methods
		[ContextMenu("Load")]
		public void LoadScene()
		{
			SceneManager.LoadScene(sceneToLoad, loadMode);
		}
		#endregion
	}
}
