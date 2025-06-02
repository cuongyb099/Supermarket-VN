using Core.Entity.Common;
using Core.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Entity
{
    [RequireComponent(typeof(Movement))]
    public class FirstPersonCharacterCtrl : MonoBehaviour
    {
        public Movement MovementCtrl { get; private set; }
        [SerializeField] protected Transform pivotLook;
        
        [Header("Speed")]
        [SerializeField] protected float walkSpeed = 2f;
        [SerializeField] protected float runSpeed = 5f;
        [SerializeField] protected float timeToReachSpeed = 0.25f;
        protected Tween speedTween;
        
        private void Awake()
        {
            MovementCtrl = GetComponent<Movement>();
            MovementCtrl.Speed = walkSpeed;
            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            InputAction RunAction = defaultActions.Run;
            
            RunAction.performed += HandleRunPerfomed;
            RunAction.canceled += HandleRunCanceled;
        }

        private void OnDestroy()
        {
            if(!InputManager.IsExist) return;
            
            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            InputAction RunAction = defaultActions.Run;
            
            RunAction.performed -= HandleRunPerfomed;
            RunAction.canceled -= HandleRunCanceled;
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            MovementCtrl.FloatingCapsule();
        }

        private void Update()
        {
            UpdateRotation();
        }

        private void HandleRunCanceled(InputAction.CallbackContext context)
        {
            UpdateCharacterSpeed(walkSpeed);
        }


        private void HandleRunPerfomed(InputAction.CallbackContext context)
        {
           UpdateCharacterSpeed(runSpeed);
        }
        
        private void UpdateCharacterSpeed(float characterSpeed)
        {
            if(speedTween != null && speedTween.IsActive()) speedTween.Kill();
            
            speedTween = DOVirtual.Float(MovementCtrl.Speed, characterSpeed, timeToReachSpeed, (speed) =>
            {
                MovementCtrl.Speed = speed;
            })
            .SetEase(Ease.Linear);
        }
        
        private void UpdateRotation()
        {
            const float smoothSpeed = 30f;
            Transform camTransform = CameraManager.Instance.MainCamera.transform;
            Vector3 camForward = Vector3.ProjectOnPlane(camTransform.forward, Vector3.up);
            Rigidbody rb = MovementCtrl.MovementRigidbody;
            
            if (camForward != Vector3.zero)
            {
                rb.MoveRotation(Quaternion.Lerp(rb.rotation, Quaternion.LookRotation(camForward, Vector3.up), smoothSpeed * Time.deltaTime));
            }
            
            pivotLook.rotation = Quaternion.LookRotation(camTransform.forward, camTransform.up);
        }

        private void UpdateMovement()
        {
            Vector3 moveInput = InputManager.Instance.MoveInput;

            Transform camTransform = CameraManager.Instance.MainCamera.transform;
            Vector3 camForward = Vector3.ProjectOnPlane(camTransform.forward, Vector3.up);
            Vector3 camRight = Vector3.ProjectOnPlane(camTransform.right, Vector3.up);
            Vector3 moveDir = camForward * moveInput.z + camRight * moveInput.x;
            MovementCtrl.Move(moveDir);
        }
    }
}
