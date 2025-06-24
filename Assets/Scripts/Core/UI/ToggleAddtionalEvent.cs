using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class ToggleAddtionalEvent : MonoBehaviour
{
    public UnityEvent OnToggleTrue;
    public UnityEvent OnToggleFalse;

    protected Toggle toggle;

    protected virtual void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener((result) =>
        {
            if (result)
            {
                OnToggleTrue?.Invoke();
                return;
            }
            
            OnToggleFalse?.Invoke();
        });
    }
}