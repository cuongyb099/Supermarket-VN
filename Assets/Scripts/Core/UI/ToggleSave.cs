using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleSave : MonoBehaviour
{
    public string SaveKey;
    public int SaveValue = 0;
    
    public UnityEvent OnToggleTrue;
    public UnityEvent OnToggleFalse;

    protected Toggle toggle;

    protected virtual void Awake()
    {
        toggle = GetComponent<Toggle>();
        Load();
        toggle.onValueChanged.AddListener((result) =>
        {
            if (result)
            {
                OnToggleTrue?.Invoke();
                PlayerPrefs.SetInt(SaveKey, SaveValue);
                return;
            }
            
            OnToggleFalse?.Invoke();
        });
    }

    protected virtual void Load()
    {
        if(PlayerPrefs.GetInt(SaveKey, SaveValue) != SaveValue) return;
        
        toggle.isOn = true;
    }
}