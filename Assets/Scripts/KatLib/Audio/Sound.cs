using KatLib.Logger;
using KatLib.Pooling;
using UnityEngine;

namespace KatAudio
{
    [System.Serializable]
    public class Sound
    {
        [field: SerializeField] public string ClipName { get; private set; }
        [field: SerializeField] public SoundType M_SoundType { get; private set; } = SoundType.SFX; 
        [field: SerializeField] public Vector3 Position { get; private set; }
        [field: SerializeField] public float Volume { get; private set; } = 1f;
        [field: SerializeField] public float Pitch { get; private set; } = 1f;
        [field: SerializeField] public float StereoPan { get; private set; }
        [field: SerializeField] public float SpatialBlend { get; private set; }
        [field: SerializeField] public float MinDistance { get; private set; } = 1f;
        [field: SerializeField] public float MaxDistance { get; private set; } = 500f;
        [field: SerializeField] public bool Loop { get; private set; }
        [field: SerializeField] public bool IsFade { get; private set; }
        [field: SerializeField] public float FadeDuration { get; private set; }
        [field: SerializeField] public AudioRolloffMode RolloffMode { get; private set; } = AudioRolloffMode.Linear; 
        [field: SerializeField] public Transform FollowTarget { get; private set; }
        
        /// <summary>
        /// Sets the name of the audio clip to be played.
        /// </summary>
        /// <param name="clipName">The name of the audio clip.</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetClip(string clipName)
        {
            ClipName = clipName;
            return this;
        }

        /// <summary>
        /// Sets whether the sound should loop when played.
        /// </summary>
        /// <param name="isLoop">True if the sound should loop, false otherwise.</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetLoop(bool isLoop)
        {
            Loop = isLoop;
            return this;
        }

        /// <summary>
        /// Sets the type of the sound (e.g., SFX, BGM).
        /// </summary>
        /// <param name="soundType">The type of the sound.</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetVolumeType(SoundType soundType)
        {
            M_SoundType = soundType;
            return this;
        }

        /// <summary>
        /// Sets the minimum and maximum distance for the sound.
        /// Ensures that min is not greater than max and that min is non-negative.
        /// If invalid values are provided, default values are used.
        /// </summary>
        /// <param name="min">The minimum distance. Must be >= 0.</param>
        /// <param name="max">The maximum distance. Must be >= min.</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetDistance(float min, float max)
        {
            if (min > max)
            {
                MinDistance = 1f;
                MaxDistance = 500f;
                return this;
            }

            if (min < 0) min = 0;
            
            MinDistance = min;
            MaxDistance = max;
            return this;
        }

        /// <summary>
        /// Sets the volume of the sound.
        /// The volume is clamped between 0 and 1 to ensure valid values.
        /// </summary>
        /// <param name="volume">The desired volume level (0 to 1).</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetVolume(float volume)
        {
            Volume = Mathf.Clamp01(volume);
            return this;
        }

        /// <summary>
        /// Sets the pitch of the sound.
        /// The pitch is clamped between -3 and 3 to ensure valid values.
        /// </summary>
        /// <param name="pitch">The desired pitch value (-3 to 3).</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetPitch(float pitch)
        {
            Pitch = Mathf.Clamp(pitch, -3f, 3f);
            return this;
        }

        /// <summary>
        /// Sets the stereo pan of the sound.
        /// The stereo pan is clamped between -1 and 1 to ensure valid values.
        /// </summary>
        /// <param name="stereoPan">The desired stereo pan value (-1 to 1).</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetStereoPan(float stereoPan)
        {
            StereoPan = Mathf.Clamp(stereoPan, -1f, 1f);
            return this;
        }

        /// <summary>
        /// Sets the spatial blend of the sound.
        /// The spatial blend is clamped between 0 and 1 to ensure valid values.
        /// A value of 0 means fully 2D, while 1 means fully 3D.
        /// </summary>
        /// <param name="spatialBlend">The desired spatial blend value (0 to 1).</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetSpatialBlend(float spatialBlend)
        {
            SpatialBlend = Mathf.Clamp01(spatialBlend);
            return this;
        }

        /// <summary>
        /// Sets the target transform that the sound will follow.
        /// If null, the sound will not follow any target.
        /// </summary>
        /// <param name="target">The target transform to follow, or null if no target.</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetFollowTarget(Transform target)
        {
            FollowTarget = target;
            return this;
        }

        /// <summary>
        /// Sets the fade duration for the sound.
        /// A value of 0 means no fade effect will be applied.
        /// </summary>
        /// <param name="duration">The desired fade duration in seconds.</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetFade(float duration)
        {
            if (duration <= 0)
            {
                IsFade = false;
                return this;
            }
            
            IsFade = true;
            FadeDuration = duration;
            return this;
        }

        /// <summary>
        /// Sets the position of the sound in world space.
        /// </summary>
        /// <param name="position">The desired position in world space.</param>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound SetPosition(Vector3 position)
        {
            Position = position;
            return this;
        }
        
        /// <summary>
        /// Resets the Sound object to its default state.
        /// This method is typically used when returning the object to the object pool.
        /// </summary>
        /// <returns>The current Sound instance for method chaining.</returns>
        public Sound Reset()
        {
            Position = Vector3.zero;
            Volume = 1f;
            ClipName = string.Empty;
            IsFade = false;
            FadeDuration = 0f;
            Pitch = 1f;
            StereoPan = 0f;
            SpatialBlend = 0f;
            MinDistance = 1f;
            MaxDistance = 500f;
            Loop = false;
            M_SoundType = SoundType.SFX;
            RolloffMode = AudioRolloffMode.Logarithmic;
            FollowTarget = null;
            return this;
        }

        /// <summary>
        /// Retrieves a new Sound object from the object pool and resets it to its default state.
        /// </summary>
        /// <returns>A Sound instance ready for configuration and use.</returns>
        public static Sound GetNewSound() => GenericPool<Sound>.Get().Reset();
        
        /// <summary>
        /// Plays the sound using the SoundManager and returns the Sound object to the object pool.
        /// If the SoundManager does not exist, logs a warning and returns the object to the pool immediately.
        /// </summary>
        public void PlayWithPool()
        {
            if (!SoundManager.IsExist)
            {
                LogCommon.LogWarning("Sound Manager Not Exist");
                GenericPool<Sound>.Return(this);
                return;
            }
            
            SoundManager.Instance.PlaySound(this);
            GenericPool<Sound>.Return(this);
        }

        /// <summary>
        /// Plays the sound using the SoundManager.
        /// If the SoundManager does not exist, logs a warning and exits without playing the sound.
        /// </summary>
        public void PLay()
        {
            if (!SoundManager.IsExist)
            {
                LogCommon.LogWarning("Sound Manager Not Exist");
                return;
            }
            
            SoundManager.Instance.PlaySound(this);
        }
    }
}