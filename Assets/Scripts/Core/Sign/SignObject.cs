using Core.Interact;
using UnityEngine;

namespace Core.Sign
{
    public class SignObject : InteractObject
    {
        private SignSystem _signSystem;
        protected override void Awake()
        {
            base.Awake();
            this.CanInteract = true;
            this._signSystem = GetComponent<SignSystem>();
        }

        protected override void OnInteract(Transform source)
        {
            // this.CanInteract = false;
            // this.ObjectOutline.DisableOutline();
            Debug.Log("SignObject OnInteract");
            if (!this._signSystem) return;
            this._signSystem.StatusChange();
        }
    }
}