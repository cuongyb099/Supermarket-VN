using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderPlayerPrefSave : MonoBehaviour
{
    [SerializeField] protected string saveKey;
    protected Slider slider;
    
    protected virtual void Awake()
    {
        slider = GetComponent<Slider>();
        Load();
        slider.onValueChanged.AddListener(HandleValueChange);
    }

    protected virtual void Load()
    {
        slider.value = PlayerPrefs.GetFloat(saveKey, 1);
    }

    protected virtual void HandleValueChange(float value)
    {
        PlayerPrefs.SetFloat(saveKey, value);
    }
}
