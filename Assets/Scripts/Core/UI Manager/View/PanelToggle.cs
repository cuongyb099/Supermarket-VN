namespace Core.UIManager
{
    public abstract class PanelToggle : PanelBase, IHidable, IShowable
    {
        public bool IsVisible { get; protected set; }

        public void Hide()
        {
            IsVisible = false;
            uiManager.RemoveFromHistory(this);
            OnHide();
        }

        public void Show()
        {
            IsVisible = true;
            uiManager.AddToHitory(this);
            OnShow();        
        }
        
        public abstract void OnHide();
        public abstract void OnShow();
    }
}
