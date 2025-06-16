using UnityEngine;
using System;
using KatLib.Singleton;
using KatLib.Data_Serialize;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Audio;
using KatAudio;

namespace Core.Settings
{
    public enum GraphicQuality
    {
        Low,
        Medium,
        High
    }

    public class GameSettingsManager : SingletonPersistent<GameSettingsManager>
    {
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Toggle shakeToggle;
        [SerializeField] private List<Toggle> graphicQualityToggles;
        [SerializeField] private AudioMixer audioMixer;

        [Header("Default Settings")] [SerializeField]
        private float musicVolume = 0.3f;

        [SerializeField] private float sfxVolume = 0.6f;
        [SerializeField] private bool pushAlarmEnabled = true;
        [SerializeField] private GraphicQuality graphicQuality = GraphicQuality.Medium;

        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        private const string SHAKE_KEY = "Shake";
        private const string GRAPHIC_QUALITY_KEY = "GraphicQuality";

        private void OnEnable()
        {
            LoadSettings();
            
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            shakeToggle.onValueChanged.AddListener(OnPushAlarmChanged);
            graphicQualityToggles.ForEach(toggle => toggle.onValueChanged.AddListener(OnGraphicQualityChanged));
        }

        private void OnDisable()
        {
            musicVolumeSlider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
            sfxVolumeSlider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
            shakeToggle.onValueChanged.RemoveListener(OnPushAlarmChanged);
            graphicQualityToggles.ForEach(toggle => toggle.onValueChanged.RemoveListener(OnGraphicQualityChanged));
        }

        private void LoadSettings()
        {
            Debug.Log("Loading settings");
            musicVolume = DataSerialize.GetData<float>(MUSIC_VOLUME_KEY);
            sfxVolume = DataSerialize.GetData<float>(SFX_VOLUME_KEY);
            pushAlarmEnabled = DataSerialize.GetData<bool>(SHAKE_KEY);
            graphicQuality = (GraphicQuality)DataSerialize.GetData<int>(GRAPHIC_QUALITY_KEY);

            // UI
            musicVolumeSlider.value = musicVolume;
            sfxVolumeSlider.value = sfxVolume;
            shakeToggle.isOn = DataSerialize.GetData<bool>(SHAKE_KEY); 
            for (int i = 0; i < graphicQualityToggles.Count; i++)
            {
                graphicQualityToggles[i].isOn = (i == (int)graphicQuality);
            }
        }

        private void SaveSettings()
        {
            Debug.Log("Saving settings");
            DataSerialize.SetData(MUSIC_VOLUME_KEY, musicVolume);
            DataSerialize.SetData(SFX_VOLUME_KEY, sfxVolume);
            DataSerialize.SetData(SHAKE_KEY, pushAlarmEnabled);
            DataSerialize.SetData(GRAPHIC_QUALITY_KEY, (int)graphicQuality);
            DataSerialize.SaveFile();
        }

        private void OnMusicVolumeChanged(float value)
        {
            musicVolume = value;
            audioMixer.SetFloat(SoundVolumeConstant.MusicVolume, value);
            SaveSettings();
        }

        private void OnSFXVolumeChanged(float value)
        {
            sfxVolume = value;
            audioMixer.SetFloat(SoundVolumeConstant.FXVolume, value);
            SaveSettings();
        }

        private void OnPushAlarmChanged(bool value)
        {
            pushAlarmEnabled = value;
            SaveSettings();
        }

        private void OnGraphicQualityChanged(bool value)
        {
            if (!value) return;
            for (int i = 0; i < graphicQualityToggles.Count; i++)
            {
                if (graphicQualityToggles[i].isOn)
                {
                    graphicQuality = (GraphicQuality)i;
                    break;
                }
            }

            SaveSettings();
        }
    }
}