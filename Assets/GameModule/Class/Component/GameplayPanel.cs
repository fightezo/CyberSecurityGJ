using System.Collections;
using System.Collections.Generic;
using GameModule.Class.Component.UI;
using GameModule.Class.Interface;
using ItemModule.Class;
using ItemModule.Class.Data;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameModule.Class.Component
{
    internal class GameplayPanel : MonoBehaviour, IUIPanel
    {
        public Text CountDownText;
        public Slider SecurityLevelSlider;
        public GameManager.GameState _currentGameState;

        public List<UIGameplayChest> UIGameplayChestList;
        public List<UIGameplayTool> UIGameplayToolList;

        private PlayerManager _playerManager;
        public GameObject GetSelf()
        {
            return gameObject;
        }

        public void UpdateView(GameManager.GameState gameState, int defenderSecurityMax, int hackerSecurityMax, int securityLevel)
        {
            _currentGameState = gameState;
            SecurityLevelSlider.minValue = hackerSecurityMax;
            SecurityLevelSlider.maxValue = defenderSecurityMax;
            SecurityLevelSlider.value = securityLevel;
            _playerManager = GameManager.Instance.GetPlayerManager();
            var planningList = ItemManager.Instance.GetPlacementList(_playerManager.GetTeam());
            for (int i = 0; i < planningList.Length; i++)
            {
                var item = planningList[i];
                var itemCategory = ItemHelper.GetItemCategory(item.GetItemType());
                var itemOwner = ItemHelper.GetItemOwner(item.GetItemType());

                if (itemCategory == ItemCategory.Placement)
                {
                    UIGameplayChestList[i].Item = item;
                    UIGameplayChestList[i].Name.text = ItemHelper.GetName(item.GetItemType());
                    UIGameplayChestList[i].Image.sprite = itemOwner == Team.Defender
                        ? ItemManager.Instance.DefenderChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())]
                        : ItemManager.Instance.HackerChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())];
                }

                if (itemCategory == ItemCategory.Tool)
                {
                    UIGameplayToolList[i].Item = item;
                    UIGameplayToolList[i].Description.text = ItemHelper.GetDescription(item.GetItemType());
                    UIGameplayToolList[i].Image.sprite = itemOwner == Team.Defender
                        ? ItemManager.Instance.DefenderChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())]
                        : ItemManager.Instance.HackerChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())]; 
                }
            }
            // _securityLevelSlider.onValueChanged.AddListener(UpdateSecurityLevel);
        }
        
        public void EndPhaseUpdate()
        {
            if (_currentGameState == GameManager.GameState.Preparation)
            {
                
            }

            if (_currentGameState == GameManager.GameState.Battle)
            {
                
            }
        }

    }
}