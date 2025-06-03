using KatLib.State_Machine;
using UnityEngine;

namespace Core.Interact.Interact_Mode
{
    public abstract class InteractState : BaseState
    {
        protected InteractData data;
        protected Interactor interactor;
        protected StateMachine<InteractMode, InteractState> stateMachine;
        
        public InteractState(InteractData data, Interactor interactor, StateMachine<InteractMode, InteractState> stateMachine)
        {
            this.data = data;
            this.interactor = interactor;
            this.stateMachine = stateMachine;
        }

        public abstract YieldInstruction OnUpdate();

        public override void Enter()
        {
			
        }

        public override void Exit()
        {
			
        }
        
#if UNITY_EDITOR
        public virtual void OnDrawGizmos()
        {
            
        }
#endif
    }
}

