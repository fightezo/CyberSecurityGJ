using System.Collections;
using System.Collections.Generic;
using GameModule.Class.Interface;
using ItemModule;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component
{
    internal class PlanningPanel : MonoBehaviour, IUIPanel
    {
        public Text CountDownText;
        public Text PointsLeftText;
        public Text PlayerName;
        public List<UIPlanningItem> PlanningChestList;
        public List<UIPlanningItem> PlanningToolList;
 
        [SerializeField] private int _defenderPoints = 10;
        [SerializeField] private int _hackerPoints = 10;
        private int _usedPoints = 0; 
        
        private PlayerManager _playerManager;


        public GameObject GetSelf()
        {
            return gameObject;
        }
        public void EndPhaseUpdate()
        {
            UpdateItemManager();
        }
        public void UpdateView()
        {
            _playerManager =  GameManager.Instance.GetPlayerManager();
            for (int i = 0; i < PlanningChestList.Count; i++)
            {
                var chest = PlanningChestList[i];
                if (_playerManager.GetTeam() == Team.Defender)
                {
                    chest.NameText.text = ItemHelper.GetName(ItemManager.Instance.AvailableDefenderChest[i].GetItemType());
                    chest.DescriptionText.text = ItemHelper.GetDescription(ItemManager.Instance.AvailableDefenderChest[i].GetItemType());
                    chest.Cost = ItemHelper.GetCost(ItemManager.Instance.AvailableDefenderChest[i].GetItemType());
                    chest.CostText.text = chest.Cost.ToString();
                    chest.Texture.sprite = ItemManager.Instance.DefenderChestSpriteList[i];
                }
            
                if (_playerManager.GetTeam() == Team.Hacker)
                {
                    chest.NameText.text = ItemHelper.GetName(ItemManager.Instance.AvailableHackerChest[i].GetItemType());
                    chest.DescriptionText.text = ItemHelper.GetDescription(ItemManager.Instance.AvailableHackerChest[i].GetItemType());
                    chest.Cost = ItemHelper.GetCost(ItemManager.Instance.AvailableHackerChest[i].GetItemType());
                    chest.CostText.text = chest.Cost.ToString();
                    chest.Texture.sprite = ItemManager.Instance.HackerChestSpriteList[i];
                } 
            }
                
            for (int i = 0; i < PlanningToolList.Count; i++)
            {
                var chest = PlanningToolList[i];

                if (_playerManager.GetTeam() == Team.Defender)
                {
                    chest.NameText.text = ItemHelper.GetName(ItemManager.Instance.AvailableDefenderTools[i].GetItemType());
                    chest.DescriptionText.text = ItemHelper.GetDescription(ItemManager.Instance.AvailableDefenderTools[i].GetItemType());
                    chest.Cost = ItemHelper.GetCost(ItemManager.Instance.AvailableDefenderTools[i].GetItemType());
                    chest.CostText.text = chest.Cost.ToString();
                    chest.Texture.sprite = ItemManager.Instance.DefenderActionToolSpriteList[i]; 
                }
            
                if (_playerManager.GetTeam() == Team.Hacker)
                {
                    chest.NameText.text = ItemHelper.GetName(ItemManager.Instance.AvailableHackerTools[i].GetItemType());
                    chest.DescriptionText.text = ItemHelper.GetDescription(ItemManager.Instance.AvailableHackerTools[i].GetItemType());
                    chest.Cost = ItemHelper.GetCost(ItemManager.Instance.AvailableHackerTools[i].GetItemType());
                    chest.CostText.text = chest.Cost.ToString();
                    chest.Texture.sprite = ItemManager.Instance.HackerActionToolSpriteList[i]; 
                } 
            } 

        }

        public void UpdateUsedPoints(int newPoints)
        {
            _usedPoints += newPoints;
            PointsLeftText.text = GetPointsLeft().ToString();
        }
        public int GetPointsLeft()
        {
            if(_playerManager.GetTeam() == Team.Defender)
            {
                return _defenderPoints - _usedPoints;
            }
            if(_playerManager.GetTeam() == Team.Hacker)
            {
                return _hackerPoints - _usedPoints;
            }
            Debug.Log($"Team:: {_playerManager.GetTeam()}");
            return 0;
        }
        
        public void UpdateItemManager()
        {
            foreach (var item in PlanningChestList)
            {
                
            }
            ItemModule.ItemManager.Instance.UpdateItem();
        }
    }
}