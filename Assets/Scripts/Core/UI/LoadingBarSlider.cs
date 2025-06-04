using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Core.UI
{
    [RequireComponent(typeof(Slider))]
    public class LoadingBarSlider : LoadingBarBase
    {
        protected Slider slider;
        [SerializeField] protected TextMeshProUGUI description_TMP;
        [SerializeField] protected TextMeshProUGUI progress_TMP;
        public const string Progresss = "Loading";
        protected StringBuilder stringBuilder;
        
        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            stringBuilder = GenericPool<StringBuilder>.Get();
            description_TMP.text = string.Empty;
        }

        private void OnDestroy()
        {
            GenericPool<StringBuilder>.Release(stringBuilder);
        }

        public override void SetProgress(float progress)
        {
            slider.value = progress;
            stringBuilder.Clear();
            stringBuilder.Append(Progresss).Append(' ').Append(Mathf.FloorToInt(progress * 100)).Append('%');
            progress_TMP.text = stringBuilder.ToString();
        }

        public override void SetDescription(string description)
        {
            description_TMP.text = description;
        }

        public override void ResetProgress()
        {
            slider.value = 0f;
            progress_TMP.text = string.Empty;
        }

        public override float CurrentProgress => slider.value;
    }
}
