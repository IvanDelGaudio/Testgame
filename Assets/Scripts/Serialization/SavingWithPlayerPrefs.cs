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

namespace SerializationTests
{
	[DisallowMultipleComponent]
	public class SavingWithPlayerPrefs : MonoBehaviour
	{
		#region Private methods
		private void ReadData(string keyName)
		{
			//	Keys can be read providing a default value to return in case the key is not defined
			string s = PlayerPrefs.GetString(keyName, "default value");
			int i = PlayerPrefs.GetInt(keyName, 0);
			float f = PlayerPrefs.GetFloat(keyName, 0.0f);
		}
		private void SaveData(string keyName)
		{
			PlayerPrefs.SetString(keyName, "new value");
			PlayerPrefs.SetInt(keyName, 10);
			PlayerPrefs.SetFloat(keyName, 10.0f);
		}
		private bool HasData(string keyName)
		{
			return PlayerPrefs.HasKey(keyName);
		}
		private void DeleteData(string keyName)
		{
			PlayerPrefs.DeleteKey(keyName);
		}
		private void DeleteAllData()
		{
			PlayerPrefs.DeleteAll();
		}
		private void EnsureSaveData()
		{
			/*
			 * PlayerPrefs are loaded at game start and automatically
			 * saved on application quit.
			 * It's still wise to call Save() in specific moments to
			 * prevent data loss on crash.
			 */
			PlayerPrefs.Save();
		}
		#endregion
	}
}
