using KatJsonInventory.Item;
using UnityEngine;

public abstract class ItemWithSize : ItemBase
{
    public abstract Vector3Int GetSize();
}