using KatJsonInventory.Item;
using UnityEngine;

public class Test : ItemWithSize
{
    public Vector3Int Size;
    
    public override void Init(ItemData itemData)
    {
        throw new System.NotImplementedException();
    }

    public override ItemData GetItemData()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3Int GetSize() => Size;
}