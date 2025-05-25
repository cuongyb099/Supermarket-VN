using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Interact
{
    [DisallowMultipleComponent]
    public class Outline : MonoBehaviour 
    {
        public enum OutlineMode 
        {
            OutlineAll,
            OutlineVisible,
            OutlineHidden,
            OutlineAndSilhouette,
            SilhouetteOnly
        }

        [Header("Need Bake Mesh Before Enter")]
        public OutlineConfigSO OutlineConfig;

        private Renderer[] renderers;
        private static Material outlineMaskMaterial;
        private static Material outlineFillMaterial;
        public bool IsEnable { get; private set; }
        
        private static bool _isLoadDone;
        
        private static readonly int OutlineColor = Shader.PropertyToID("_OutlineColor");
        private static readonly int ZTest = Shader.PropertyToID("_ZTest");
        private static readonly int OutlineWidth = Shader.PropertyToID("_OutlineWidth");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Dispose()
        {
            _isLoadDone = false;
            outlineMaskMaterial = null;
            outlineFillMaterial = null;
        }
        
        private void Awake() {

            renderers = GetComponentsInChildren<Renderer>();
#if UNITY_EDITOR
            GenerateSmoothNormalUV.Bake(GetComponentsInChildren<MeshFilter>().ToList());
#endif
              if(_isLoadDone) return;
            _ = LoadMaterials();
        }

        private async UniTaskVoid LoadMaterials()
        {
            _isLoadDone = true;
            const string outlineMaskKey = "OutlineMask";
            const string outlineFillKey = "OutlineFill";
            
            var task1 = AddressablesManager.Instance.LoadAssetAsync<Material>(outlineMaskKey);
            var task2 = AddressablesManager.Instance.LoadAssetAsync<Material>(outlineFillKey);
            
            var result = await UniTask.WhenAll(task1 , task2);
            
            outlineMaskMaterial = Instantiate(result.Item1);
            outlineFillMaterial = Instantiate(result.Item2);
            
            AddressablesManager.Instance.Release(outlineMaskKey);
            AddressablesManager.Instance.Release(outlineFillKey);
        }

        [ContextMenu("Enable Outline")]
        public void EnableOutline()
        {
            if(IsEnable) return;
            
            IsEnable = true;
            foreach (var meshRenderer in renderers)
            {
                int length = meshRenderer.sharedMaterials.Length;
                var newMaterial = new Material[length + 2];

                for (var i = 0; i < length; i++)
                {
                    newMaterial[i] = meshRenderer.sharedMaterials[i];
                }

                newMaterial[length] = outlineMaskMaterial;
                newMaterial[length + 1] = outlineFillMaterial;
                
                meshRenderer.sharedMaterials = newMaterial;
            }
            
            UpdateMaterialProperties();
        }

        [ContextMenu("Disable Outline")]
        public void DisableOutline()
        {
            if(!IsEnable) return;
            
            IsEnable = false;
            foreach (var meshRenderer in renderers)
            {
                int length = meshRenderer.sharedMaterials.Length - 2;
                var newMaterial = new Material[length];

                for (var i = 0; i < length; i++)
                {
                    newMaterial[i] = meshRenderer.sharedMaterials[i];
                }
                    
                meshRenderer.sharedMaterials = newMaterial;
            }
            
            UpdateMaterialProperties();
        }

        private void UpdateMaterialProperties() 
        {
              outlineFillMaterial.SetColor(OutlineColor, OutlineConfig.OutlineColor);

              switch (OutlineConfig.OutlineMode) 
              {
                case OutlineMode.OutlineAll:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(OutlineWidth, OutlineConfig.OutlineWidth);
                  break;

                case OutlineMode.OutlineVisible:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                  outlineFillMaterial.SetFloat(OutlineWidth, OutlineConfig.OutlineWidth);
                  break;

                case OutlineMode.OutlineHidden:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Greater);
                  outlineFillMaterial.SetFloat(OutlineWidth, OutlineConfig.OutlineWidth);
                  break;

                case OutlineMode.OutlineAndSilhouette:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(OutlineWidth, OutlineConfig.OutlineWidth);
                  break;

                case OutlineMode.SilhouetteOnly:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Greater);
                  outlineFillMaterial.SetFloat(OutlineWidth, 0f);
                  break;
        }
      }
    }
}
