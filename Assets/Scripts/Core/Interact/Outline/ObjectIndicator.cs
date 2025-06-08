using System;
using Core.Constant;
using UnityEngine;

namespace Core.Interact
{
    public class ObjectIndicator : MonoBehaviour, IIndicatable
    {
        [SerializeField] protected Renderer[] renderers;
        public Action<Color> OnEnableIndicator { get; set; }
        public Action OnDisableIndicator { get; set; }
        
        private void Reset()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }
        
        public void EnableIndicator(Color color)
        {
            foreach (var meshRenderer in renderers)
            {
                foreach (var material in meshRenderer.materials)
                {
                    material.SetFloat(MaterialConstant.SurfaceID, 1);
                    material.SetFloat(MaterialConstant.ZWrite, 1);
                    material.SetFloat(MaterialConstant.SourceBlend, 5); // SrcAlpha
                    material.SetFloat(MaterialConstant.DestBlend, 10); // OneMinusSrcAlpha
                    material.SetFloat(MaterialConstant.Cutout, 0); // Cutout
                    material.DisableKeyword(MaterialConstant.CutoutKeyword);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.color = color;
                }

                meshRenderer.staticShadowCaster = false;
            }
            OnEnableIndicator?.Invoke(color);
        }

        public void DisableIndicator()
        {
            foreach (var meshRenderer in renderers)
            {
                foreach (var material in meshRenderer.materials)
                {
                    material.SetFloat(MaterialConstant.SurfaceID, 0);
                    material.SetFloat(MaterialConstant.ZWrite, 1);
                    material.SetFloat(MaterialConstant.SourceBlend, 1); 
                    material.SetFloat(MaterialConstant.DestBlend, 0); 
                    material.SetFloat(MaterialConstant.Cutout, 0); 
                    material.DisableKeyword(MaterialConstant.CutoutKeyword);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                    material.color = Color.white;
                }
                
                meshRenderer.staticShadowCaster = true;
            }
            OnDisableIndicator?.Invoke();
        }
    }
}
