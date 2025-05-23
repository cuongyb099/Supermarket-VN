using Core.Entity.Common;
using Core.Input;
using UnityEngine;

namespace Core.Entity
{
    [RequireComponent(typeof(Movement))]
    public class FirstPersonCharacterCtrl : MonoBehaviour
    {
        public Movement MovementCtrl { get; private set; }
        protected Camera CharacterCamera;
        
        private void Awake()
        {
            CharacterCamera = Camera.main;
            MovementCtrl = GetComponent<Movement>();
        }

        
        private void FixedUpdate()
        {
            Vector3 moveInput = InputManager.Instance.MoveInput;

            Vector3 camForward = CharacterCamera.transform.forward;
            Vector3 camRight = CharacterCamera.transform.right;

            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDir = camForward * moveInput.z + camRight * moveInput.x;
            MovementCtrl.Move(moveDir);
        }
    }
}
