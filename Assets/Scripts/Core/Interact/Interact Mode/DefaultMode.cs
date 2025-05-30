using UnityEngine;

namespace Core.Interact.Interact_Mode
{
    [System.Serializable]
    public class DefaultMode : InteractModeBase
    {
        [SerializeField] protected LayerMask layer;
        [SerializeField] protected OutlineConfigSO outlineConfig;
        protected WaitForSeconds wait;
        
        public override void Init(Interactor interactor, InteractData interactData)
        {
            base.Init(interactor, interactData);
            wait = new WaitForSeconds(1f / this.data.CheckPerSecond);
        }

        public override YieldInstruction OnUpdate()
        {
            Transform cameraTransform = this.data.CamTransform;
            InteractObject currentTarget = this.data.CurrentTarget;
            RaycastHit[] hits = this.data.RayHits;
            float rayDistance = this.data.RayDistance;
            
            if (currentTarget is { CanInteract: false })
            {
                currentTarget = null;
            }
                
            int hitCount = Physics.RaycastNonAlloc(cameraTransform.position, 
                cameraTransform.forward, hits, rayDistance, layer);

            for (int i = 0; i < hitCount; i++)
            {
                var hit = hits[i];
                
                if(!hit.transform.TryGetComponent(out InteractObject interactable) 
                   || !interactable.CanInteract) continue;
                
                currentTarget?.ObjectOutline.DisableOutline();
                data.CurrentTarget = interactable;
                interactable.ObjectOutline?.SetConfig(outlineConfig);
                interactable.ObjectOutline?.EnableOutline();
                return wait;
            }

            currentTarget?.ObjectOutline?.DisableOutline();
            data.CurrentTarget = null;

            return wait;
        }

        public override void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Transform cameraTransform = this.data.CamTransform;
            Gizmos.DrawLine(cameraTransform.position, cameraTransform.position + cameraTransform.forward * data.RayDistance);
        }
    }
}

