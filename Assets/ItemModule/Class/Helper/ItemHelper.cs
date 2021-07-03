using System.Collections;
using System.Collections.Generic;
using ItemModule.Class.Data;
using PlayerModule.Class.Data;
using UnityEngine;

public struct ItemData
{
    public string Name;
    public string Description;
    public int Cost;
    public ItemCategory ItemCategory;
    public Team ItemOwner;
    public int SpriteIndex;
    public string ResourcesName;
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
            ItemCategory = ItemCategory.Placement,
            ItemOwner = Team.Defender,
            SpriteIndex = 0,
            ResourcesName = "NumericChest",
        }},
        {ItemType.AlphanumericChest, new ItemData()
        {
            Name = "AlphaNumeric Chest", 
            Description = "AlphaNumeric Chest DESC",
            Cost = 6,
            ItemCategory = ItemCategory.Placement,
            ItemOwner = Team.Defender,
            SpriteIndex = 1,
            ResourcesName = "AlphanumericChest",
        }},
        {ItemType.NormalChest, new ItemData()
        {
            Name = "AllAble Chest", 
            Description = "AllAble Chest DESC",
            Cost = 9,
            ItemCategory = ItemCategory.Placement,
            ItemOwner = Team.Defender,
            SpriteIndex = 2,
            ResourcesName = "NormalChest",
        }},
        {ItemType.AntiVirus, new ItemData()
        {
            Name = "AntiVirus", 
            Description = "AntiVirus DESC",
            Cost = 3,
            ItemCategory = ItemCategory.Tool,
            ItemOwner = Team.Defender,
            SpriteIndex = 0,
            ResourcesName = "AntiVirusTool",
        }},
        {ItemType.Firewall, new ItemData()
        {
            Name = "Firewall", 
            Description = "Firewall DESC",
            Cost = 7,
            ItemCategory = ItemCategory.Tool,
            ItemOwner = Team.Defender,
            SpriteIndex = 1,
            ResourcesName = "FirewallTool",
        }},
        {ItemType.BruteForce, new ItemData()
        {
            Name = "BruteForce", 
            Description = "BruteForce DESC",
            Cost = 3,
            ItemCategory = ItemCategory.Placement,
            ItemOwner = Team.Hacker,
            SpriteIndex = 0,
            ResourcesName = "BruteForce",
        }},
        {ItemType.Phishing, new ItemData()
        {
            Name = "Phishing", 
            Description = "Phishing DESC",
            Cost = 6,
            ItemCategory = ItemCategory.Placement,
            ItemOwner = Team.Hacker,
            SpriteIndex = 1,
            ResourcesName = "Phishing",
        }},
        {ItemType.Spyware, new ItemData()
        {
            Name = "SpyWare", 
            Description = "SpyWare DESC",
            Cost = 9,
            ItemCategory = ItemCategory.Placement,
            ItemOwner = Team.Hacker,
            SpriteIndex = 2,
            ResourcesName = "Spyware",
        }},
        {ItemType.Trojan, new ItemData()
        {
            Name = "Trojan", 
            Description = "Trojan DESC",
            Cost = 3,
            ItemCategory = ItemCategory.Tool,
            ItemOwner = Team.Hacker,
            SpriteIndex = 0,
            ResourcesName = "TrojanTool",
        }},
        {ItemType.Ransomware, new ItemData()
        {
            Name = "Ransomware", 
            Description = "Ransomware DESC",
            Cost = 7,
            ItemCategory = ItemCategory.Tool,
            ItemOwner = Team.Hacker,
            SpriteIndex = 1,
            ResourcesName = "RansomwareTool",
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
    public static ItemCategory GetItemCategory(ItemType itemType)
    {
        return ItemDataList[itemType].ItemCategory;
    }
    public static Team GetItemOwner(ItemType itemType)
    {
        return ItemDataList[itemType].ItemOwner;
    }

    public static int GetSpriteIndex(ItemType itemType)
    {
        return ItemDataList[itemType].SpriteIndex;
    }
}
