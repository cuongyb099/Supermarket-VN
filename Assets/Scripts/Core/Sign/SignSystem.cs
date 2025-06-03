using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Sign
{
    public class SignSystem : MonoBehaviour
    {
        [SerializeField]
        public bool StatusReverse;
        
        [SerializeField]
        public bool statusOpened;
        
        private Action _continueSpawnCallback;
        private Action _pauseSpawnCallback;

        public Action ContinueSpawnCallback
        {
            get => _continueSpawnCallback;
            set => _continueSpawnCallback = value;
        }

        public Action PauseSpawnCallback
        {
            get => _pauseSpawnCallback;
            set => _pauseSpawnCallback = value;
        }

        private SignRender _signRender;
        
        private void Awake()
        {
            // Test
            _continueSpawnCallback += CallbackOpen;
            _pauseSpawnCallback += CallbackClose;
            
            StatusReverse = false;
            statusOpened = true;
            
            _signRender = GetComponent<SignRender>();
        }

        void OnClose()
        {
            _pauseSpawnCallback?.Invoke();
            if (!_signRender)
            {
                return;
            }
            _signRender.ShowClose();
        }

        void OnOpen()
        {
            _continueSpawnCallback?.Invoke();
            if (!_signRender)
            {
                return;
            }
            _signRender.ShowOpen();
        }

        // public void CheckStatus()
        // {
        //     if (!StatusReverse) return;
        //     
        //     if (statusOpened)
        //     {
        //         OnClose();
        //         statusOpened = false;
        //     }
        //     else
        //     {
        //         OnOpen();
        //         statusOpened = true;
        //     }
        //
        //     StatusReverse = false;
        // }

        public void StatusChange()
        {
            if (statusOpened)
            {
                OnClose();
                statusOpened = false;
            }
            else
            {
                OnOpen();
                statusOpened = true;
            }
        }
        // private void Update()
        // {
        //     this.CheckStatus();
        // }
        
        //Test
        private void CallbackClose()
        {
            Debug.Log("Pause spawn");
        }

        private void CallbackOpen()
        {
            Debug.Log("Continue spawn");
        }
    }
}