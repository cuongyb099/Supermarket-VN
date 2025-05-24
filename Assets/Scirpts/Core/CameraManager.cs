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
        
        private void Reset()
        {
            MainCamera = GetComponentInChildren<Camera>();
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        protected override void Awake()
        {
            base.Awake();
            if(!VirtualCamera) return;
            CamPOV = VirtualCamera.GetCinemachineComponent<CinemachinePOV>();
        }
    }
}
