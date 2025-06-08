using KatLib.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class InputManager : Singleton<InputManager>
    {
        public InputMap PlayerInputMap { get; private set; }
        
        //Property
        public Vector3 MoveInput { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            PlayerInputMap = new InputMap();
            PlayerInputMap.Enable();
            var defaultAction = PlayerInputMap.Default;
            InputAction MoveAction = defaultAction.Move;
            
            MoveAction.performed += HandleMovePerformed;
            MoveAction.canceled += HandleMoveCanceled;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            PlayerInputMap.Disable();
            var defaultAction = PlayerInputMap.Default;
            InputAction MoveAction = defaultAction.Move;
            
            MoveAction.performed -= HandleMovePerformed;
            MoveAction.canceled -= HandleMoveCanceled;
        }

        private void HandleMoveCanceled(InputAction.CallbackContext obj)
        {
            MoveInput = Vector3.zero;
        }
        
        private void HandleMovePerformed(InputAction.CallbackContext context)
        {
            var temp = context.ReadValue<Vector2>();
            MoveInput = new Vector3(temp.x, 0, temp.y);
        }
    }
}
