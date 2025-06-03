using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBase : MonoBehaviour
{
    public Button BTN { get; private set; }

    protected virtual void Awake()
    {
        BTN = GetComponent<Button>();
    }
}
