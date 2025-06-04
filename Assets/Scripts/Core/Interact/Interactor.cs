using System.Collections;
using Core.Interact.Interact_Mode;
using KatLib.State_Machine;
using UnityEngine;

namespace Core.Interact
{
    public enum InteractMode
    {
        Idle,
        HoldingItem,
        PlacingObject,
    }
    
    public class Interactor : MonoBehaviour
    {
        protected Coroutine coroutine;
        [SerializeField] protected InteractData interactData;
    
        protected StateMachine<InteractMode, InteractState> stateMachine = new ();
    
        protected IdleState idle;
        protected HoldingItemState holdingItem;
        protected PlaceState place;
        
        private void Awake()
        {
            idle = new IdleState(interactData, this, stateMachine);
            holdingItem = new HoldingItemState(interactData, this, stateMachine);
            place = new PlaceState(interactData, this, stateMachine);
            
            stateMachine.AddNewState(InteractMode.Idle, idle);
            stateMachine.AddNewState(InteractMode.HoldingItem, holdingItem);
            stateMachine.AddNewState(InteractMode.PlacingObject, place);
        }

        protected virtual void Start()
        {
            stateMachine.Initialize(InteractMode.Idle);
            coroutine = StartCoroutine(InteractUpdate());       
        }

        protected virtual IEnumerator InteractUpdate()
        {
            while (true)
            {
                yield return stateMachine.CurrentState.OnUpdate();
            }            
        }

        public void AttachItemToHand(ObjectAttackToHand item) => holdingItem.AttachItemToHand(item);
        public InteractObject CurrentObjectInHand => interactData.CurrentTargetFristSlot;
        public InteractMode CurrentInteractMode => stateMachine.CurrentStateID;
        
#if UNITY_EDITOR
        public bool Debugging;
        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying || !Debugging) return;
            
            stateMachine.CurrentState.OnDrawGizmos();
        }
#endif
    }
}
