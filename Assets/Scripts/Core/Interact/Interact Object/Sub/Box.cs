using System.Collections;
using System.Collections.Generic;
using Core.Constant;
using Core.Input;
using Core.Utilities;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interact
{
    public class Box : PlacableObject
    {
        [SerializeField] protected Grid3D Grid3D;
        [SerializeField] protected Animator anim;
        public bool IsOpen { get; protected set; }
        protected const float animTransition = 0.25f;
        protected Coroutine coroutine;
        protected List<Product> products = new List<Product>();
        protected List<Vector3> productsPosition = new List<Vector3>();
        [SerializeField] protected TrajectoryCurveSO trajectoryCurve;
        
        protected override void Reset()
        {
            base.Reset();
            CanInteract = true;
            Grid3D = GetComponentInChildren<Grid3D>();
            anim = GetComponentInChildren<Animator>();
        }

        public void AddProduct(Product product)
        {
            //var firstItem = products[0];
            //if(firstItem.GetItemData().ID != product.GetItemData().ID) return;
            product.RenderOnTop.SetOnTop();
            products.Add(product);
            product.transform.SetParent(Grid3D.transform);
            int tempIndex = Mathf.Clamp(products.Count - 1, 0, products.Count - 1);
            Transform productTransform = product.transform;
            productTransform.DOKill();
            Vector3 targetPosition = productsPosition[tempIndex];
            DOVirtual.Float(0f, 1f, trajectoryCurve.Duration, (normalizedTime) =>
            {
                var tempPosition = Vector3.Lerp(productTransform.localPosition, targetPosition, normalizedTime);
                tempPosition.y += trajectoryCurve.Curve.Evaluate(normalizedTime) * trajectoryCurve.MaxHeight;
                productTransform.localPosition = tempPosition;
                productTransform.localRotation = Quaternion.Lerp(productTransform.localRotation, Quaternion.identity, normalizedTime);
            }).SetEase(trajectoryCurve.EaseMode);
        }

        public bool RemoveProduct(out Product product)
        {
            product = null;
            
            if(!IsOpen || products.Count == 0) return false;
            
            int lastIndex = products.Count - 1;
            product = products[lastIndex];
            products.RemoveAt(lastIndex);
            return true;
        }

        protected override void Awake()
        {
            base.Awake();
            ProductUtilities.CalculateProductPosition(prefab, Grid3D, ref productsPosition);
            foreach (var position in productsPosition)
            {
                var clone = Instantiate(prefab, Grid3D.transform, true);
                clone.transform.localPosition = position;
                clone.transform.localRotation = Quaternion.identity;
                products.Add(clone);
            }
        }

        public Product prefab;

        protected override void OnInteract(Interactor source)
        {
            base.OnInteract(source);
            
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            
            rb.constraints = RigidbodyConstraints.None;
            coroutine = StartCoroutine(CheckInput());

            foreach (var product in products)
            {
                product.RenderOnTop.SetOnTop();
            }
        }

        protected void OpenBox()
        {
            anim.CrossFadeInFixedTime(AnimConstant.OpenBox, animTransition);
        }
        
        protected void CloseBox()
        {
            anim.CrossFadeInFixedTime(AnimConstant.CloseBox, animTransition);
        }

        private IEnumerator CheckInput()
        {
            InputAction close = InputManager.Instance.PlayerInputMap.Default.Close;
            
            while (true)
            {
                if (close.WasPressedThisFrame())
                {
                    IsOpen = !IsOpen;
                    if (IsOpen) OpenBox();
                    else CloseBox();
                }
                
                yield return null;
            }
        }
        
        public override void ResetToIdle()
        {
            base.ResetToIdle();
            foreach (var product in products)
            {
                product.RenderOnTop.ReturnDefault();
            }
            StopCoroutine(coroutine);
        }
        
        public Transform StartPoint;
        public Transform EndPoint;
        public int DebugCount;
        private void OnDrawGizmos()
        {
            if(!Application.isPlaying) return;
        
            Gizmos.color = Color.green;
        
            for (int i = 1; i < DebugCount; i++)
            {
                var start = Vector3.Lerp(StartPoint.position, EndPoint.position, (float) (i - 1) / DebugCount);
                var end = Vector3.Lerp(StartPoint.position, EndPoint.position, (float) i / DebugCount);
                start.y += this.trajectoryCurve.Curve.Evaluate((float) (i - 1) / DebugCount) * this.trajectoryCurve.MaxHeight;
                end.y += this.trajectoryCurve.Curve.Evaluate((float) i / DebugCount) * this.trajectoryCurve.MaxHeight;
                Gizmos.DrawLine(start, end);
            }
        }
    }
}
