using Core.Constant;
using UnityEngine;

namespace Core.Interact
{
    public abstract class InteractObject : MonoBehaviour
    {
        [SerializeField] protected Renderer[] renderers;
        [SerializeField] protected OutlineConfigSO outlineConfig;
        
        [field: SerializeField] public bool CanInteract { get; protected set; }
        
        public static readonly Color OutlineColor = Color.green;
        private static bool _isLoadone;
        
        protected virtual void Reset()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        public void Interact(Transform source)
        {
	        if(!CanInteract) return;
            
	        OnInteract(source);
        }

        protected abstract void OnInteract(Transform source);
        
        public void ShowOutline()
        {
            foreach (var render in renderers)
            {
	            foreach (var material in render.materials)
	            {
		            material.SetInt(MaterialConstant.EnableOutline, 1);
		            material.SetShaderPassEnabled(MaterialConstant.SRPDefaultUnlit, true);
		            material.SetColor(MaterialConstant.OutlineColor, OutlineColor);
		            material.DisableKeyword(MaterialConstant.OutlinePass);
		            material.SetFloat(MaterialConstant.OutlineWidth, outlineConfig.OutlineWidth);
		            RefreshShader(material);
	            }
            }
        }

        public void HideOutline()
        {
            foreach (var render in renderers)
            {
	            foreach (var material in render.materials)
	            {
		            material.SetInt(MaterialConstant.EnableOutline, 0);
		            material.SetShaderPassEnabled(MaterialConstant.SRPDefaultUnlit, false);
		            material.EnableKeyword(MaterialConstant.OutlinePass);
		            RefreshShader(material);
	            }
            }
        }
        
        public void RefreshShader(Material material)
        {
	        var tempShader = material.shader;
	        material.shader = null;
	        material.shader = tempShader;
        }

        public virtual void ResetToIdle()
        {
	        
        }
    }
}
