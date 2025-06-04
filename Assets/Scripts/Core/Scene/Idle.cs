using Core.Constant;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Scene
{
    public class Idle : SceneCtrlBase<Idle>
    {
        private void Start()
        {
            SceneManager.LoadSceneAsync(SceneConstant.Loading, LoadSceneMode.Additive);
            
            Loading.AddLoadingTask(async () =>
            {
                Loading.Instance.ChangeLoadingDescription("UnLoad Idle");
                await SceneManager.UnloadSceneAsync(SceneConstant.Idle);
            });
            Loading.AddLoadingTask(async () =>
            {
                Loading.Instance.ChangeLoadingDescription("Load Main Menu");
                await SceneManager.LoadSceneAsync(SceneConstant.MainMenu, LoadSceneMode.Additive);
            });
        }
    }
}
