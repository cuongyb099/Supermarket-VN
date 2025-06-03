using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Config")]
public class ItemProductSO : ScriptableObject
{
    public List<ItemProductBase> lsItemProducts;

    public ItemProductBase GetItemProductById(int idProduct)
    {
        foreach(var item in this.lsItemProducts) if(item.idProduct == idProduct) return item;
        return null;
    }
}

[System.Serializable]
public class ItemProductBase
{
    public int idProduct;
    public string nameProduct;
    public Vector3 size;
    public int amountPerBox;
    public float sellPrice;
    public float buyPrice;
    public int boxRef;
}
