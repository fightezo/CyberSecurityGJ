using System.Collections;
using System.Collections.Generic;
using ItemModule.Class.Data;
using UnityEngine;

public struct ItemData
{
    public string Name;
    public string Description;
    public int Cost;
}
public static class ItemHelper
{
    public static Dictionary<ItemType, ItemData> ItemDataList = new Dictionary<ItemType, ItemData>()
    {
        {ItemType.NumericChest, new ItemData()
        {
            Name = "Numeric Chest", 
            Description = "",
            Cost = 1,
        }},
    };

    public static string GetName(ItemType itemType)
    {
        return ItemDataList[itemType].Name;
    }
    
    public static string GetDescription(ItemType itemType)
    {
        return ItemDataList[itemType].Description;
    }
    public static int GetCost(ItemType itemType)
    {
        return ItemDataList[itemType].Cost;
    }
}
