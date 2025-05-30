using System.Collections.Generic;
using Core.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interact.Interact_Mode
{
    [System.Serializable]
    public class PlaceMode : InteractModeBase
    {
        [SerializeField] protected LayerMask layer;
        protected IIndicatable CurrentIndicatable;
        [SerializeField] protected Color canPlaceColor = Color.green;
        [SerializeField] protected Color cannotPlaceColor = Color.red;
        [SerializeField] protected List<string> overlapTagsCheck;
        protected bool canPlace;
        protected IIndicatable Indicatable;
        protected InputAction PlacePerform;

        public override void Init(Interactor interactor, InteractData interactData)
        {
            base.Init(interactor, interactData);
            PlacePerform = InputManager.Instance.PlayerInputMap.Default.Interact;
        }

        public override YieldInstruction OnUpdate()
        {
            Indicatable ??= data.CurrentTarget.GetComponent<IIndicatable>();
            
            float RayDistance = this.data.RayDistance;
            RaycastHit[] hits = this.data.RayHits;
            Transform cameraTransform = this.data.CamTransform;
            InteractObject CurrentTarger = this.data.CurrentTarget;
            
            Transform targetTransform = CurrentTarger.transform;
            float rayLenght = RayDistance * 1.5f;
            
            int hitCount = Physics.RaycastNonAlloc(cameraTransform.position, 
                cameraTransform.forward, hits, rayLenght);
                
            Vector3 lookDir = Vector3.ProjectOnPlane(-cameraTransform.forward, Vector3.up);;
            targetTransform.rotation = Quaternion.LookRotation(lookDir, Vector3.up);

            PlaceHitbox hitbox = data.CurrentPlaceObject.GetPlaceHitBox();
            int overlapCount = Physics.OverlapBoxNonAlloc(hitbox.transform.position, hitbox.Size / 2
                , data.OverlapHits, hitbox.transform.rotation, layer);

            canPlace = false;
            
            for (int i = 0; i < overlapCount; i++)
            {
                canPlace = CheckingPlace(data.OverlapHits[i]); 
                
                if(canPlace) continue;
                    
                break;
            }
            
            Indicatable.EnableIndicator(canPlace ? canPlaceColor : cannotPlaceColor);

            if (canPlace && PlacePerform.WasPerformedThisFrame())
            {
                PlaceObject();
                return null;
            }
            
            if (hitCount == 0)
            {
                var pos = cameraTransform.position + cameraTransform.forward * rayLenght;
                targetTransform.position = pos;
                return null;
            }
                
            var hit = hits[0];
            targetTransform.position = hit.point;
            return null;
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

        private void PlaceObject()
        {
            Indicatable.DisableIndicator();
            data.CurrentTarget.SetActiveCollision(true);
            data.CurrentTarget.ResetToIdle();
            data.CurrentTarget = null;
            data.CurrentInteractMode = InteractMode.Default;
        }
    }
}
