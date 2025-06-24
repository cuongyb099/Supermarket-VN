using KatAudio;
using UnityEngine;

public class SoundSliderSave : SliderPlayerPrefSave
{
    [SerializeField] protected SoundType soundType;
    
    protected override void HandleValueChange(float value)
    {
        base.HandleValueChange(value);
        SoundManager.Instance.SetSoundVolume(soundType, value);
        Debug.Log("xxx");
    }
}