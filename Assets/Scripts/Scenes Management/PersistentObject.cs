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

namespace SceneMGMT
{
	[DisallowMultipleComponent]
	public class PersistentObject : MonoBehaviour
	{
		#region Lifecycle
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
		#endregion
	}
}
