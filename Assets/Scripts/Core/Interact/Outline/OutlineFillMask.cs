using System.Linq;
using Core.Constant;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Interact
{
    [DisallowMultipleComponent]
    public class OutlineFillMask : OutlineBase, IIndicatable
    {
        public enum OutlineMode 
        {
            OutlineAll,
            OutlineVisible,
            OutlineHidden,
            OutlineAndSilhouette,
            SilhouetteOnly
        }

        [Header("Uncheck Static Batching To Outline Object")]
        [SerializeField, Tooltip("Don't Need Drag Ref So In Here")] 
        protected OutlineConfigSO config; 

        private static Material outlineMaskMaterial;
        private static Material outlineFillMaterial;
        
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
        
        private void Awake() 
        {
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

        public override void EnableOutline()
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

        public override void DisableOutline()
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

        public override void SetConfig(OutlineConfigSO config)
        {
            this.config = config;
        }

        private void UpdateMaterialProperties() 
        {
              outlineFillMaterial.SetColor(OutlineColor, config.OutlineColor);

              switch (config.OutlineMode) 
              {
                case OutlineMode.OutlineAll:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(OutlineWidth, config.OutlineWidth);
                  break;

                case OutlineMode.OutlineVisible:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                  outlineFillMaterial.SetFloat(OutlineWidth, config.OutlineWidth);
                  break;

                case OutlineMode.OutlineHidden:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Greater);
                  outlineFillMaterial.SetFloat(OutlineWidth, config.OutlineWidth);
                  break;

                case OutlineMode.OutlineAndSilhouette:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Always);
                  outlineFillMaterial.SetFloat(OutlineWidth, config.OutlineWidth);
                  break;

                case OutlineMode.SilhouetteOnly:
                  outlineMaskMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                  outlineFillMaterial.SetFloat(ZTest, (float)UnityEngine.Rendering.CompareFunction.Greater);
                  outlineFillMaterial.SetFloat(OutlineWidth, 0f);
                  break;
        }
      }

        public void EnableIndicator(Color color)
        {
            foreach (var meshRenderer in renderers)
            {
                foreach (var material in meshRenderer.materials)
                {
                    material.SetFloat(MaterialConstant.SurfaceID, 1);
                    material.SetFloat(MaterialConstant.ZWrite, 1);
                    material.SetFloat(MaterialConstant.SourceBlend, 5); // SrcAlpha
                    material.SetFloat(MaterialConstant.DestBlend, 10); // OneMinusSrcAlpha
                    material.SetFloat(MaterialConstant.Cutout, 0); // Cutout
                    material.DisableKeyword(MaterialConstant.CutoutKeyword);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                    material.color = color;
                }

                meshRenderer.staticShadowCaster = false;
            }
        }

        public void DisableIndicator()
        {
            foreach (var meshRenderer in renderers)
            {
                foreach (var material in meshRenderer.materials)
                {
                    material.SetFloat(MaterialConstant.SurfaceID, 0);
                    material.SetFloat(MaterialConstant.ZWrite, 1);
                    material.SetFloat(MaterialConstant.SourceBlend, 1); 
                    material.SetFloat(MaterialConstant.DestBlend, 0); 
                    material.SetFloat(MaterialConstant.Cutout, 0); 
                    material.DisableKeyword(MaterialConstant.CutoutKeyword);
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
                    material.color = Color.white;
                }
                
                meshRenderer.staticShadowCaster = true;
            }
        }
    }
}
