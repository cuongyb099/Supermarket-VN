using UnityEngine;

namespace Core.UIManager
{
    public abstract class PanelBase : MonoBehaviour
    {
        public bool IsVisible { get; protected set; }
        
        public void Init(UIManager manager)
        {
            this.uiManager = manager;
        }

        protected UIManager uiManager; 
        
        public abstract void Show();
        public abstract void Hide();
    }
}
