using System;
using System.Collections.Generic;
using KatLib.Data_Serialize;
using Newtonsoft.Json;
using KatLib.Singleton;
using UnityEngine;
using TextAsset = UnityEngine.TextAsset;

namespace Language
{
    public class LanguageManager : SingletonPersistent<LanguageManager>
    {
        public const string Language = "Language";
        private Dictionary<string , Dictionary<string, string>> _languageDictionary = new ();
        [SerializeField]
        private Language _currentLanguage;
        public Language CurLanguage => _currentLanguage;
        public static Action<Language> OnLanguageChange;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            GetInstance();
        }
        
        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        private void Start()
        {
            OnLanguageChange?.Invoke(_currentLanguage);
        }

        private void Load()
        {
            var textAsset = Resources.Load<TextAsset>(Language);
            _languageDictionary = JsonConvert.DeserializeObject
                <Dictionary<string, Dictionary<string, string>>>(textAsset.text);
            _currentLanguage = (Language)PlayerPrefs.GetInt(Language, 0);
        }
        
        public void ChangeLanguage(Language language)
        {
            OnLanguageChange?.Invoke(language);
            _currentLanguage = language;
            DataSerialize.SetData(Language, (int)language);
            //PlayerPrefs.SetInt(Language, (int)language);
        }
        
        public string FindContent(string key, Language language)
        {
            if (!_languageDictionary.TryGetValue(key, out var result))
                return string.Empty;
                
            switch (language)
            {
                case global::Language.Language.EN:
                    return result[LanguageConstant.English];
                case global::Language.Language.VN:
                    return result[LanguageConstant.Vietnamese];
                default:
                    return string.Empty;
            }
        }
    }
}