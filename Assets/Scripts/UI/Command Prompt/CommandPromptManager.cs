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
	public class CommandPromptManager : MonoBehaviour
	{
		#region Private variables
		[Header("Layout")]
		[SerializeField]
		private InputField promptField;
		[SerializeField]
		private RectTransform promptLinesContainer;
		[Header("Prefab")]
		[SerializeField]
		private PromptResponse promptResponsePrefab;
		#endregion
		#region Lifecycle
		#endregion
		#region Public methods
		public void SubmitCurrentPrompt()
		{
			HandlePrompt(promptField.text);
		}
		public void HandlePrompt(string prompt)
		{
			//	Do not submit empty commands
			if(string.IsNullOrEmpty(prompt))
				return;

			//	Handle command
			PromptResponse lineInstance = Instantiate<PromptResponse>(promptResponsePrefab);
			lineInstance.transform.SetParent(promptLinesContainer, false);
			lineInstance.text = $"> {prompt}";

			//	Clear input prompt
			promptField.text = "";
		}
		#endregion
	}
}
