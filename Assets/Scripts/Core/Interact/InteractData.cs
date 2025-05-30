using UnityEngine;

namespace Core.Interact
{
    [System.Serializable]
    public class InteractData
    {
        [field: SerializeField] public float RayDistance { get; private set; } = 2;
        [field: SerializeField] public int CheckPerSecond { get; private set; } = 8;
        public InteractObject CurrentTarget;
        public Transform CamTransform => CameraManager.Instance.MainCamera.transform;
        public RaycastHit[] RayHits = new RaycastHit[10];
        public Collider[] OverlapHits = new Collider[10];
        public InteractMode CurrentInteractMode;
        public IIndicatable CurrentIndicatable;
        public IPlacable CurrentPlaceObject;
    }
}
