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
	public class SampleUIManager : MonoBehaviour
	{
		#region Public variables
		[Header("Anchors")]
		public Text log;
		public Toggle firstToggle;
		public Slider slider;
		public Scrollbar scrollbar;
		public ScrollRect scrollRect;
		public Button button;
		public InputField inputField;
		#endregion
		#region Lifecycle
		void Start()
		{
			//  RW Text
			log.text = "PlaceHolder";
			Debug.Log(log.text);

			//  Set slider to float slider
			slider.wholeNumbers = false;
		}
		void Update()
		{
			//inputField.text = log.text;
			//log.text = inputField.text;
			//log.text = $"{scrollRect.normalizedPosition:0.00}";
		}
		#endregion
	}
}
