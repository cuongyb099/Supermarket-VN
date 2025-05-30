using System;
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
            _currentVirtualCamera = VirtualCamera.transform;
        }

        protected override void Awake()
        {
            base.Awake();
            if(!VirtualCamera) return;
            CamPOV = VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }

        public void SwitchVitualCamera(Transform newVirtualCamera)
        {
            _currentVirtualCamera.gameObject.SetActive(false);
            _currentVirtualCamera = newVirtualCamera;
            newVirtualCamera.gameObject.SetActive(true);
        }

        public void ToDefaultVitualCamera()
        {
            Transform virtualCameraTransform = VirtualCamera.transform;
            
            if (_currentVirtualCamera == virtualCameraTransform) return;

            SwitchVitualCamera(virtualCameraTransform);
        }
    }
}
