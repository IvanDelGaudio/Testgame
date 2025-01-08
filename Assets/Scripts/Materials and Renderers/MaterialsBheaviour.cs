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

namespace MatRendererTests
{
	[DisallowMultipleComponent]
	public class MaterialsBheaviour : MonoBehaviour
	{
		#region Public variables
		#endregion
		#region Private variables
		[SerializeField]
		private Renderer r;
		[SerializeField]
		private Texture2D tex;
		#endregion
		#region Lifecycle
		void Update()
		{
			if(
				r != null &&
				Input.GetKeyDown(KeyCode.Tab)
			)
			{
				r.material.color = Random.ColorHSV();
				r.material.SetColor("_Color", Random.ColorHSV());
				r.material.mainTexture = tex;
				r.material.SetTexture("_MainTex", tex);
				r.material.SetFloat("_Metallic", Random.Range(0.0f, 1.0f));
			}
		}
		#endregion
		#region Public methods
		#endregion
		#region Private methods
		#endregion
	}
}
