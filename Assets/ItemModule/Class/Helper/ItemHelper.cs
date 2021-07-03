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
            Description = "Numeric Chest DESC",
            Cost = 3,
        }},
        {ItemType.AlphanumericChest, new ItemData()
        {
            Name = "AlphaNumeric Chest", 
            Description = "AlphaNumeric Chest DESC",
            Cost = 6,
        }},
        {ItemType.NormalChest, new ItemData()
        {
            Name = "AllAble Chest", 
            Description = "AllAble Chest DESC",
            Cost = 9,
        }},
        {ItemType.AntiVirus, new ItemData()
        {
            Name = "AntiVirus", 
            Description = "AntiVirus DESC",
            Cost = 3,
        }},
        {ItemType.Firewall, new ItemData()
        {
            Name = "Firewall", 
            Description = "Firewall DESC",
            Cost = 7,
        }},
        {ItemType.BruteForce, new ItemData()
        {
            Name = "BruteForce", 
            Description = "BruteForce DESC",
            Cost = 3,
        }},
        {ItemType.Phishing, new ItemData()
        {
            Name = "Phishing", 
            Description = "Phishing DESC",
            Cost = 6,
        }},
        {ItemType.Spyware, new ItemData()
        {
            Name = "SpyWare", 
            Description = "SpyWare DESC",
            Cost = 9,
        }},
        {ItemType.Trojan, new ItemData()
        {
            Name = "Trojan", 
            Description = "Trojan DESC",
            Cost = 3,
        }},
        {ItemType.Ransomware, new ItemData()
        {
            Name = "Ransomware", 
            Description = "Ransomware DESC",
            Cost = 7,
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
