using Core.Interact;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicThrowObject : MonoBehaviour, IThrowable
{
    protected Rigidbody rb;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Throw(Vector3 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode.VelocityChange);
    }
}