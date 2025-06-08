using UnityEngine;

namespace Core.AnimationEventSystem.EventData
{
    [CreateAssetMenu(menuName = "Animation Event Trigger/No Param")]
    public class AnimEventNoParamSO : AnimEventSO
    {
        public override void Raise(AnimationEventReceiver receiver)
        {
            receiver.RaiseEvent(this.Key);
        }
    }
}
