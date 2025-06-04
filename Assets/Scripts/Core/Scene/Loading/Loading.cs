using System;
using System.Collections.Generic;
using System.Linq;
using Core.Constant;
using Core.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Scene
{
    public class Loading : SceneCtrlBase<Loading>
    {
        public static LoadingConfig Config = new LoadingConfig();
        public static Action OnLoadDone;
        protected UIManager.UIManager uiManager;
        protected LoadingBarBase loadingBar;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Init()
        {
            ResetScene();
        }
        
        private static List<Func<UniTask>> _loadingTasks = new List<Func<UniTask>>();

        protected override void OnAwake()
        {
            uiManager = FindAnyObjectByType<UIManager.UIManager>();
        }

        public static void AddLoadingTask(Func<UniTask> task)
        {
            _loadingTasks.Add(task);
        }

        public void ChangeLoadingDescription(string description)
        {
            loadingBar?.SetDescription(description);
        }
        
        private void Start()
        {
            if(!Config.LoadOnStart) return;
            
            StartLoading().Forget();
        }

        public async UniTaskVoid StartLoading()
        {
            var cancelToken = this.GetCancellationTokenOnDestroy();
            
            while (_loadingTasks.Count == 0)
            {
                await UniTask.Yield(cancelToken);
            }
            
            await UniTask.Yield(cancelToken);
            
            uiManager.SetUIActive(true);
            loadingBar ??= uiManager.GetFirstPanelOfType<LoadingPanel>().LoadingBar;
            loadingBar.ResetProgress();
            
            await SetLoadingProgress(Config.BeginProgress, Config.BeginFillTime);

            float progressEachTask = (Config.EndProgress - Config.BeginProgress) / _loadingTasks.Count;
            
            foreach (var task in _loadingTasks)
            {
                await task();
                await SetLoadingProgress(loadingBar.CurrentProgress + progressEachTask, Config.FillTimeEachTask);
                ChangeLoadingDescription(string.Empty);
            }
            
            await UniTask.Yield(cancelToken);
            
            await SetLoadingProgress(1f, Config.EndFillTime);
            
            OnLoadDone?.Invoke();
            ResetScene();
            uiManager.SetUIActive(false);
            
            if (Config.UnLoadSceneOnEnd)
            {
                await SceneManager.UnloadSceneAsync(SceneConstant.Loading);
            }
        }

        protected UniTask SetLoadingProgress(float progress, float duration)
        {
            var cancelToken = this.GetCancellationTokenOnDestroy();
            
            DOVirtual.Float(loadingBar.CurrentProgress, progress, duration, (curProgress) =>
            {
                loadingBar.SetProgress(curProgress);
            });

            return UniTask.WaitForSeconds(duration, cancellationToken: cancelToken);
        }
        
        
        private static void ResetScene()
        {
            _loadingTasks.Clear();
            OnLoadDone = null;
            Config.Reset();
        }
    }
}
