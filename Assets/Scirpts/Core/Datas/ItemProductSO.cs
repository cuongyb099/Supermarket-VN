using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemProduct_Type
{
    None = 0,
    Food = 1,
    Tech = 2,
}


[CreateAssetMenu(menuName = "Item Config")]
public class ItemProductSO : ScriptableObject
{
    public List<ItemFood> lsFoods;
    public List<ItemTech> lsTechs;


    public ItemFood GetItemFoodById(string id)
    {
        foreach(var food in this.lsFoods) if(food.id == id) return food;
        return null;
    }

    public ItemTech GetItemTechById(string id)
    {
        foreach (var tech in this.lsTechs) if (tech.id == id) return tech;
        return null;
    }
}

[System.Serializable]
public class ItemProduct
{
    public string id;
    public string name;
    public float sellPrice;
    public float buyPrice;
    public ItemProduct_Type itemType;
}
[System.Serializable]
public class ItemFood : ItemProduct
{

}

[System.Serializable]
public class ItemTech: ItemProduct
{

}
