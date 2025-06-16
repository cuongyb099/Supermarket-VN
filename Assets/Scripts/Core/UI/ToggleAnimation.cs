using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ToggleAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform handle;
        [SerializeField] private Color toggleOff, toggleOn;
        [SerializeField] private Image image;
        [SerializeField] private Toggle toggle;

        private void Start()
        {
            toggle.onValueChanged.AddListener(OnValueChanged);
            OnValueChanged(toggle.isOn);
        }

        private void OnValueChanged(bool value)
        {
            if (value)
            {   
                image.DOColor(toggleOn, 0.3f);
                handle.DOLocalMoveX(25, 0.3f);
            }
            else
            { 
                image.DOColor(toggleOff, 0.3f);
                handle.DOLocalMoveX(-25, 0.3f);
            }
        }
    }
}