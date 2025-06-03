using UnityEngine;

namespace Core.Interact
{
    public abstract class InteractObject : MonoBehaviour
    {
        [field: SerializeField] public bool CanInteract { get; protected set; }
	    protected OutlineBase outline;

        protected virtual void Awake()
        {
	        outline = GetComponent<OutlineBase>();
        }

        public void Interact(Interactor source)
        {
	        if(!CanInteract) return;
            
	        OnInteract(source);
        }

        public virtual void Focus()
        {
	        outline.EnableOutline();
        }

        public virtual void UnFocus()
        {
	        outline.DisableOutline();
        }
        
        protected abstract void OnInteract(Interactor source);

        public virtual void ResetToIdle()
        {
	        SetActiveCollision(true);
        }
        
        public virtual void SetActiveCollision(bool value)
        {
            
        }
    }
}
