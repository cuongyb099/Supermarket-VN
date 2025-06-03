using Core.Event;
using DG.Tweening;
using KatLib.State_Machine;
using UnityEngine;

namespace Core.Interact.Interact_Mode
{
    [System.Serializable]
    public class HoldingItemState : InteractState
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
            if(!item) return;
            
            this.data.CurrentTarget = item;
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
            if(stateMachine.CurrentStateID != InteractMode.HoldingItem) return;
            
            var CurrentTarget = this.data.CurrentTarget;
            Transform targetTransform = CurrentTarget.transform;
            data.CurrentPlaceObject = targetTransform.GetComponent<IPlacable>();
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
            var target = data.CurrentTarget;
            target.transform.SetParent(null);
            target.ResetToIdle();
            var rb = target.GetComponent<Rigidbody>();
            rb.AddForce(data.CamTransform.forward * data.ThrowForce, ForceMode.VelocityChange);
            data.ResetCurrentTarget();
            stateMachine.ChangeState(InteractMode.Idle);
        }
    }
}

