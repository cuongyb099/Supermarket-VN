namespace Core.UIManager
{
    public abstract class PanelToggle : PanelBase, IHidable, IShowable
    {
        public override void Hide()
        {
            IsVisible = false;
            uiManager.RemoveFromHistory(this);
            OnHide();
        }

        public override void Show()
        {
            IsVisible = true;
            uiManager.AddToHitory(this);
            OnShow();        
        }
        
        public abstract void OnHide();
        public abstract void OnShow();
    }
}
