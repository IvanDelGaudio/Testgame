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

namespace UITests
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Text))]
	public class PromptResponse : MonoBehaviour
	{
		#region Private variables
		private Text label = null;
		#endregion
		#region Public properties
		public string text
		{
			get => label.text;
			set => label.text = value;
		}
		#endregion
		#region Lifecycle
		void Awake()
		{
			label = GetComponent<Text>();
		}
		#endregion
	}
}
