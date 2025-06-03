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
            get => this._continueSpawnCallback;
            set => this._continueSpawnCallback = value;
        }

        public Action PauseSpawnCallback
        {
            get => this._pauseSpawnCallback;
            set => this._pauseSpawnCallback = value;
        }

        private SignRender _signRender;
        
        private void Awake()
        {
            // Test
            this._continueSpawnCallback += CallbackOpen;
            this._pauseSpawnCallback += CallbackClose;
            
            this.StatusReverse = false;
            this.statusOpened = true;
            
            this._signRender = GetComponent<SignRender>();
        }

        void OnClose()
        {
            this._pauseSpawnCallback?.Invoke();
            if (!this._signRender)
            {
                return;
            }
            this._signRender.ShowClose();
        }

        void OnOpen()
        {
            this._continueSpawnCallback?.Invoke();
            if (!this._signRender)
            {
                return;
            }
            this._signRender.ShowOpen();
        }

        public void CheckStatus()
        {
            if (!this.StatusReverse) return;
            
            if (this.statusOpened)
            {
                this.OnClose();
                this.statusOpened = false;
            }
            else
            {
                this.OnOpen();
                statusOpened = true;
            }

            this.StatusReverse = false;
        }

        public void StatusChange()
        {
            if (this.statusOpened)
            {
                this.OnClose();
                this.statusOpened = false;
            }
            else
            {
                this.OnOpen();
                statusOpened = true;
            }
        }
        private void Update()
        {
            this.CheckStatus();
        }
        
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