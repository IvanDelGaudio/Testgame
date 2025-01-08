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
using UnityEngine.Audio;

namespace AudioTests
{
	[DisallowMultipleComponent]
	public class AudioTester : MonoBehaviour
	{
		#region Public variables
		public AudioClip clip;
		public AudioMixerGroup targetGroup;
		public AudioMixer mixer;
		#endregion
		#region Lifecycle
		void Update()
		{
			//	Transition to snapshot
			if(Input.GetKeyDown(KeyCode.LeftShift))
			{
				AudioMixerSnapshot snap = mixer.FindSnapshot("My");
				mixer.TransitionToSnapshots(
					new AudioMixerSnapshot[] { snap },
					new float[] { 1.0f },
					5.0f
				);
			}
			//	Set mixer exposed parameter
			if(Input.GetKeyDown(KeyCode.LeftControl))
			{
				mixer.SetFloat("Master Volume", Random.Range(-80f, 0f));
			}
			//	Play clip at random point around the player
			if(Input.GetKeyDown(KeyCode.Tab))
			{
				//	Find random point arount the camera (audio listener)
				Vector3 spawnPosition = transform.position + Random.onUnitSphere * Random.Range(0.2f, 1.5f);

				//	Play clip at point using Unity's utility (limited, no reference to the spawned source)
				//AudioSource.PlayClipAtPoint(clip, spawnPosition);

				//	Play clip at point using our custom utility
				AudioSource source = PlayClipAtPoint(clip, Camera.main.transform.TransformPoint(Random.onUnitSphere));

				//	Manipulate instance, as our utility returns it
				//source.volume = Random.Range(0.0f, 1.0f);
				source.outputAudioMixerGroup = targetGroup;
			}
		}
		#endregion
		#region Private methods
		private AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, bool autoDestroy = true, bool loop = false)
		{
			//	Prepare game object for audio source component
			GameObject sourceGO = new GameObject($"{clip.name}@{Time.frameCount}");

			//	Add the audio source component to the created game object
			AudioSource source = sourceGO.AddComponent<AudioSource>();

			//	Prepare audio source with needed data
			source.clip = clip;
			source.spatialBlend = 1.0f;	//	3D sound
			source.loop = loop;
			if(!loop)	//	Looping sounds must not destroy after the duration of a single loop
				Destroy(sourceGO, clip.length);

			//	Play audio clip
			source.Play();

			//	Return instance for further edit or storage
			return source;
		}
		#endregion
	}
}
