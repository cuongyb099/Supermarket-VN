using Core.Interact.Interact_Mode;
using UnityEngine;

namespace Core.Interact
{
    public abstract class ObjectAttackToHand : InteractObject, IRenderOnTopHandle
    {
        [field: SerializeField] public virtual HoldingItemState.HandPositionType HandPosition { get; protected set; }
        protected RenderOnTop renderOnTop;
        protected bool isTopLayer;
        
        protected override void Awake()
        {
            base.Awake();
            renderOnTop = GetComponent<RenderOnTop>();
        }

        protected override void OnInteract(Interactor source)
        {
            source.GetComponent<Interactor>().AttachItemToHand(this);
            SetOnTop();
        }

        public override void ResetToIdle()
        {
            base.ResetToIdle();
            CanInteract = true;
            ToDefaultRender();
        }

        public virtual void SetOnTop()
        {
            if(isTopLayer) return;
            
            renderOnTop.SetOnTop();
            isTopLayer = true;
        }

        public virtual void ToDefaultRender()
        {
            if(!isTopLayer) return;
            
            renderOnTop.ReturnDefault();
            isTopLayer = false;
        }
    }
}
