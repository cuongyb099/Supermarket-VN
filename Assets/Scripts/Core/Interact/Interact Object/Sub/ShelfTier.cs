using System.Collections.Generic;
using Core.Input;
using Core.Interact;
using Core.Utilities;
using DG.Tweening;
using UnityEngine;

public class ShelfTier : InteractObject
{
    protected List<Vector3> productsPosition = new List<Vector3>();
    protected List<Product> products = new List<Product>();
    protected Grid3D grid;
    
    [SerializeField] protected TrajectoryCurveSO trajectoryCurve;
    
    protected override void Awake()
    {
        this.outline = GetComponentInParent<OutlineBase>();
        grid = GetComponent<Grid3D>();
    }
    
    protected override void OnInteract(Interactor source)
    {
        if(!trajectoryCurve) return;
        
        var currentItem = source.CurrentObjectInHand;

        if (InputManager.Instance.PlayerInputMap.Default.Interact.WasPressedThisFrame())
        {
            AddProduct(currentItem);
            return;
        }

        RemoveProduct(currentItem);
    }

    private void RemoveProduct(InteractObject currentItem)
    {
        if(products.Count == 0) return;
        
        int lastIndex = products.Count - 1;
        var product = products[lastIndex];
        
        if(!currentItem || currentItem is not Box box || !box.AddProduct(product)) return;
        
        products.RemoveAt(lastIndex);
    }
    
    private void AddProduct(InteractObject currentItem)
    {
        if(!currentItem || currentItem is not Box box) return;

        if(!box.RemoveProduct(out Product product)) return;

        if (products.Count == 0)
        {
            ProductUtilities.CalculateProductPosition(product, grid, ref productsPosition, true);
        }
        
        if(products.Count == productsPosition.Count) return;
        
        product.transform.SetParent(this.transform);
        products.Add(product);
        
        int tempIndex = Mathf.Clamp(products.Count - 1, 0, products.Count - 1);
        
        product.transform.DoProductCurveAnim(trajectoryCurve, productsPosition[tempIndex]);
        
        DOVirtual.DelayedCall(trajectoryCurve.Duration * 0.35f, () =>
        {
            product.RenderOnTop.ReturnDefault();
        });
    }

    public override void Focus(Interactor source)
    {
        if (source.CurrentInteractMode != InteractMode.HoldingItem || source.CurrentObjectInHand is not Box)
        {
            this.CanInteract = false;
            ResetInteract().Forget();
            return;
        }
        
        outline.EnableOutline();
        ResetInteract().Forget();
    }
}
