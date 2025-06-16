using UnityEngine;
using Language;
using UnityEngine.UI;

namespace Core.UI
{
    public class ButtonLanguage : MonoBehaviour
    { 
        [SerializeField] private Language.Language language;
        [SerializeField] private RectTransform focus;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ChangeLanguage);
        }

        public void ChangeLanguage()
        { 
            LanguageManager.Instance.ChangeLanguage(language);
            focus.SetParent(transform);
            focus.position = transform.position;
        }
    }
}
