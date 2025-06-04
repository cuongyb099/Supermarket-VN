using System.Collections.Generic;
using System;
using KatLib.Logger;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Core.UIManager
{
    [DefaultExecutionOrder(-100)]
    public class UIManager : MonoBehaviour
    {
        private Dictionary<string, PanelToggle> _panelDictionary = new();
        [SerializeField] private List<PanelToggle> _panelsHistory = new();
        
        private void Awake()
        {
            _panelsHistory.Clear();
            foreach (var panel in GetComponentsInChildren<PanelBase>())
            {
                panel.Init(this);
                
                if (panel is not PanelToggle panelToggle) continue;
                
                _panelDictionary.Add(panel.name, panelToggle);
             
                if (!panelToggle.IsVisible) continue;
                
                _panelsHistory.Add(panelToggle);
            }
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
            if (_panelDictionary.TryGetValue(panelName, out PanelToggle panel)) return panel;

            var go = await AddressablesManager.Instance.InstantiateAsync(panelName, parent? parent: transform, false);
            go.name = panelName;
            
            if (!go.TryGetComponent(out PanelBase panelBase))
            {
                return null;
            }
            panelBase.Init(this);
            if (panelBase is not PanelToggle panelToggle)
            {
                onComplete?.Invoke(panelBase);
                return panelBase;
            }
            
            _panelDictionary.Add(panelName, panelToggle);
            onComplete?.Invoke(panelToggle);
            return panelToggle;
        }

        public void RemovePanel(string panelName)
        {
            if(!_panelDictionary.Remove(panelName, out var panel)) return;
            _panelsHistory.Remove(panel);
            Destroy(panel.gameObject);
            AddressablesManager.Instance.ReleaseInstance(panelName);
        }

        public T GetPanel<T>(string panelName) where T : PanelToggleByCanvas
        {
            return (T)_panelDictionary.GetValueOrDefault(panelName);
        }

        public T GetFirstPanelOfType<T>() where T : PanelToggleByCanvas
        {
            foreach (var panel in _panelDictionary.Values)
            {
                if (panel is T tPanel) return tPanel;
            }

            return null;
        }
        
        public void ShowPanel(string panelName)
        {
            if (!_panelDictionary.TryGetValue(panelName, out var panel) || _panelsHistory.Contains(panel)) return;
            
            panel.Show();
        }

        public void HideCurrentPanel()
        {
            if(_panelsHistory.Count == 0) return;
            
            _panelsHistory[^1].Hide();
        }
    }
}
