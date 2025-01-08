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
using System.IO;

namespace SerializationTests
{
	[S.Serializable]	//	Make the class serializable to enable JSON serialization
	public class MySaveFile
	{
		public string playerName;
		public Vector3 position;
	}

	[DisallowMultipleComponent]
	public class JSONSerialziation : MonoBehaviour
	{
		#region Public variables
		public MySaveFile mySaveFile = new MySaveFile();
		#endregion
		#region Public methods
		[ContextMenu("Save")]
		public void Save()
		{
			//	Create JSON string from instance
			string jsonSaveFile = JsonUtility.ToJson(mySaveFile);

			//	Log JSON string for debug
			Debug.Log(jsonSaveFile);

			//	Save JSON string to file
			File.WriteAllText("mySaveFile.sav", jsonSaveFile);
		}
		[ContextMenu("Load")]
		public void Load()
		{
			//	Read file contents
			string fileContents = File.ReadAllText("mySaveFile.sav");

			//	Log file contents for debug
			Debug.Log(fileContents);

			//	Parse file contents into a new instance (less efficient but sometimes necessary)
			//mySaveFile = JsonUtility.FromJson<MySaveFile>(fileContents);
			//	Parse file contents into an existing instance (more efficient but not always possible)
			JsonUtility.FromJsonOverwrite(fileContents, mySaveFile);
		}
		#endregion
	}
}
