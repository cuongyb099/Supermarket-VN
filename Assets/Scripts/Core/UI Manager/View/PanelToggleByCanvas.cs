using UnityEngine;

namespace Core.UIManager
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class PanelToggleByCanvas : PanelToggle
    {
        protected CanvasGroup canvasGroup;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup.alpha > 0.9)
            {
                this.IsVisible = true;
            }
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            
        }

        public override void OnHide()
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            this.IsVisible = false;
        }

        public override void OnShow()
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            this.IsVisible = true;
        }
    }
}
