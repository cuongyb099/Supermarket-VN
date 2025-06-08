using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using KatLib.Data_Serialize;
using KatLib.Singleton;
using UnityEngine;
using UnityEngine.Audio;

namespace KatAudio
{
	public class SoundManager : SingletonPersistent<SoundManager>
	{
		private GameObject _emptyObj;
		private AudioMixer _audioMixer;
		private AudioMixerGroup _masterGroup;
		private AudioMixerGroup _bgGroup;
		private AudioMixerGroup _sfxGroup;
		private AudioChildPool _audioSourcePool;
		private bool _isSound;
		private CancellationToken _cancelToken;
		private readonly List<AudioSource> _instanceAudioSource = new();
		private readonly Dictionary<string, AudioClip> _soundData = new();
		private readonly Dictionary<string, List<AudioClip>> _soundGroup = new();
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Initialize()
		{
			GetInstance();
		}
			
		protected override void Awake()
		{
			base.Awake();
			CreateAudioChildPrefab();
			LoadAudioMixer();
		}
	
		private void Start()
		{
			LoadVolume();
		}

		private void CreateAudioChildPrefab()
		{
			_emptyObj = new GameObject("Audio Child")
			{
				transform =
				{
					parent = transform
				}
			};
			var audioChild = _emptyObj.AddComponent<AudioChild>();
			_audioSourcePool = new AudioChildPool(audioChild);
			
		}
		private void LoadAudioMixer()
		{
			_audioMixer = Resources.Load<AudioMixer>("Audio Mixer");
			var groups = _audioMixer.FindMatchingGroups("Master");
			_masterGroup = groups[0];
			_bgGroup = groups[1];
			_sfxGroup = groups[2];
		}

		public async UniTask LoadSoundGroupAsync(string key)
		{
			if(_soundGroup.ContainsKey(key)) return;
			
			var clips = await AddressablesManager.Instance.LoadAssetsAsync<AudioClip>(key, token: _cancelToken);
			
			if(clips == null) return;

			foreach (var clip in clips)
			{
				_soundData.Add(clip.name, clip);
			}
			
			_soundGroup.Add(key, clips);
			
			AddressablesManager.Instance.ReleaseInstance(key);
		}

		public void ReleaseSoundGroup(string key, Action onReleaseComplete = null)
		{
			if(!_soundGroup.TryGetValue(key, out var clips)) return;
			
			foreach (var clip in clips)
			{
				_soundData.Remove(clip.name);
			}
			_soundGroup.Remove(key);
			onReleaseComplete?.Invoke();
		}
		
		private void LoadVolume()
		{
			SetSoundVolume(SoundVolumeConstant.MusicVolume, PlayerPrefs.GetFloat(SoundVolumeConstant.MusicVolume, 1));
			SetSoundVolume(SoundVolumeConstant.MasterVolume, PlayerPrefs.GetFloat(SoundVolumeConstant.MasterVolume, 1));
			SetSoundVolume(SoundVolumeConstant.FXVolume, PlayerPrefs.GetFloat(SoundVolumeConstant.FXVolume, 1));
		}

		public void PlaySound(Sound sound)
		{
			var audioClip = GetClipFromLibrary(sound.ClipName);
			if (!audioClip) return;

			var audioChild = _audioSourcePool.GetFromPool(sound.Position, default) as AudioChild;
			audioChild.transform.parent = transform;
			audioChild.FollowTarget = sound.FollowTarget;
			var audioSource = audioChild.Source;

			if (!_instanceAudioSource.Contains(audioSource))
			{
				_instanceAudioSource.Add(audioSource);
			}
			
			audioSource.clip = audioClip;
			audioSource.volume = sound.Volume;
			audioSource.pitch = sound.Pitch;
			audioSource.loop = sound.Loop;
				
			audioSource.outputAudioMixerGroup = GetMixerGroup(sound.M_SoundType);
			audioSource.playOnAwake = false;
			audioSource.panStereo = sound.StereoPan;
			audioSource.spatialBlend = sound.SpatialBlend;
			audioSource.minDistance = sound.MinDistance;
			audioSource.maxDistance = sound.MaxDistance;
			audioSource.rolloffMode = sound.RolloffMode;
			audioSource.Play();
			audioChild.StartSound(_cancelToken);
			
			if (!sound.IsFade) return;
			
			sound.SetVolume(0f);
			audioSource.DOFade(sound.Volume, sound.FadeDuration);
		}

		public void StopSound(string clipName, bool stopAll = false, bool stopImmediately = false)
		{
			foreach (var audioSource in _instanceAudioSource)
			{
				if(audioSource.clip.name != clipName) continue;
				
				if (stopImmediately)
				{
					audioSource.gameObject.SetActive(false);
					if (!stopAll) return;
					continue;
				}
				
				audioSource.loop = false;
				if (!stopAll) return;
			}
		}
		
		public void StopAllSound()
		{
			foreach (var audioSource in _instanceAudioSource)
			{
				if (!audioSource.isPlaying) continue;
				audioSource.Stop();
			}
		}

		private AudioClip GetClipFromLibrary(string clipName)
		{
			return _soundData.GetValueOrDefault(clipName);
		}

		private AudioMixerGroup GetMixerGroup(SoundType type)
		{
			switch (type)
			{
				case SoundType.Master:
					return _masterGroup;
				case SoundType.BGM:
					return _bgGroup;
				case SoundType.SFX:
					return _sfxGroup;
				default:
					return null;
			}
		}

		public static string GetVolumeParameter(SoundType type)
		{
			switch (type)
			{
				case SoundType.Master:
					return SoundVolumeConstant.MasterVolume;
				case SoundType.BGM:
					return SoundVolumeConstant.MusicVolume;
				case SoundType.SFX:
					return SoundVolumeConstant.FXVolume;
				default:
					return null;
			}
		}

		public void DestroyAllInstanceSound()
		{
			foreach (var audioSource in _instanceAudioSource)
			{
				Destroy(audioSource.gameObject);
			}
			_instanceAudioSource.Clear();
		}
		
		public void SetSoundVolume(SoundType soundType, float value)
		{
			SetSoundVolume(GetVolumeParameter(soundType), value);
		}
		
		public void SetSoundVolume(string parameter, float value)
		{
			value = Math.Clamp(value, 0.0001f, 0.999f);
			_audioMixer.SetFloat(parameter, (float)Math.Log10(value) * 20f);
		}

		public void SetSound(bool value)
		{
			_isSound = value;
			if (!_isSound) _instanceAudioSource.ForEach(x => x.Stop());
		}
	}
}