using System;
using TMPro;
using UnityEngine;

namespace Language
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMPLanguageSupport : MonoBehaviour
    {
        [SerializeField, LanguageSearchField()]
        private string _languageKey;
        public string LanguageKey => _languageKey;
        public TextMeshProUGUI TextMesh {get; private set;}
        
        private TMP_FontAsset defaultFont;

        protected virtual void Reset()
        {
            if (!TextMesh)
            {
                TextMesh = GetComponent<TextMeshProUGUI>();
            }
        }

        protected virtual void Awake()
        {
            if (!TextMesh)
            {
                TextMesh = GetComponent<TextMeshProUGUI>();
            }
            defaultFont = TextMesh.font;
            LanguageManager.OnLanguageChange += HandleLanguageChange;
        }

        protected virtual void OnEnable()
        {
            if (LanguageManager.Instance)
            {
                HandleLanguageChange(LanguageManager.Instance.CurLanguage);
            }
        }

        protected virtual void OnDestroy()
        {
            LanguageManager.OnLanguageChange -= HandleLanguageChange;
        }

        public virtual void SetKey(string key)
        {
            _languageKey = key;
            if(!TextMesh) return;
           
            string result = LanguageManager.Instance.FindContent(_languageKey,
                LanguageManager.Instance.CurLanguage);
            
            if (result == string.Empty) return;
            
            SetUpText(result);
        }
        public Func<string, string> OnLanguageChange;
        
        public string GetKey() => _languageKey;
        
        private void HandleLanguageChange(Language newLanguage)
        {
            if(TextMesh.text == string.Empty) return;
            
            var result = LanguageManager.Instance.FindContent(_languageKey, newLanguage);
            
            if (result == string.Empty) return;
            
            SetUpText(result);
        }

        private void SetUpText(string result)
        {
            var newResult = OnLanguageChange?.Invoke(result);

            if (string.IsNullOrEmpty(newResult))
            {
                TextMesh.text = result;
                return;
            }
            
            TextMesh.text = newResult;
        }
    }
}