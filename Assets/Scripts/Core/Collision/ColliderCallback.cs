using UnityEngine;
using UnityEngine.Events;

namespace Core.Collision
{
    public class ColliderCallback : MonoBehaviour
    {
        public UnityEvent<Collider> TriggerEnterCallback;
        public UnityEvent<Collider> TriggeExitCallback;
        public UnityEvent<UnityEngine.Collision> CollisionEnterCallback;
        public UnityEvent<UnityEngine.Collision> CollisionExitCallback;
        
        public Collider ObjectCollider { get; protected set; }

        protected virtual void Awake()
        {
            ObjectCollider = GetComponent<Collider>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            TriggerEnterCallback?.Invoke(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            TriggeExitCallback?.Invoke(other);
        }

        private void OnCollisionEnter(UnityEngine.Collision other)
        {
            CollisionEnterCallback?.Invoke(other);
        }

        private void OnCollisionExit(UnityEngine.Collision other)
        {
            CollisionExitCallback?.Invoke(other);
        }
    }
}
