using KatLib.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    public class InputManager : Singleton<InputManager>
    {
        private InputMap _inputMap;
        
        //Action
        public InputAction LookAction { get; private set; }
        public InputAction MoveAction { get; private set; }
        
        //Property
        public Vector3 MoveInput { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        private void Init()
        {
            _inputMap = new InputMap();
            _inputMap.Enable();
            MoveAction = _inputMap.Default.Move;
            LookAction = _inputMap.Default.Look;

            MoveAction.performed += HandleMovePerformed;
            MoveAction.canceled += HandleMoveCanceled;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            _inputMap.Disable();
            
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
