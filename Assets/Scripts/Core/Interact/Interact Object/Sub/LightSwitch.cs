using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Interact
{
    public class LightSwitch : InteractObject
    {
        [SerializeField] protected List<Transform> lights;
        public bool IsTurnOn { get; protected set; }
        
        protected void Start()
        {
            foreach (Transform childLight in lights)
            {
                childLight.gameObject.SetActive(IsTurnOn);
            }
        }

        protected override void OnInteract(Interactor source)
        {
            this.CanInteract = false;
            this.outline.DisableOutline();
            TurnOnLight();

            DOVirtual.DelayedCall(0.1f, () =>
            {
                CanInteract = true;
            });
        }

        private void TurnOnLight()
        {
            IsTurnOn = !IsTurnOn;
            foreach(Transform childLight  in lights)
            {
                childLight.gameObject.SetActive(IsTurnOn);
            }
        }
    }
}
