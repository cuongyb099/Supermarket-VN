using UnityEngine;

public class MainLight : MonoBehaviour
{
    private void Awake()
    {
        RenderSettings.sun = GetComponent<Light>();
    }
}
