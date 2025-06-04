using Core.Interact;
using KatJsonInventory.Item;
using UnityEngine;

public class TestItem : Product
{
    public override void Init(ItemData itemData)
    {
        throw new System.NotImplementedException();
    }

    public override ItemData GetItemData()
    {
        throw new System.NotImplementedException();
    }

    public Vector3Int Size;
    public override Vector3Int GetSize() => Size;
    public RenderOnTop Render;
    public override RenderOnTop RenderOnTop => Render;
}