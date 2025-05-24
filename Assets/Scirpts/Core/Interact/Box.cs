using Core.Entity.Common;
using Core.Interact;
using UnityEngine;

public class Box : InteractObject
{
    protected Collider objectCollider;
    protected Rigidbody rb;
    
    private void Awake()
    {
        objectCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    protected override void OnInteract(Transform source)
    {
        if(!source.TryGetComponent(out Interactor interactor)) return;
        HideOutline();
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