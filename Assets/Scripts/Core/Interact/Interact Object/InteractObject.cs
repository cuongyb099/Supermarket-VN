using UnityEngine;

namespace Core.Interact
{
    public abstract class InteractObject : MonoBehaviour
    {
        public OutlineBase ObjectOutline { get; protected set; }
        [field: SerializeField] public bool CanInteract { get; protected set; }
        private static bool _isLoadone;

        protected virtual void Awake()
        {
	        ObjectOutline = GetComponent<OutlineBase>();
        }

        public void Interact(Transform source)
        {
	        if(!CanInteract) return;
            
	        OnInteract(source);
        }

        protected abstract void OnInteract(Transform source);
        
        public void RefreshShader(Material material)
        {
	        var tempShader = material.shader;
	        material.shader = null;
	        material.shader = tempShader;
        }

        public virtual void ResetToIdle()
        {
	        SetActiveCollision(true);
        }
        
        public virtual void SetActiveCollision(bool value)
        {
            
        }
    }
}
