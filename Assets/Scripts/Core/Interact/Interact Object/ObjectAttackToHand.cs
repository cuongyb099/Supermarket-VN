using Core.Interact.Interact_Mode;
using UnityEngine;

namespace Core.Interact
{
    public abstract class ObjectAttackToHand : InteractObject
    {
        [field: SerializeField] public virtual HandPositionType HandPosition { get; protected set; }
        
        protected override void OnInteract(Transform source)
        {
            source.GetComponent<Interactor>().AttachItemToHand(this);
        }

        public override void ResetToIdle()
        {
            base.ResetToIdle();
            CanInteract = true;
        }
    }
}
