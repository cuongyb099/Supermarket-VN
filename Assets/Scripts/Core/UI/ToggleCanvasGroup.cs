using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCanvasGroup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool isOn;

    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Awake()
    {
        canvasGroup.alpha = isOn ? 1 : 0;
        canvasGroup.blocksRaycasts = isOn;
        canvasGroup.interactable = isOn;
    }

    public void Activate()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void Deactivate()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }
}
