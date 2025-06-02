using UnityEngine;

namespace Core.AnimationEventSystem.EventData
{
    public abstract class AnimEventSO : ScriptableObject
    {
        [field: SerializeField]
        public string Key { get; protected set; }

        [field: SerializeField, Range(0, 0.99f)]
        public float NormalizeTimeCall { get; protected set; }

        public abstract void Raise(AnimationEventReceiver receiver);
    }
}
