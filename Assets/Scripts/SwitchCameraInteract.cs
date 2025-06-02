using System.Collections;
using Core;
using Core.Input;
using Core.Interact;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCameraInteract : InteractObject
{
    [SerializeField] protected Transform camPoistion;
    [SerializeField] protected UIStackManager pcUIManger;
    
    protected override void OnInteract(Transform source)
    {
        CameraManager.Instance.SwitchVitualCamera(camPoistion);
        this.CanInteract = false;
        StartCoroutine(CheckInput());
    }

    private IEnumerator CheckInput()
    {
        var defaultMap = InputManager.Instance.PlayerInputMap.Default;
        Cursor.lockState = CursorLockMode.None;
        InputAction exit = defaultMap.Exit;
        defaultMap.Look.Disable();
        defaultMap.Move.Disable();
        
        while (true)
        {
            if (exit.WasPressedThisFrame())
            {
                if (!pcUIManger.HideCurrentPanel())
                {
                    CameraManager.Instance.ToDefaultVitualCamera();
                    defaultMap.Look.Enable();
                    defaultMap.Move.Enable();
                    Cursor.lockState = CursorLockMode.Locked;
                    this.CanInteract = true;
                    yield break;    
                }
            }
            
            yield return null;
        }
    }
}
