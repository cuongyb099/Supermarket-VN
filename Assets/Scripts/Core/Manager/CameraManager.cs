using Cinemachine;
using KatLib.Singleton;
using UnityEngine;

namespace Core
{
    public class CameraManager : Singleton<CameraManager>
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera VirtualCamera { get; private set; }
        public CinemachinePOV CamPOV { get; private set; }
        private Transform _currentVirtualCamera;
        
        private void Reset()
        {
            MainCamera = GetComponentInChildren<Camera>();
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        
        public GameObject cam;

        [ContextMenu("tes")]
        public void Test()
        {
            SwitchVitualCamera(cam.transform);
        }
        
        protected override void Awake()
        {
            base.Awake();
        
            if(!VirtualCamera) return;
            
            CamPOV = VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
            _currentVirtualCamera = VirtualCamera.transform;
        }

        public void SwitchVitualCamera(Transform newVirtualCamera)
        {
            _currentVirtualCamera.gameObject.SetActive(false);
            _currentVirtualCamera = newVirtualCamera;
            newVirtualCamera.gameObject.SetActive(true);
        }

        [ContextMenu("To Default Vitual Camera")]
        public void ToDefaultVitualCamera()
        {
            if (_currentVirtualCamera)
            {
                _currentVirtualCamera.gameObject.SetActive(false);
            }
            
            Transform virtualCameraTransform = VirtualCamera.transform;
            
            if (_currentVirtualCamera == virtualCameraTransform) return;

            SwitchVitualCamera(virtualCameraTransform);
        }
    }
}
