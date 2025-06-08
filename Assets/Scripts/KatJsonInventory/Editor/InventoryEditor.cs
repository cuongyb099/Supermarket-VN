using System;
using KatJsonInventory.Item;

namespace KatJsonInventory.Editor
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor;
    using UnityEngine;
    
    [CustomEditor(typeof(Inventory))]
    public class InventoryEditor : Editor
    {
        private List<IInstanceItem> items => (target as Inventory).InventoryEditor;
        private string _searchID = string.Empty;
        private int _pageCount = 10;
        private int _curPage;
        
        private void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search");
            _searchID = EditorGUILayout.TextField(_searchID, GUILayout.MaxWidth(400));
            EditorGUILayout.EndHorizontal();
            
            GUI.enabled = false;
            if (items == null)
            {
                return;
            }
            var value = items.ToList();

            if (_searchID != string.Empty)
            {
                value = value.FindAll(x => x.GetItemData().ID.ToLower().StartsWith(_searchID.ToLower()));
            }
            
            var maxPageCount = Mathf.CeilToInt(value.Count / _pageCount);
            if (_curPage > maxPageCount)
            {
                _curPage = maxPageCount;
            }
            for (int i = 0 + _curPage * _pageCount; i < _pageCount + _curPage * _pageCount; i++)
            {
                if(i >= value.Count) break;
                var item = value[i];
                string content = $"Item {i}: {item.GetItemData().ID}";
                
                if (item is IItemStack stack)
                {
                    content += $" | Quantities : {stack.Quantity}";
                }
                
                EditorGUILayout.TextField(content);
            }
            
            GUI.enabled = true;
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Previous"))
            {
                if (_curPage > 0)
                {
                    _curPage--;
                }
            };
            if (GUILayout.Button("Next"))
            {
                if (_curPage < maxPageCount)
                {
                    _curPage++;
                }
            };
            EditorGUILayout.EndHorizontal();
        }
    }
}