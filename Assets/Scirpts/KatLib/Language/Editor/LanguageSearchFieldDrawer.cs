using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Language.Editor
{
    [CustomPropertyDrawer(typeof(LanguageSearchField))]
    public class LanguageSearchFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String) return;
            
            var field = (LanguageSearchField)attribute;

            Rect labelPos = new Rect(position.x, position.y, 200, 16f);
            Rect textField = new Rect(position.x + labelPos.width, position.y, position.width - labelPos.width, 16f);
            Rect buttonPos = new Rect(position.x, position.y + 18f, position.width, 17f);
            EditorGUI.LabelField(labelPos, label);
            if (!property.serializedObject.isEditingMultipleObjects)
            {
                property.stringValue = EditorGUI.TextField(textField, property.stringValue);
                property.serializedObject.ApplyModifiedProperties();
            }
            else
            {
                EditorGUI.LabelField(textField, property.stringValue);
            }
            if (GUI.Button(buttonPos, "Search"))
            {
                StringSearch search = ScriptableObject.CreateInstance<StringSearch>();
                var json = File.ReadAllText(field.Path);
                var dict = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(json);
                search.Keys = new List<string>();
                search.Callback = (value) =>
                {
                    property.stringValue = value;
                    property.serializedObject.ApplyModifiedProperties();
                };
                foreach (var key in dict.Keys)
                {
                    search.Keys.Add(key);
                }
                SearchWindow.Open(new SearchWindowContext(GUIUtility
                    .GUIToScreenPoint(Event.current.mousePosition)), search);   
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 34f;
        }
    }
}