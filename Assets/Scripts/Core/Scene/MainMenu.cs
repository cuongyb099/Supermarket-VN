using Core.Constant;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.Scene
{
    public class MainMenu : SceneCtrlBase<MainMenu>
    {
        public Button btn;
        public UIManager.UIManager uiManager;
        protected override void OnAwake()
        {
            btn.onClick.AddListener(() =>
            {
                SceneManager.LoadSceneAsync(SceneConstant.Loading, LoadSceneMode.Additive);
                Loading.AddLoadingTask(async () =>
                {
                    _ = SceneManager.UnloadSceneAsync(SceneConstant.MainMenu);
                    
                    Loading.Instance.ChangeLoadingDescription("Load Environment");
                    await SceneManager.LoadSceneAsync(SceneConstant.Environment, LoadSceneMode.Additive);
                    await SceneManager.LoadSceneAsync(SceneConstant.Gameplay, LoadSceneMode.Additive);
                });
            });
            
            
        }
    }
}

