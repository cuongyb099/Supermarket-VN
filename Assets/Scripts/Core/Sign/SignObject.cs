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
            CanInteract = true;
            // ObjectOutline.
            _signSystem = GetComponent<SignSystem>();
        }

        protected void OnInteract(Interactor source)
        {
            // this.CanInteract = false;
            // this.ObjectOutline.DisableOutline();
            Debug.Log("SignObject OnInteract");
            if (!_signSystem) return;
            _signSystem.StatusChange();
        }
    }
}