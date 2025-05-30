using Core.Collision;
using Core.Input;
using DG.Tweening;
using Unity.VisualScripting;
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
        [SerializeField] protected Transform leftHandPos;
        [SerializeField] protected Transform middleHandPos;
        [SerializeField] protected float itemReturnHandTime = 0.2f;
        
        public override YieldInstruction OnUpdate() => null;

        public override void Init(Interactor interactor, InteractData interactData)
        {
            base.Init(interactor, interactData);
            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            
            InputAction IntoPlaceModeAction = defaultActions.IntoPlaceMode;
            IntoPlaceModeAction.performed += SwitchToPlaceMode;
        }

        public override void Dispose()
        {
            if(!InputManager.IsExist) return;
            
            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            InputAction IntoPlaceModeAction = defaultActions.IntoPlaceMode;
            
            IntoPlaceModeAction.performed -= SwitchToPlaceMode;
        }

        public void AttachItemToHand(ObjectAttackToHand item)
        {
            if(!item) return;
            
            this.data.CurrentTarget = item;
            data.CurrentInteractMode = InteractMode.OnHand;
            item.SetActiveCollision(false);
            Transform interactTransform = item.transform;
            interactTransform.SetParent(GetHandTransform(item.HandPosition));
            interactTransform.DOLocalMove(Vector3.zero, itemReturnHandTime);
            interactTransform.DOLocalRotate(Vector3.zero, itemReturnHandTime);
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
        
        private void SwitchToPlaceMode(InputAction.CallbackContext context)
        {
            if(this.data.CurrentInteractMode != InteractMode.OnHand) return;
            
            this.data.CurrentInteractMode = InteractMode.Place;
            var CurrentTarget = this.data.CurrentTarget;
            Transform targetTransform = CurrentTarget.transform;
            data.CurrentPlaceObject = targetTransform.GetComponent<IPlacable>();
            targetTransform.SetParent(null);
        }
    }
}
