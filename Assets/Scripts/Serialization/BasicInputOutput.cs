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
	[DisallowMultipleComponent]
	public class BasicInputOutput : MonoBehaviour
	{
		#region Private methods
		private void Paths()
		{
			//	Get base data path
			string path = Application.persistentDataPath;
			//	Get containing folder path
			path = Path.Combine(path, "Save Data", "Slot 01");
			//	Ensure directory exists
			Directory.CreateDirectory(path);
			//	Get file path
			path = Path.Combine(path, "file.txt");

			Debug.Log(path);
		}
		private void RawWriteFile(string path, byte[] bytesToWrite)
		{
			using(FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
			{	//	The using directive ensure that the opened file stream is closed as it goes out of scope
				//	Write the bytes to the file stream, starting from the 0 position, for the length of the buffer
				fs.Write(bytesToWrite, 0, bytesToWrite.Length);
			}
		}
		private void RawReadFile(string path)
		{
			using(FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read))
			{	//	The using directive ensure that the opened file stream is closed as it goes out of scope
				//	Use a small buffer to read portions of the file
				byte[] buffer = new byte[256];

				//	Read the file piece by piece to avoid saturating the RAM
				while(true)
				{
					int offset = 0;
					int bytesRead = fs.Read(buffer, offset, buffer.Length);
					if(bytesRead == 0)
						break;
					for(int i = 0; i < bytesRead; i++)
					{
						byte b = buffer[i];
						//.... do something with the files
					}
					offset += bytesRead;
				}
			}
		}
		private void StreamWriteFile(string path, params string[] contentLines)
		{
			using(FileStream fs = File.OpenWrite(path))
			using(StreamWriter sw = new StreamWriter(fs))
			{
				foreach(string line in contentLines)
					sw.WriteLine(line);
			}

		}
		private void StreamReadFile(string path)
		{
			string fileContents = "";
			using(FileStream fs = File.OpenRead(path))
			using(StreamReader sr = new StreamReader(fs))
			{
				string line;
				while((line = sr.ReadLine()) != null)
					fileContents += line + S.Environment.NewLine;
			}

			Debug.Log(fileContents);
		}
		private void QuickWrite(string path)
		{
			File.WriteAllBytes(path, new byte[1024]);
			File.WriteAllText(path, "Hello, world!");
			File.WriteAllLines(path, new string[10]);
		}
		private void QuickAppend(string path)
		{
			File.AppendAllText(path, "New Text Here!!");
			File.AppendAllLines(path, new string[10]);
		}
		private void QuickRead(string path)
		{
			byte[] fileBytes = File.ReadAllBytes(path);
			string fileText = File.ReadAllText(path);
			string[] lines = File.ReadAllLines(path);
		}
		#endregion
	}
}
