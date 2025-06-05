using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

namespace Core
{
    public class CameraManager : KatLib.Singleton.Singleton<CameraManager>
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera GameplayCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera Default { get; private set; }
        
        public CinemachinePOV CamPOV { get; private set; }
        private CinemachineVirtualCamera _currentVirtualCamera;
        
        private void Reset()
        {
            MainCamera = GetComponentInChildren<Camera>();
            GameplayCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }
        
        protected override void Awake()
        {
            base.Awake();
            
            if(!GameplayCamera) return;
            
            MainCamera.GetOrAddComponent<AudioListener>();
            CamPOV = GameplayCamera.GetCinemachineComponent<CinemachinePOV>();
            _currentVirtualCamera = Default;
        }

        public void SwitchVitualCamera(CinemachineVirtualCamera newVirtualCamera)
        {
            if(!newVirtualCamera) return;
            
            _currentVirtualCamera.gameObject.SetActive(false);
            _currentVirtualCamera = newVirtualCamera;
            newVirtualCamera.gameObject.SetActive(true);
        }
    }
}
