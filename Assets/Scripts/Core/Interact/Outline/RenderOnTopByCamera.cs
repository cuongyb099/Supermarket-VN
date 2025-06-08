using Core.Constant;
using UnityEngine;

namespace Core.Interact
{
    public class RenderOnTopByCamera : RenderOnTop
    {
        protected Renderer[] renderers;
        protected LayerMask defaultLayer;
        
        private void Awake()
        {
            renderers = GetComponentsInChildren<Renderer>();
            defaultLayer = gameObject.layer;
        }

        public override void SetOnTop()
        {
            foreach (var meshRender in renderers)
            {
                meshRender.gameObject.layer = LayerConstant.Overlay;
            }
        }

        public override void ReturnDefault()
        {
            foreach (var meshRender in renderers)
            {
                meshRender.gameObject.layer = defaultLayer;
            }
        }
    }
}
