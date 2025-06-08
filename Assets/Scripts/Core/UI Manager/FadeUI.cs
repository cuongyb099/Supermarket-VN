using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        if(!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void FadeIn(float duration)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => canvasGroup.blocksRaycasts = true);
    }

    public void FadeOut(float duration)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.Linear);
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}