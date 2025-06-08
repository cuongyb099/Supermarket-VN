using Core.Interact;
using KatJsonInventory.Item;
using UnityEngine;

public abstract class Product : ItemBase
{
    public abstract Vector3Int GetSize();
    public abstract RenderOnTop RenderOnTop { get; }
    public abstract IIndicatable GetIndicatable();
}