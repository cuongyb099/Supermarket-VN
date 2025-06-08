using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FadeUI))]
public class FadeButton : Button, IHidable, IShowable
{
    protected FadeUI fadeUI;
    protected const float _fadeDuration = 0.25f;
    protected bool isVisible;
    
    protected override void Awake()
    {
        base.Awake();
        fadeUI = GetComponent<FadeUI>();
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        
    }

    public bool IsVisible() => isVisible;

    public virtual void Hide()
    {
        fadeUI.FadeOut(_fadeDuration);
        isVisible = false;
    }

    public virtual void Show()
    {
        fadeUI.FadeIn(_fadeDuration);
        isVisible = true;
    }
}