using System;
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
        
        protected void Awake()
        {
            
        }

        void StartAnimation()
        {
            _sequenceTween = DOTween.Sequence();
            _sequenceTween.Append(transform.DOScale(MinScale, SignAnimationSecond))
                          .Append(transform.DOScale(MaxScale, SignAnimationSecond));
        }
        
        public void ShowOpen()
        {
            StartAnimation();
            Debug.Log("Show Open");
        }

        public void ShowClose()
        {
            StartAnimation();
            Debug.Log("Show Close");
        }
    }
}