using UnityEngine;

namespace Core.UIManager
{
    public abstract class PanelBase : MonoBehaviour
    {
        public void Init(UIManager manager)
        {
            this.uiManager = manager;
        }

        protected UIManager uiManager; 
    }
}
