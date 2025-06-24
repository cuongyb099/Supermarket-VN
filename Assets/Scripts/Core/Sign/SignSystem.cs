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
        public bool StatusOpened;
        
        static public Action ContinueSpawnCallback;
        static public Action PauseSpawnCallback;

        private SignRender _signRender;
        
        private void Awake()
        {
            // Test
            ContinueSpawnCallback += CallbackOpen;
            PauseSpawnCallback += CallbackClose;
            
            StatusReverse = false;
            StatusOpened = false;
            
            _signRender = GetComponent<SignRender>();
        }

        void OnClose()
        {
            PauseSpawnCallback?.Invoke();
            if (!_signRender)
            {
                return;
            }
            _signRender.ShowClose();
        }

        void OnOpen()
        {
            ContinueSpawnCallback?.Invoke();
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
            if (StatusOpened)
            {
                OnClose();
                StatusOpened = false;
            }
            else
            {
                OnOpen();
                StatusOpened = true;
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