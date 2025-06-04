using System;
using System.Collections.Generic;
using Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interact
{
    [Serializable]
    public class InteractData
    {
        [field: SerializeField] public float RayDistance { get; private set; } = 2;
        [field: SerializeField] public LayerMask RaycastLayer { get; private set; }
        
        public Transform CamTransform => CameraManager.Instance.MainCamera.transform;
        
        [NonSerialized]public Transform CurrentHandTransform;
        [NonSerialized] public RaycastHit[] RayHits = new RaycastHit[10];
        [NonSerialized] public Collider[] OverlapHits = new Collider[10];
        
        public InteractObject CurrentTargetFristSlot;
        public InteractObject CurrentTargetSecondSlot;
        
        public InputMap.DefaultActions DefaultMap => InputManager.Instance.PlayerInputMap.Default;
        public InputAction PlacePerform => DefaultMap.Interact;
        public InputAction RotateAction => DefaultMap.RotateItem;
        public InputAction ExitAction => DefaultMap.Exit;
        public InputAction IntoPlaceModeAction => DefaultMap.IntoPlaceMode;
        public InputAction ThrowAction => DefaultMap.Throw;
        public InputAction LeftInteract => DefaultMap.Interact;
        public InputAction RightInteract => DefaultMap.Interact2;
        
        [field: Header("HoldingItem Config")]
        [field: SerializeField] public float ReturnHandDuration { get; private set; } = 0.2f;
        [field: SerializeField] public Transform LeftHandPos{ get; private set; }
        [field: SerializeField] public Transform MiddleHandPos{ get; private set; }
        [field: SerializeField] public float ThrowForce { get; private set; } = 10f;
        [field: SerializeField] public float MinThrowAngle { get; private set; } = 30f;
        
        [field: Header("PlaceMode Config")]
        [field: SerializeField] public Color CanPlaceColor { get; private set; } = Color.green;
        [field: SerializeField] public Color CannotPlaceColor { get; private set; } = Color.red;
        [field: SerializeField] public List<string> OverlapTagsCheck{ get; private set; }
        [field: SerializeField] public float AngleEachRotate { get; private set; } = 15f;

        public void ResetTargetSlot(ref InteractObject targetSlot)
        {
            targetSlot = null;
            if(targetSlot == CurrentTargetSecondSlot) return;
            CurrentHandTransform = null;
        }
    }
}
