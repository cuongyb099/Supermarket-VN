using System;
using UnityEngine;

namespace Core.Movement
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
        
        private Rigidbody _rb;
        private float _rideHeight;
        private RaycastHit _hit;
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
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            CalculateColliderData(_collider, _colliderData);
            _rb.useGravity = false;
            _rideHeight = _collider.center.y;
        }

        public bool IsMoving()
        {
            var temp = _rb.velocity;
            temp.y = 0f;
            return temp.magnitude > 0.01f;
        }
        
        private void OnValidate()
        {
            CalculateColliderData(_collider, _colliderData);
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
        
        private void ApplyGravity()
        {
            _rb.AddForce(Physics.gravity, ForceMode.Force);
        }

        private void FixedUpdate()
        {
            FloatingCapsule();
        }

        public void FloatingCapsule()
        {
            IsGrounded = Physics.Raycast(_collider.bounds.center, Vector3.down, out _hit, 
                _rideHeight + 0.4f, _groundMask);
            
            if (IsGrounded)
            {
                var distanceToFloatingPoint = _rideHeight * transform.localScale.y - _hit.distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                var amountToLift = distanceToFloatingPoint * _stepReachForce - _rb.velocity.y;
                
                _rb.AddForce(Vector3.up * amountToLift, ForceMode.VelocityChange);
            }
            else
            {
                ApplyGravity();
            }
        }

        public void Stop()
        {
            _rb.velocity = Vector3.zero;
        }

        public void Move(Vector3 moveDirection)
        {
            var currentVelocity = _rb.velocity;
            moveDirection = moveDirection.normalized;
            
            moveDirection.y = 0f;
            currentVelocity.y = 0f;
            
            if(moveDirection.magnitude > 0.01f)
            {
                _acclerationMultipler = Mathf.Lerp(_acclerationMultipler, 1f, Acceleration * Time.fixedDeltaTime);
            }
            else
            {
                _acclerationMultipler = Mathf.Lerp(_acclerationMultipler, 0f, Acceleration * Time.fixedDeltaTime);
            }
          
            _rb.AddForce(moveDirection * Speed * _acclerationMultipler - currentVelocity, ForceMode.VelocityChange);
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
                _collider = GetComponent<CapsuleCollider>();
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_collider.bounds.center + RideHeightOffset, transform.position + RideHeightOffset);
            }

            if (DrawRaycast && Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_collider.bounds.center, _collider.bounds.center + Vector3.down * (_rideHeight + 0.4f));
                Gizmos.DrawSphere(_hit.point, 0.2f);
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
