[System.Serializable]
public class LoadingConfig
{
    public bool LoadOnStart = true;
    public bool UnLoadSceneOnEnd = true;
        
    public float BeginProgress = 0.2f;
    public float BeginFillTime = 0.5f;
    public float EndProgress = 0.8f;
    public float EndFillTime = 0.5f;
    public float FillTimeEachTask = 0.5f;

    public void Reset()
    {
        LoadOnStart = true;
        UnLoadSceneOnEnd = true;
        
        BeginProgress = 0.2f;
        EndProgress = 0.8f;
        BeginFillTime = 0.5f;
        EndFillTime = 0.5f;
        FillTimeEachTask = 0.5f;
    }
}