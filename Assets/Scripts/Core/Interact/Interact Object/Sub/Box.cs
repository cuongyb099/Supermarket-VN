using System.Collections;
using System.Collections.Generic;
using Core.Constant;
using Core.Input;
using Core.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Interact
{
    public class Box : PlacableObject
    {
        [SerializeField] protected Grid3D Grid3D;
        [SerializeField] protected Animator anim;
        [SerializeField] protected TrajectoryCurveSO trajectoryCurve;
        
        public bool IsOpen { get; protected set; }
        
        protected const float animTransition = 0.25f;
        protected Coroutine coroutine;
        protected List<Product> products = new List<Product>();
        protected List<Vector3> productsPosition = new List<Vector3>();
        protected IIndicatable indicatable;
        
        protected override void Reset()
        {
            base.Reset();
            CanInteract = true;
            Grid3D = GetComponentInChildren<Grid3D>();
            anim = GetComponentInChildren<Animator>();
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

            if(!TryGetComponent(out indicatable)) return;
            indicatable.OnEnableIndicator += (color) =>
            {
                foreach (var product in products)
                {
                    product.GetIndicatable().EnableIndicator(color);
                }
            };
            indicatable.OnDisableIndicator += () =>
            {
                foreach (var product in products)
                {
                    product.GetIndicatable().DisableIndicator();
                }
            };
        }
        
        public bool AddProduct(Product product)
        {
            if (!IsOpen)
            {
                return false;
            }
            
            product.RenderOnTop.SetOnTop();
            products.Add(product);
            product.transform.SetParent(Grid3D.transform);
            int tempIndex = Mathf.Clamp(products.Count - 1, 0, products.Count - 1);
            product.transform.DoProductCurveAnim(trajectoryCurve, productsPosition[tempIndex]);
            return true;
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

        public Product prefab;

        protected override void OnInteract(Interactor source)
        {
            base.OnInteract(source);
            
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            
            rb.constraints = RigidbodyConstraints.None;
            coroutine = StartCoroutine(CheckInput(source));
        }

        protected void OpenBox()
        {
            anim.CrossFadeInFixedTime(AnimConstant.OpenBox, animTransition);
        }
        
        protected void CloseBox()
        {
            anim.CrossFadeInFixedTime(AnimConstant.CloseBox, animTransition);
        }

        private IEnumerator CheckInput(Interactor source)
        {
            InputAction close = InputManager.Instance.PlayerInputMap.Default.Close;
            
            while (true)
            {
                if (source.CurrentInteractMode != InteractMode.HoldingItem)
                {
                    yield return null;
                    continue;
                }
                
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
            StopCoroutine(coroutine);
        }

        public override void SetOnTop()
        {
            if(isTopLayer) return;
            
            renderOnTop.SetOnTop();
            foreach (var product in products)
            {
                product.RenderOnTop.SetOnTop();
            }

            isTopLayer = true;
        }

        public override void ToDefaultRender()
        {
            if(!isTopLayer) return;
            
            renderOnTop.ReturnDefault();
            foreach (var product in products)
            {
                product.RenderOnTop.ReturnDefault();
            }
            isTopLayer = false;
        }

      
    }
}
