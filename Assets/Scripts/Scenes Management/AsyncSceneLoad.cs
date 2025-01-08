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
	public class AsyncSceneLoad : MonoBehaviour
	{
		#region Public variables
		public string sceneToLoad = "Cameras Playground";
		public LoadSceneMode loadMode = LoadSceneMode.Single;
		#endregion
		#region Private variables
		AsyncOperation currentSceneLoad = null;
		#endregion
		#region Lifecycle
		void Update()
		{
			if(currentSceneLoad != null)
			{
				float progress = currentSceneLoad.progress;
				Debug.Log($"Loading: {(currentSceneLoad.progress / 0.9f) * 100.0f:0}%");
				if(
					progress >= 0.9f &&
					//!currentSceneLoad.allowSceneActivation &&
					Input.GetKeyDown(KeyCode.Space)
				)
					currentSceneLoad.allowSceneActivation = true;
			}
		}
		#endregion
		#region Public methods
		[ContextMenu("Load")]
		public void LoadScene()
		{
			currentSceneLoad = SceneManager.LoadSceneAsync(sceneToLoad, loadMode);
			currentSceneLoad.allowSceneActivation = false;
		}
		#endregion
	}
}
