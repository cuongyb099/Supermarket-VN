using System;
using UnityEngine;

namespace Language
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class LanguageSearchField : PropertyAttribute
    {
        public string Path = "Assets/Resources/Language/Language.json";
        public LanguageSearchField()
        {
            
        }
    }
}