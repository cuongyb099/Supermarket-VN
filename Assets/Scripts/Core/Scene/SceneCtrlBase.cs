using Core.Constant;
using Core.Utilities;
using KatLib.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public abstract class SceneCtrlBase<T> : Singleton<T> where T : SceneCtrlBase<T>
{
    protected override void Awake()
    {
        base.Awake();
        if (!Utilities.CheckSceneIsActive(SceneConstant.Camera))
        {
            SceneManager.LoadScene(SceneConstant.Camera, LoadSceneMode.Additive);
        }
        
        OnAwake();
    }

    protected virtual void OnAwake()
    {
        
    }
}