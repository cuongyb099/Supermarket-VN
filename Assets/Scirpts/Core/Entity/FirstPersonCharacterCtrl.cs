using Core.Entity.Common;
using Core.Input;
using Core.Manager;
using UnityEngine;

namespace Core.Entity
{
    [RequireComponent(typeof(Movement))]
    public class FirstPersonCharacterCtrl : MonoBehaviour
    {
        public Movement MovementCtrl { get; private set; }
        [SerializeField] protected Transform pivotLook;
        
        private void Awake()
        {
            MovementCtrl = GetComponent<Movement>();
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

        private void UpdateRotation()
        {
            const float smoothSpeed = 30f;
            Transform camTransform = CameraManager.Instance.MainCamera.transform;
            Vector3 camForward = camTransform.forward;
            Rigidbody rb = MovementCtrl.MovementRigidbody;
            camForward.y = 0;
            camForward.Normalize();
            
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
