using System;
using UnityEngine;

namespace Core.Entity.Common
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class Movement : MonoBehaviour 
    {
        [SerializeField] private CapsuleCollider _collider;
        [SerializeField] private FloatingCapsuleData _colliderData;
        [SerializeField] [Range(0,1)] private float _stepHeightPercentage = 0.75f;
        [SerializeField] private float _stepReachForce = 25f;
        [SerializeField] private LayerMask _groundMask;
        
        public Rigidbody MovementRigidbody { get; private set; }
        private float _rideHeight;
        private RaycastHit[] _hit = new RaycastHit[1];
        public bool IsGrounded { get; private set; }
        public bool ForceStop;
        
        [Header("Speed")] 
        public float Speed;
        public float Acceleration = 10f;
        private float _acclerationMultipler;

        private void Reset()
        {
            _collider = GetComponent<CapsuleCollider>();       
        }

        protected virtual void Awake()
        {
            MovementRigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            CalculateColliderData(_collider, _colliderData);
            MovementRigidbody.useGravity = false;
            _rideHeight = _collider.center.y;
        }

        private void OnValidate()
        {
            CalculateColliderData(_collider, _colliderData);
        }

        public bool IsMoving()
        {
            var temp = MovementRigidbody.velocity;
            temp.y = 0f;
            return temp.magnitude > 0.01f;
        }
        
        private void CalculateColliderData(CapsuleCollider defaultColliderData, FloatingCapsuleData data)
        {
            defaultColliderData.height = data.Height * (1 - _stepHeightPercentage);
            defaultColliderData.radius = data.Radius;

            var differenceHeight = data.Height - defaultColliderData.height;

            var newColliderCenter = data.Center;
            newColliderCenter.y = data.Center.y + differenceHeight / 2;
            defaultColliderData.center = newColliderCenter;

            if (defaultColliderData.height / 2f < defaultColliderData.radius)
            {
                defaultColliderData.radius = defaultColliderData.height / 2;
            }
        }
        
        public void FloatingCapsule()
        {
             int hitAmount = Physics.RaycastNonAlloc(_collider.bounds.center, Vector3.down, _hit, 
                _rideHeight + 0.4f, _groundMask);
            
            IsGrounded = hitAmount > 0;
             
            if (IsGrounded)
            {
                var distanceToFloatingPoint = _rideHeight * transform.localScale.y - _hit[0].distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                var amountToLift = distanceToFloatingPoint * _stepReachForce - MovementRigidbody.velocity.y;
                
                MovementRigidbody.AddForce(Vector3.up * amountToLift, ForceMode.VelocityChange);
            }
            else
            {
                MovementRigidbody.AddForce(Physics.gravity, ForceMode.Force);
            }
        }

        public void Stop()
        {
            MovementRigidbody.velocity = Vector3.zero;
        }

        public void Move(Vector3 moveDirection)
        {
            var currentVelocity = MovementRigidbody.velocity;
            moveDirection = moveDirection.normalized;
            
            moveDirection.y = 0f;
            //currentVelocity.y = 0f;
            currentVelocity.z = 0f;
            currentVelocity.x = 0f;
            
            if(moveDirection.magnitude > 0.01f)
            {
                _acclerationMultipler = Mathf.Lerp(_acclerationMultipler, 1f, Acceleration * Time.fixedDeltaTime);
            }
            else
            {
                _acclerationMultipler = Mathf.Lerp(_acclerationMultipler, 0f, Acceleration * Time.fixedDeltaTime);
            }
         
            MovementRigidbody.velocity = currentVelocity + moveDirection * Speed * _acclerationMultipler;
            //_rb.AddForce(moveDirection * Speed * _acclerationMultipler - currentVelocity, ForceMode.VelocityChange);
        }

        #if UNITY_EDITOR
        [Header("Debug")] 
        public bool DrawRideHeight;
        public bool DrawRaycast;
        public Vector3 RideHeightOffset;
        
        private void OnDrawGizmos()
        {
            if (DrawRideHeight)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_collider.bounds.center + RideHeightOffset, transform.position + RideHeightOffset);
            }

            if (DrawRaycast && Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_collider.bounds.center, _collider.bounds.center + Vector3.down * (_rideHeight + 0.4f));
                Gizmos.DrawSphere(_hit[0].point, 0.2f);
            }
            
        }
        #endif
    }

    [Serializable]
    public class FloatingCapsuleData
    {
        public float Height;
        public Vector3 Center;
        public float Radius;
    }
}
