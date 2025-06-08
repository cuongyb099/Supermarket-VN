using UnityEngine;

namespace Core.UIManager
{
    [RequireComponent(typeof(FadeUI))]
    public abstract class FadePanel : PanelToggle
    {
        protected const float _fadeDuration = 0.25f;
        protected FadeUI fadeUI;
        
        private void Awake()
        {
            fadeUI = GetComponent<FadeUI>();
            var canvasGroup = GetComponent<CanvasGroup>();
            
            if (canvasGroup.alpha > 0.9)
            {
                this.IsVisible = true;
                canvasGroup.blocksRaycasts = true;
            }
            else
            {
                this.IsVisible = false;
                canvasGroup.blocksRaycasts = false;
            }
            
            OnAwake();
        }

        protected virtual void OnAwake() { }

        public override void OnHide()
        {
            fadeUI.FadeOut(_fadeDuration);
            this.IsVisible = false;
        }

        public override void OnShow()
        {
            fadeUI.FadeIn(_fadeDuration);
            this.IsVisible = true;
        }
    }
}
