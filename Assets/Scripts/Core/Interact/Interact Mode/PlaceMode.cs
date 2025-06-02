using System.Collections.Generic;
using Core.Input;
using Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
    using Matrix4x4 = UnityEngine.Matrix4x4;
#endif
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Core.Interact.Interact_Mode
{
    [System.Serializable]
    public class PlaceMode : InteractModeBase
    {
        [SerializeField] protected LayerMask layer;
        [SerializeField] protected Color canPlaceColor = Color.green;
        [SerializeField] protected Color cannotPlaceColor = Color.red;
        [SerializeField] protected List<string> overlapTagsCheck;
        [SerializeField] protected float angleEachRotate = 15f;
        [SerializeField] protected float throwForce = 10f;
        
        protected InputAction PlacePerform;
        protected InputAction rotateAction;
        protected InputAction exitAction;
        
        protected PlaceHitbox hitbox;
        
        protected Vector3 overlapPosition;
        protected Vector3 overlapAngles;
        protected const float smoothSpeed = 15f;
        protected float currentRotateAngle;
        protected bool canPlace;
        
        public override void Init(Interactor interactor, InteractData interactData)
        {
            base.Init(interactor, interactData);
            var defaultActions = InputManager.Instance.PlayerInputMap.Default;
            PlacePerform = defaultActions.Interact;
            rotateAction = defaultActions.RotateItem;
            exitAction = defaultActions.Exit;
        }

        public override YieldInstruction OnUpdate()
        {
            data.CurrentIndicatable ??= data.CurrentTarget.GetComponent<IIndicatable>();
            
            float RayDistance = this.data.RayDistance;
            RaycastHit[] hits = this.data.RayHits;
            Transform cameraTransform = this.data.CamTransform;
            InteractObject CurrentTarger = this.data.CurrentTarget;
            
            Transform targetTransform = CurrentTarger.transform;
            float rayLenght = RayDistance * 1.5f;
            
            int hitCount = Physics.RaycastNonAlloc(cameraTransform.position, 
                cameraTransform.forward, hits, rayLenght);
                
            hitbox = data.CurrentPlaceObject.GetPlaceHitBox();

            canPlace = false;
            RaycastHit hit = default;
            
            if (hitCount > 0)
            {
                hit = hits[0];
                overlapPosition = hit.point;
                overlapAngles = hitbox.transform.eulerAngles;
                overlapAngles.x = 0;
                overlapPosition.y += hitbox.transform.localPosition.y;
                int overlapCount = Physics.OverlapBoxNonAlloc(overlapPosition, hitbox.Size / 2
                    , data.OverlapHits, Quaternion.Euler(overlapAngles), layer);
                
                for (int i = 0; i < overlapCount; i++)
                {
                    canPlace = CheckingPlace(data.OverlapHits[i]); 
                    
                    if(canPlace) continue;
                        
                    break;
                }
            }

            UpdateRotationAngleOffset();

            if (exitAction.WasPerformedThisFrame())
            {
                data.CurrentIndicatable.DisableIndicator();
                interactor.AttachItemToHand((ObjectAttackToHand)data.CurrentTarget);
                return null;
            }
            
            if (canPlace && PlacePerform.WasPerformedThisFrame())
            {
                ResetCurrentObject();
                return null;
            }
            
            data.CurrentIndicatable.EnableIndicator(canPlace ? canPlaceColor : cannotPlaceColor);

            Quaternion targetRot;
            
            if (hitCount == 0 || hitCount > 0 && !canPlace)
            {
                Transform currentHand = this.data.CurrentHandTransform;
                Vector3 targetPos = currentHand.position;
                targetRot = currentHand.rotation;
            
                targetTransform.position = Vector3.Lerp(targetTransform.position, targetPos, smoothSpeed * Time.deltaTime);
                targetTransform.rotation = Quaternion.Lerp(targetTransform.rotation, targetRot, smoothSpeed * Time.deltaTime);
                targetTransform.SetParent(currentHand);
                return null;
            }

            Vector3 lookDir = Vector3.ProjectOnPlane(-cameraTransform.forward, Vector3.up);
            targetRot = Quaternion.LookRotation(lookDir, Vector3.up);
            targetRot *= Quaternion.Euler(0f, currentRotateAngle, 0f);
            
            targetTransform.SetParent(null);
            targetTransform.position = Vector3.Lerp(targetTransform.position, hit.point, smoothSpeed * Time.deltaTime);
            targetTransform.rotation = Quaternion.Lerp(targetTransform.rotation, targetRot, smoothSpeed * Time.deltaTime);
            return null;
        }

        private void UpdateRotationAngleOffset()
        {
            var rotateSpeed = rotateAction.ReadValue<Vector2>().y;
            
            if (rotateSpeed > 0 && rotateSpeed != 0)
            {
                currentRotateAngle += angleEachRotate;
            }
            else if(rotateSpeed != 0)
            {
                currentRotateAngle -= angleEachRotate;   
            }

            currentRotateAngle = currentRotateAngle.NormalizeAngle();
        }

        private bool CheckingPlace(Collider collider)
        {
            foreach (var tag in overlapTagsCheck)
            {
                if(!collider.CompareTag(tag)) continue;
                return true;
            }
            return false;
        }

        private void ResetCurrentObject()
        {
            data.CurrentTarget.ResetToIdle();
            data.CurrentIndicatable.DisableIndicator();
            data.CurrentInteractMode = InteractMode.Default;
            data.ResetCurrentTarget();
        }
        
#if UNITY_EDITOR
        public override void OnDrawGizmos()
        {
            var hitboxTransform = hitbox?.transform;
            if (!hitboxTransform) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.matrix = Matrix4x4.TRS(overlapPosition,  Quaternion.Euler(overlapAngles), Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, hitbox.Size);
        }
#endif
    }
}
