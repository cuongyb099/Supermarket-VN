using UnityEngine;

namespace Core.AnimationEventSystem.EventData
{
    [CreateAssetMenu(menuName = "Animation Event Trigger/Int Param")]
    public class AnimEventIntSO : AnimEventSO
    {
        [SerializeField] protected int intValue;

        public override void Raise(AnimationEventReceiver receiver)
        {
            receiver.RaiseEvent(this.Key, intValue);
        }
    }
}
