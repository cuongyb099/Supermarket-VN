using System;
using UnityEngine;

namespace Core.Interact
{
    [System.Serializable]
    public class InteractData
    {
        [field: SerializeField] public float RayDistance { get; private set; } = 2;
        
        public Transform CurrentHandTransform;
        
        public Transform CamTransform => CameraManager.Instance.MainCamera.transform;
        [NonSerialized] public RaycastHit[] RayHits = new RaycastHit[10];
        [NonSerialized] public Collider[] OverlapHits = new Collider[10];
        
        public InteractMode CurrentInteractMode;
        
        public InteractObject CurrentTarget;
        public IPlacable CurrentPlaceObject;
        public IIndicatable CurrentIndicatable;
        
        public void ResetCurrentTarget()
        {
            CurrentTarget = null;
            CurrentPlaceObject = null;
            CurrentIndicatable = null;
            CurrentHandTransform = null;
        }
    }
}
