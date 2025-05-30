using System.Collections;
using Core.Input;
using Core.Interact.Interact_Mode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interact
{
    public enum InteractMode
    {
        Default,
        OnHand,
        Place,
    }
    
    public class Interactor : MonoBehaviour
    {
        protected Coroutine coroutine;
        [SerializeField] protected InteractData interactData;
        [SerializeField] protected DefaultMode defaultModeHandle;
        [SerializeField] protected PlaceMode placeModeHandle;
        [SerializeField] protected OnHandMode onHandModeHandle;

        private void Awake()
        {
            interactData.CurrentInteractMode = InteractMode.Default;
            defaultModeHandle.Init(this, interactData);
            placeModeHandle.Init(this, interactData);
            onHandModeHandle.Init(this, interactData);

            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            InputAction interactAction = defaultActions.Interact;

            interactAction.performed += HandleInteractPerformed;
        }

        protected virtual void Start()
        {
            StartChecking();
        }

        private void OnDestroy()
        {
            defaultModeHandle.Dispose();
            placeModeHandle.Dispose();
            onHandModeHandle.Dispose();
            
            if(!InputManager.IsExist) return;
            
            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            InputAction interactAction = defaultActions.Interact;
            
            interactAction.performed -= HandleInteractPerformed;
        }
        
        private void HandleInteractPerformed(InputAction.CallbackContext context)
        {
            var CurrentTarget = interactData.CurrentTarget;
            if(!CurrentTarget || !CurrentTarget.CanInteract) return;

            switch (interactData.CurrentInteractMode)
            {
                case InteractMode.Default:
                    CurrentTarget.Interact(this.transform);
                    break;
            }
        }

        protected virtual IEnumerator InteractUpdate()
        {
            while (true)
            {
                yield return GetInteractMode().OnUpdate();
            }            
        }
        
        public virtual void StartChecking()
        {
            coroutine = StartCoroutine(InteractUpdate());       
        }
        
        public virtual void StopChecking()
        {
            if(coroutine != null) return;
            
            StopCoroutine(coroutine);
        }

        protected InteractModeBase GetInteractMode()
        {
            switch (interactData.CurrentInteractMode)
            {
                case InteractMode.Default:
                    return defaultModeHandle;
                case InteractMode.OnHand:
                    return onHandModeHandle;
                case InteractMode.Place:
                    return placeModeHandle;
                default:
                    return null;
            }
        }

        public Transform GetCameraTransform(HandPositionType handPositionType) => onHandModeHandle.GetHandTransform(handPositionType);
        public void AttachItemToHand(ObjectAttackToHand item) => onHandModeHandle.AttachItemToHand(item);
        
#if UNITY_EDITOR
        public bool Debugging;
        protected virtual void OnDrawGizmos()
        {
            if(!Application.isPlaying || !Debugging) return;
            
            GetInteractMode().OnDrawGizmos();
        }
#endif
    }
}
