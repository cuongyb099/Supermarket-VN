using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Interact
{
    public class LightSwitch : InteractObject
    {
        [SerializeField] protected List<Transform> lights;
        public bool IsTurnOn { get; protected set; }
        
        protected override void Awake()
        {
            base.Awake();
        }

        protected void Start()
        {
            foreach (Transform light in lights)
            {
                light.gameObject.SetActive(IsTurnOn);
            }
        }

        protected override void OnInteract(Transform source)
        {
            this.CanInteract = false;
            this.ObjectOutline.DisableOutline();
            TurnOnLight();

            DOVirtual.DelayedCall(0.1f, () =>
            {
                CanInteract = true;
            });
        }

        private void TurnOnLight()
        {
            IsTurnOn = !IsTurnOn;
            foreach(Transform light  in lights)
            {
                light.gameObject.SetActive(IsTurnOn);
            }
        }
    }
}
