using System.Collections;
using Core.Input;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interact
{
    public class Interactor : MonoBehaviour
    {
        public float RayDistance = 10;
        public bool Enable;
        [SerializeField] protected int checkPerSecond = 8;
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected Transform handTransform;
        [SerializeField] protected float itemReturnHandTime = 0.2f;
        
        protected RaycastHit[] hits = new RaycastHit[10];
        protected Transform cameraTransform => CameraManager.Instance.MainCamera.transform;
        protected InteractObject currentInteractTarget;
        
        private void Awake()
        {
            InputManager.Instance.InteractAction.performed += HandleInteractPerformed;
        }

        protected virtual void Start()
        {
            StartChecking();
        }

        private void OnDestroy()
        {
            if(!InputManager.IsExist) return;
            
            InputManager.Instance.InteractAction.performed += HandleInteractPerformed;
        }

        private void HandleInteractPerformed(InputAction.CallbackContext context)
        {
            if(!currentInteractTarget || !currentInteractTarget.CanInteract) return;

            if (Enable)
            {
                currentInteractTarget.Interact(this.transform);
                return;
            }
            
            //Test
            currentInteractTarget.ResetToIdle();
            currentInteractTarget.transform.SetParent(null);
            var rb = currentInteractTarget.GetComponent<Rigidbody>();
            rb.AddForce(cameraTransform.forward * 10, ForceMode.Impulse);
            Enable = true;
        }

        protected virtual IEnumerator CheckingInteract()
        {
            var wait = new WaitForSeconds(1f / checkPerSecond);
            
            while (true)
            {
                if (!Enable)
                {
                    yield return wait;
                    continue;
                }
                
                if (currentInteractTarget is { CanInteract: false })
                {
                    currentInteractTarget = null;
                }
                
                int hitCount = Physics.RaycastNonAlloc(cameraTransform.position, 
                    cameraTransform.forward, hits, RayDistance, layerMask);

                if (hitCount == 0)
                {
                    currentInteractTarget?.HideOutline();
                    currentInteractTarget = null;
                    yield return wait;
                    continue;   
                }

                for (int i = 0; i < hitCount; i++)
                {
                    var hit = hits[i];
                    
                    if(!hit.transform.TryGetComponent(out InteractObject interactable) 
                       || currentInteractTarget == interactable 
                       || !interactable.CanInteract) continue;
                    
                    currentInteractTarget = interactable;
                    interactable.ShowOutline();
                    break;
                }
                
                yield return wait;
            }            
        }

        public virtual void StartChecking()
        {
            StartCoroutine(CheckingInteract());       
        }
        
        public virtual  void StopChecking()
        {
            StopCoroutine(CheckingInteract());
        }

        public void AttachItemToHand(InteractObject item)
        {
            Enable = false;
            currentInteractTarget = item;
            Transform interactTransform = item.transform;
            interactTransform.SetParent(handTransform);
            interactTransform.DOLocalMove(Vector3.zero, itemReturnHandTime);
            interactTransform.DOLocalRotate(Vector3.zero, itemReturnHandTime);
        }
        
#if UNITY_EDITOR
        public bool Debug;
        protected virtual void OnDrawGizmos()
        {
            if(!cameraTransform || !Debug) return;
            
            Gizmos.color = Color.green;
            Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * RayDistance);
        }
#endif
    }
}
