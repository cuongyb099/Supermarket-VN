using Core.Utilities;
using KatLib.State_Machine;
using UnityEngine;

namespace Core.Interact.Interact_Mode
{
    [System.Serializable]
    public class PlaceState : InteractState 
    {
        protected PlaceHitbox hitbox;
        protected Vector3 overlapPosition;
        protected Vector3 overlapAngles;
        protected const float smoothSpeed = 15f;
        protected float currentRotateAngle;
        protected bool canPlace;
        protected IIndicatable currentIndicatable;
        protected IPlacable currentPlace => (IPlacable)data.CurrentTargetFristSlot;
        protected IRenderOnTopHandle CurrentRenderOnTopHandle => (IRenderOnTopHandle)data.CurrentTargetFristSlot;
        
        public PlaceState(InteractData data, Interactor interactor, StateMachine<InteractMode, InteractState> stateMachine) 
            : base(data, interactor, stateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            data.CurrentTargetFristSlot.TryGetComponent(out currentIndicatable);
        }

        public override YieldInstruction OnUpdate()
        {
            
            float RayDistance = this.data.RayDistance;
            RaycastHit[] hits = this.data.RayHits;
            Transform cameraTransform = this.data.CamTransform;
            InteractObject CurrentTarger = this.data.CurrentTargetFristSlot;
            
            Transform targetTransform = CurrentTarger.transform;
            float rayLenght = RayDistance * 1.5f;
            
            int hitCount = Physics.RaycastNonAlloc(cameraTransform.position, 
                cameraTransform.forward, hits, rayLenght);
                
            hitbox = currentPlace.GetPlaceHitBox();

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
                    , data.OverlapHits, Quaternion.Euler(overlapAngles), data.RaycastLayer);
                
                for (int i = 0; i < overlapCount; i++)
                {
                    canPlace = CheckingPlace(data.OverlapHits[i]); 
                    
                    if(canPlace) continue;
                        
                    break;
                }
            }

            UpdateRotationAngleOffset();

            if (data.ExitAction.WasPerformedThisFrame())
            {
                currentIndicatable.DisableIndicator();
                interactor.AttachItemToHand((ObjectAttackToHand)data.CurrentTargetFristSlot);
                return null;
            }
            
            if (canPlace && data.PlacePerform.WasPerformedThisFrame())
            {
                ResetCurrentObject();
                return null;
            }
            
            currentIndicatable.EnableIndicator(canPlace ? data.CanPlaceColor : data.CannotPlaceColor);

            Quaternion targetRot;
            
            if (hitCount == 0 || hitCount > 0 && !canPlace)
            {
                Transform currentHand = this.data.CurrentHandTransform;
                Vector3 targetPos = currentHand.position;
                targetRot = currentHand.rotation;
            
                targetTransform.position = Vector3.Lerp(targetTransform.position, targetPos, smoothSpeed * Time.deltaTime);
                targetTransform.rotation = Quaternion.Lerp(targetTransform.rotation, targetRot, smoothSpeed * Time.deltaTime);
                targetTransform.SetParent(currentHand);
                CurrentRenderOnTopHandle.SetOnTop();
                return null;
            }

            CurrentRenderOnTopHandle?.ToDefaultRender();
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
            var rotateSpeed = data.RotateAction.ReadValue<Vector2>().y;
            
            if (rotateSpeed > 0 && rotateSpeed != 0)
            {
                currentRotateAngle += data.AngleEachRotate;
            }
            else if(rotateSpeed != 0)
            {
                currentRotateAngle -= data.AngleEachRotate;   
            }

            currentRotateAngle = currentRotateAngle.NormalizeAngle();
        }

        private bool CheckingPlace(Collider collider)
        {
            foreach (var tag in data.OverlapTagsCheck)
            {
                if(!collider.CompareTag(tag)) continue;
                return true;
            }
            return false;
        }

        private void ResetCurrentObject()
        {
            data.CurrentTargetFristSlot.ResetToIdle();
            currentIndicatable.DisableIndicator();
            stateMachine.ChangeState(InteractMode.Idle);
            data.CurrentHandTransform = null;
            data.CurrentTargetFristSlot = null;
            currentIndicatable = null;
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

