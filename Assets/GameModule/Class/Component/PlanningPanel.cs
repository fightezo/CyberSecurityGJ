using System;
using System.Collections;
using System.Collections.Generic;
using GameModule.Class.Interface;
using ItemModule;
using ItemModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;
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
        
        // public List<UIPlanningItem> PlanningItemList;
        
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
        public void UpdateView(Team team)
        {
            PointsLeftText.text = (team switch
            {
                Team.Defender => _defenderPoints,
                Team.Hacker => _hackerPoints,
                _ => 0
            }).ToString();
            _playerManager =  GameManager.Instance.GetPlayerManager();
            // for (int i = 0; i < PlanningItemList.Count; i++)
            // {
            //     var chest = PlanningItemList[i];
            //     chest.ItemType = ItemManager.Instance.AvailableDefenderChest[i].GetItemType();
            //
            //     chest.NameText.text = ItemHelper.GetName(chest.ItemType);
            //     chest.DescriptionText.text = ItemHelper.GetDescription(chest.ItemType);
            //     chest.Cost = ItemHelper.GetCost(chest.ItemType);
            //     chest.CostText.text = chest.Cost.ToString();
            //     chest.CurrentCount = 0;
            //     chest.CurrentCountText.text = chest.CurrentCount.ToString(); 
            // }
            for (int i = 0; i < PlanningChestList.Count; i++)
            {
                var chest = PlanningChestList[i];
                if (team == Team.Defender)
                {
                    chest.ItemType = ItemManager.Instance.AvailableDefenderChest[i].GetItemType();
                    chest.Texture.sprite = ItemManager.Instance.DefenderChestSpriteList[i];
                }
                if (team == Team.Hacker)
                {
                    chest.ItemType = ItemManager.Instance.AvailableHackerChest[i].GetItemType();
                    chest.Texture.sprite = ItemManager.Instance.HackerChestSpriteList[i];

                } 
                chest.NameText.text = ItemHelper.GetName(chest.ItemType);
                chest.DescriptionText.text = ItemHelper.GetDescription(chest.ItemType);
                chest.Cost = ItemHelper.GetCost(chest.ItemType);
                chest.CostText.text = chest.Cost.ToString();
                chest.CurrentCount = 0;
                chest.CurrentCountText.text = chest.CurrentCount.ToString();
            }
                
            for (int i = 0; i < PlanningToolList.Count; i++)
            {
                var chest = PlanningToolList[i];

                if (team == Team.Defender)
                {
                    chest.ItemType = ItemManager.Instance.AvailableDefenderTools[i].GetItemType();
                    chest.Texture.sprite = ItemManager.Instance.DefenderActionToolSpriteList[i]; 
                }
            
                if (team == Team.Hacker)
                {
                    chest.ItemType = ItemManager.Instance.AvailableHackerTools[i].GetItemType();
                    chest.Texture.sprite = ItemManager.Instance.HackerActionToolSpriteList[i]; 
                } 
                chest.NameText.text = ItemHelper.GetName(chest.ItemType);
                chest.DescriptionText.text = ItemHelper.GetDescription(chest.ItemType);
                chest.Cost = ItemHelper.GetCost(chest.ItemType);
                chest.CostText.text = chest.Cost.ToString();
                chest.CurrentCount = 0;
                chest.CurrentCountText.text = chest.CurrentCount.ToString();
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
            var itemList = new List<int>();

            foreach (var item in PlanningChestList)
            {
                itemList.Add((int)item.ItemType);
            }

            foreach (var item in PlanningToolList)
            {
                itemList.Add((int)item.ItemType);
            }
            ItemManager.Instance.photonView.RPC("RPC_UpdateSceneItem", RpcTarget.AllBuffered, itemList.ToArray());
        }
    }
}