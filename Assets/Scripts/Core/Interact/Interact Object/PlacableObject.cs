using UnityEngine;

namespace Core.Interact
{
    public class PlacableObject : ObjectAttackToHand, IPlacable
    { 
        protected Collider fur_collider;
        protected Rigidbody rb;
        [SerializeField] protected PlaceHitbox placeHitBox;

        protected virtual void Reset()
        {
            placeHitBox = GetComponentInChildren<PlaceHitbox>();
        }

        protected override void Awake()
        {
            base.Awake();
            fur_collider = GetComponent<Collider>();
            rb = GetComponent<Rigidbody>();
        }

        protected override void OnInteract(Transform source)
        {
            CanInteract = false;
            base.OnInteract(source);
            this.ObjectOutline.DisableOutline();
        }

        public override void SetActiveCollision(bool value)
        {
            fur_collider.enabled = value;
            rb.isKinematic = !value;
        }

        public PlaceHitbox GetPlaceHitBox() => placeHitBox;
    }
}
