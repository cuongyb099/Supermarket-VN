/*
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Language.Editor
{
    [CustomEditor(typeof(TMPLanguageSupport))]
    public class TMPLanguageSupportEditor : UnityEditor.Editor
    {
        private TMPLanguageSupport _script;
        private List<string> _keys = new List<string>();
        private Action<string> _callback;
        private StringSearch _search;
        private void OnEnable()
        {
            _script = (TMPLanguageSupport)target;
            var json = File.ReadAllText("Assets/Language.json");
            var dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string,string>>>(json);
            _keys.Clear();
            foreach (var key in dict.Keys)
            {
                _keys.Add(key);
            }

            _callback = (value) =>
            {
                _script.LanguageKey = value;
            };
            
            _search = ScriptableObject.CreateInstance<StringSearch>();
            _search.Keys = _keys;
            _search.Callback = _callback;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Language Key", GUILayout.Width(100));

            if (GUILayout.Button(_script.LanguageKey, EditorStyles.popup))
            {
                
                
                SearchWindow.Open(new SearchWindowContext(GUIUtility
                    .GUIToScreenPoint(Event.current.mousePosition)), _search);
            }
            
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif
*/
