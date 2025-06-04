using DG.Tweening;
using UnityEngine;

public static class UIUtilities
{
    public static void FadeOutWithCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, duration)
            .SetEase(Ease.Linear);
    }
    
    public static void FadeInWithCanvasGroup(CanvasGroup canvasGroup, float duration)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1f, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => canvasGroup.blocksRaycasts = true);
    }
}