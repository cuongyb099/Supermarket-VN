using System.Collections.Generic;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Core.UIManager
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIManager : MonoBehaviour
    {
        private Dictionary<string, PanelBase> _panels = new();
        private CanvasGroup _canvasGroup;
        [SerializeField] private List<PanelBase> _panelsHistory = new();
        
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _panelsHistory.Clear();
            foreach (var panel in GetComponentsInChildren<PanelBase>())
            {
                panel.Init(this);

                _panels.Add(panel.name, panel);
             
                if (!panel.IsVisible) continue;
                
                _panelsHistory.Add(panel);
            }
        }

        public void SetUIActive(bool value)
        {
            if (value)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
                return;
            }
            
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
        }
        
        public void AddToHitory(PanelToggle panel)
        {
            if(_panelsHistory.Contains(panel)) return;
            
            _panelsHistory.Add(panel);
        }

        public void RemoveFromHistory(PanelToggle panel)
        {
            _panelsHistory.Remove(panel);
        }
        
        public async UniTask<PanelBase> CreatePanelAsync(string panelName, Transform parent = null, Action<PanelBase> onComplete = null)
        {
            if (_panels.TryGetValue(panelName, out PanelBase panel)) return panel;

            var go = await AddressablesManager.Instance.InstantiateAsync(panelName, parent? parent: transform, false);
            go.name = panelName;
            
            if (!go.TryGetComponent(out PanelBase panelBase))
            {
                return null;
            }
            
            panelBase.Init(this);
            
            _panels.Add(panelName, panelBase);
            onComplete?.Invoke(panelBase);
            return panelBase;
        }

        public void RemovePanel(string panelName)
        {
            if(!_panels.Remove(panelName, out var panel)) return;
            
            _panelsHistory.Remove(panel);
            AddressablesManager.Instance.ReleaseInstance(panelName);
        }

        public void RemoveAllPanel()
        {
            foreach (var panel in _panels)
            {
                AddressablesManager.Instance.ReleaseInstance(panel.Key);
            }
            
            _panelsHistory.Clear();
        }
        
        public T GetPanel<T>(string panelName) where T : PanelBase
        {
            return (T)_panels.GetValueOrDefault(panelName);
        }

        public T GetFirstPanelOfType<T>() where T : PanelBase
        {
            foreach (var panel in _panels.Values)
            {
                if (panel is T tPanel) return tPanel;
            }

            return null;
        }
        
        public void ShowPanel(string panelName)
        {
            if (!_panels.TryGetValue(panelName, out var panel)) return;
         
            panel.Show();
        }

        public void HideCurrentPanel()
        {
            if(_panelsHistory.Count == 0) return;
            
            _panelsHistory[^1].Hide();
        }
    }
}
