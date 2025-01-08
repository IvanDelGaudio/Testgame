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
	public class OldGUI : MonoBehaviour
	{
		#region Private variables
		private string text = "placeholder";
		#endregion
		#region Lifecycle
		void OnGUI()
		{
			//  Display
			GUILayout.Label("qwe");
			GUILayout.Box("asd");

			//  Input
			text = GUILayout.TextField(text);
			Time.timeScale = GUILayout.HorizontalSlider(Time.timeScale, 0.0f, 2.0f);

			//  Actions
			if(GUILayout.Button("Click Me!"))
				Debug.Log("Button clicked!!! " + text);

			//   Layout w/o scopes
			GUILayout.BeginHorizontal();
			GUILayout.Box("asd");
			GUILayout.Space(10);
			GUILayout.BeginVertical();
			GUILayout.Box("asd");
			GUILayout.Space(10);
			GUILayout.Box("asd");
			GUILayout.Space(10);
			GUILayout.Box("asd");
			GUILayout.EndVertical();
			GUILayout.Space(10);
			GUILayout.Box("asd");
			GUILayout.Space(10);
			GUILayout.Box("asd");
			GUILayout.EndHorizontal();

			//   Layout w/ scopes
			using(new GUILayout.HorizontalScope())
			{
				GUILayout.Box("qwe");
				GUILayout.Space(10);
				using(new GUILayout.VerticalScope())
				{
					GUILayout.Box("qwe");
					GUILayout.Space(10);
					GUILayout.Box("qwe");
					GUILayout.Space(10);
					GUILayout.Box("qwe");
				}
				GUILayout.Space(10);
				GUILayout.Box("qwe");
				GUILayout.Space(10);
				GUILayout.Box("qwe");
			}
		}
		#endregion
	}
}
