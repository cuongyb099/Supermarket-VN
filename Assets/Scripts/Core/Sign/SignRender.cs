using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core.Sign
{
    public class SignRender : MonoBehaviour
    {
        [SerializeField] public float SignAnimationSecond;
        [SerializeField] public Vector3 MinScale;
        [SerializeField] public Vector3 MaxScale;
        
        private Sequence _sequenceTween;
        private Material _material;
        private Texture2D _signOpenTexture;
        private Texture2D _signClosedTexture;
        private bool _loadTexture2DDone;
        
        protected void Awake()
        {
            _loadTexture2DDone = false;
            _ = LoadTexture2D();
            _material = GetComponent<MeshRenderer>().sharedMaterial;
            // _material.mainTexture = _signOpenTexture;
            StartCoroutine(SetDefaultTexture());
        }

        IEnumerator SetDefaultTexture()
        {
            yield return new WaitUntil(() => _loadTexture2DDone);
            _material.mainTexture = _signOpenTexture;
        }

        private async UniTaskVoid LoadTexture2D()
        {
            const string SignOpenStore = "sign_open_store";
            const string SignClosedStore = "sign_closed_store";

            var texture1 = AddressablesManager.Instance.LoadAssetAsync<Texture2D>(SignOpenStore);
            var texture2 = AddressablesManager.Instance.LoadAssetAsync<Texture2D>(SignClosedStore);
            
            var textures = await UniTask.WhenAll(texture1, texture2);

            _signOpenTexture = textures.Item1;
            _signClosedTexture = textures.Item2;
            
            _loadTexture2DDone = true;
            
            AddressablesManager.Instance.Release(SignOpenStore);
            AddressablesManager.Instance.Release(SignClosedStore);
        }

        void StartAnimation(bool openStatus)
        {
            _sequenceTween = DOTween.Sequence();
            _sequenceTween.Append(transform.DOScale(MinScale, SignAnimationSecond))
                          .AppendCallback(() =>
                          {
                              if (!_material)
                              {
                                  return;
                              }
                              _material.mainTexture = openStatus ? _signOpenTexture : _signClosedTexture;
                              // _material.mainTexture = _signClosedTexture;
                              // Debug.Log("Callback " + _material.mainTexture.name);
                          })
                          .Append(transform.DOScale(MaxScale, SignAnimationSecond));
        }
        
        public void ShowOpen()
        {
            StartAnimation(openStatus : true);
            // Debug.Log("Show Open");
        }

        public void ShowClose()
        {
            StartAnimation(openStatus : false);
            // Debug.Log("Show Close");
        }
    }
}