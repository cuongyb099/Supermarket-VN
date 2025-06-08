using Core.Event;
using DG.Tweening;
using KatLib.State_Machine;
using UnityEngine;

namespace Core.Interact.Interact_Mode
{
    [System.Serializable]
    public class HoldingItemState : IdleState
    {
        public enum HandPositionType
        {
            Left,
            Middle,
        }
        
        public const string CurrentAngleLessMinAngle = "Cannot Throw With Current Angle";


        public HoldingItemState(InteractData data, Interactor interactor, StateMachine<InteractMode, InteractState> stateMachine) 
            : base(data, interactor, stateMachine)
        {
        }

        public override YieldInstruction OnUpdate()
        {
            if (data.LeftInteract.WasPressedThisFrame() || data.RightInteract.WasPressedThisFrame())
            {
                data.CurrentTargetSecondSlot?.Interact(this.interactor);
            }
            
            if (stateMachine.CurrentStateID != InteractMode.HoldingItem)
            {
                return null;
            }
            
            CheckInteractObject(ref data.CurrentTargetSecondSlot);
            
            if (data.IntoPlaceModeAction.WasPressedThisFrame())
            {
                SwitchToPlaceMode();
                return null;
            }

            if (!data.ThrowAction.WasPressedThisFrame()) return null;
            float currentAngle = Vector3.Angle(data.CamTransform.forward, Vector3.down);

            if (currentAngle < data.MinThrowAngle)
            {
                InteractEvent.OnThrowIgnore?.Invoke(CurrentAngleLessMinAngle);
                return null;
            }
            ThrowObject();
            
            return null;
        }

        public void AttachItemToHand(ObjectAttackToHand item)
        {
            if(!item || data.CurrentTargetFristSlot != item) return;
            
            item.SetActiveCollision(false);
            Transform interactTransform = item.transform;
            var curHand = GetHandTransform(item.HandPosition);
            this.data.CurrentHandTransform = curHand;
            interactTransform.SetParent(curHand);
            interactTransform.DOLocalMove(Vector3.zero, data.ReturnHandDuration);
            interactTransform.DOLocalRotate(Vector3.zero, data.ReturnHandDuration);
            stateMachine.ChangeState(InteractMode.HoldingItem);
        }
        
        private void SwitchToPlaceMode()
        {
            var CurrentTarget = this.data.CurrentTargetFristSlot;
            
            if(CurrentTarget is not IPlacable) return;
            
            stateMachine.ChangeState(InteractMode.PlacingObject);
        }
        
        public Transform GetHandTransform(HandPositionType handPositionType)
        {
            switch (handPositionType)
            {
                case HandPositionType.Left:
                    return data.LeftHandPos;
                case HandPositionType.Middle:
                    return data.MiddleHandPos;
            }

            return null;
        }
        
        public void ThrowObject()
        {
            var target = data.CurrentTargetFristSlot;
            
            if(!target.TryGetComponent(out IThrowable throwable)) return;
            
            target.transform.SetParent(null);
            target.ResetToIdle();
            data.ResetTargetSlot(ref data.CurrentTargetFristSlot);
            throwable.Throw(data.CamTransform.forward, data.ThrowForce);
            stateMachine.ChangeState(InteractMode.Idle);
        }
    }
}

