using Core.Event;
using Core.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interact.Interact_Mode
{
    public enum HandPositionType
    {
        Left,
        Middle,
    }
    
    [System.Serializable]
    public class OnHandMode : InteractModeBase
    {
        [SerializeField] protected  float returnHandDuration = 0.2f;
        [SerializeField] protected Transform leftHandPos;
        [SerializeField] protected Transform middleHandPos;
        [SerializeField] protected float throwForce = 10f;
        [SerializeField] protected float minThrowAngle = 30f;
        protected InputAction intoPlaceModeAction;
        protected InputAction throwAction;
        
        //Message
        public const string CurrentAngleLessMinAngle = "Cannot Throw With Current Angle"; 
        
        public override YieldInstruction OnUpdate()
        {
            if (intoPlaceModeAction.WasPressedThisFrame())
            {
                SwitchToPlaceMode();
                return null;
            }

            if (!throwAction.WasPressedThisFrame()) return null;
            float currentAngle = Vector3.Angle(data.CamTransform.forward, Vector3.down);

            if (currentAngle < minThrowAngle)
            {
                InteractEvent.OnThrowIgnore?.Invoke(CurrentAngleLessMinAngle);
                return null;
            }
            ThrowObject();
            
            return null;
        }

        public override void Init(Interactor interactor, InteractData interactData)
        {
            base.Init(interactor, interactData);
            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            
            intoPlaceModeAction = defaultActions.IntoPlaceMode;
            throwAction = defaultActions.Throw;
        }

        public void AttachItemToHand(ObjectAttackToHand item)
        {
            if(!item) return;
            
            this.data.CurrentTarget = item;
            data.CurrentInteractMode = InteractMode.OnHand;
            item.SetActiveCollision(false);
            Transform interactTransform = item.transform;
            var curHand = GetHandTransform(item.HandPosition);
            this.data.CurrentHandTransform = curHand;
            interactTransform.SetParent(curHand);
            interactTransform.DOLocalMove(Vector3.zero, returnHandDuration);
            interactTransform.DOLocalRotate(Vector3.zero, returnHandDuration);
        }
        
        private void SwitchToPlaceMode()
        {
            if(this.data.CurrentInteractMode != InteractMode.OnHand) return;
            
            this.data.CurrentInteractMode = InteractMode.Place;
            var CurrentTarget = this.data.CurrentTarget;
            Transform targetTransform = CurrentTarget.transform;
            data.CurrentPlaceObject = targetTransform.GetComponent<IPlacable>();
        }
        
        public Transform GetHandTransform(HandPositionType handPositionType)
        {
            switch (handPositionType)
            {
                case HandPositionType.Left:
                    return leftHandPos;
                case HandPositionType.Middle:
                    return middleHandPos;
            }

            return null;
        }
        
        public void ThrowObject()
        {
            var target = data.CurrentTarget;
            target.transform.SetParent(null);
            target.ResetToIdle();
            var rb = target.GetComponent<Rigidbody>();
            rb.AddForce(data.CamTransform.forward * throwForce, ForceMode.VelocityChange);
            data.ResetCurrentTarget();
            data.CurrentInteractMode = InteractMode.Default;
        }
    }
}
