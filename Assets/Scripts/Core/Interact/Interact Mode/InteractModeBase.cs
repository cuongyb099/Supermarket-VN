using UnityEngine;

namespace Core.Interact.Interact_Mode
{
    public abstract class InteractModeBase
    {
        protected Interactor interactor;
        protected InteractData data;
        
        public virtual void Init(Interactor interactor, InteractData interactData)
        {
            this.data = interactData;
            this.interactor = interactor;
        }       
        
        public abstract YieldInstruction OnUpdate();
        public virtual void Dispose(){}
        public virtual void OnDrawGizmos(){}
    }
}
