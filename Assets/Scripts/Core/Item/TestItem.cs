using Core.Interact;
using KatJsonInventory.Item;
using UnityEngine;

public class TestItem : Product
{
    protected IIndicatable indicatable;
    public Vector3Int Size;
    public RenderOnTop Render;

    private void Awake()
    {
        indicatable = GetComponent<IIndicatable>();
    }

    public override void Init(ItemData itemData)
    {
        throw new System.NotImplementedException();
    }

    public override ItemData GetItemData()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3Int GetSize() => Size;
    
    public override RenderOnTop RenderOnTop => Render;
    
    public override IIndicatable GetIndicatable() => indicatable;
}