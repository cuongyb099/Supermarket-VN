using Core.Interact;
using UnityEngine;
//Testing Feature
public class Box : InteractObject
{
    protected Collider objectCollider;
    protected Rigidbody rb;
    
    protected override void Awake()
    {
        base.Awake();
        objectCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnInteract(Transform source)
    {
        if(!source.TryGetComponent(out Interactor interactor)) return;
        ObjectOutline.DisableOutline();
        objectCollider.isTrigger = true;
        rb.isKinematic = true;
        interactor.AttachItemToHand(this);
    }

    public override void ResetToIdle()
    {
        objectCollider.isTrigger = false;
        rb.isKinematic = false;
        this.CanInteract = true;
        this.transform.SetParent(null);
    }
}