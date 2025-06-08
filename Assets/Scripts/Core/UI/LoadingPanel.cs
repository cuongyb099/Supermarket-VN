using Core.UIManager;
using UnityEngine;

namespace Core.UI
{
    public class LoadingPanel : PanelBase
    {
        [field: SerializeField] public LoadingBarBase LoadingBar { get; protected set; }
        
        public override void Show()
        {
         
        }

        public override void Hide()
        {
         
        }
    }
}
