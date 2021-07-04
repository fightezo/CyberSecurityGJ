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

        public Text MinText;
        public Text CurrentText;
        public Text MaxText;

        public GameObject NoActionTeleport;
        public GameObject NoActionDestroy;
        public GameObject[] NoItemToolList;
        public GameObject[] NotInRangeList;
        public GameObject[] KeyCapList;
        
        public GameObject GetSelf()
        {
            return gameObject;
        }

        public void UpdateView(GameManager.GameState gameState, int defenderSecurityMax, int hackerSecurityMax,
            int securityLevel)
        {
            _currentGameState = gameState;
            SecurityLevelSlider.minValue = hackerSecurityMax;
            SecurityLevelSlider.maxValue = defenderSecurityMax;
            SecurityLevelSlider.value = securityLevel;
            MinText.text = SecurityLevelSlider.minValue.ToString();
            MaxText.text = SecurityLevelSlider.maxValue.ToString();
            _playerManager = GameManager.Instance.GetPlayerManager();
            UpdateUIView();
            SecurityLevelSlider.onValueChanged.AddListener(GameManager.Instance.UpdateGame);
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

        #region Public Button Methods

        public void OnTeleportButtonClicked()
        {

        }

        public void OnDestroyButtonClicked()
        {

        }

        #endregion

        public void UpdateUIView()
        {
            CurrentText.text = SecurityLevelSlider.value.ToString();
            NoActionTeleport.SetActive(!GameManager.Instance.IsReadyToTeleport(_playerManager.GetTeam()));
            NoActionDestroy.SetActive(!_playerManager.GetIsPlayerInRange());
            _UpdateChests();
            _UpdateTools();
        }

        private void _UpdateTools()
        {
            var toolList = ItemManager.Instance.GeToolList(_playerManager.GetTeam());
            for (int i = 0; i < toolList.Length; i++)
            {
                UIGameplayToolList[i].gameObject.SetActive(false);
                NoItemToolList[i].SetActive(true);
                NotInRangeList[i].SetActive(false);
                var item = toolList[i];
                if (item == null) continue;
                var itemCategory = ItemHelper.GetItemCategory(item.GetItemType());
                var itemOwner = ItemHelper.GetItemOwner(item.GetItemType());

                if (itemCategory == ItemCategory.Tool)
                {
                    UIGameplayToolList[i].Item = item;
                    UIGameplayToolList[i].Description.text = ItemHelper.GetDescription(item.GetItemType());
                    UIGameplayToolList[i].Image.sprite = itemOwner == Team.Defender
                        ? ItemManager.Instance.DefenderChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())]
                        : ItemManager.Instance.HackerChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())];
                   
                    UIGameplayToolList[i].gameObject.SetActive(true);
                    NoItemToolList[i].SetActive(false);
                    if (_playerManager.ItemInRange() != null)
                    {
                        NotInRangeList[i].SetActive(false);
                    }
                }
            }


        }

        private void _UpdateChests()
        {
            ItemManager.Instance.GetPlacementList(_playerManager.GetTeam());
            var planningList = ItemManager.Instance.GetPlacementList(_playerManager.GetTeam());
            for (int i = 0; i < planningList.Length; i++)
            {
                UIGameplayChestList[i].gameObject.SetActive(false);
                KeyCapList[i].SetActive(false);
                var item = planningList[i];
                if (item == null) continue;
                var itemCategory = ItemHelper.GetItemCategory(item.GetItemType());
                var itemOwner = ItemHelper.GetItemOwner(item.GetItemType());

                if (itemCategory == ItemCategory.Placement)
                {
                    UIGameplayChestList[i].Item = item;
                    UIGameplayChestList[i].Name.text = ItemHelper.GetName(item.GetItemType());
                    UIGameplayChestList[i].Image.sprite = itemOwner == Team.Defender
                        ? ItemManager.Instance.DefenderChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())]
                        : ItemManager.Instance.HackerChestSpriteList[ItemHelper.GetSpriteIndex(item.GetItemType())];
                    UIGameplayChestList[i].gameObject.SetActive(true);
                    if (_playerManager.GetIsSlotPointInRange() != null)
                    {
                        KeyCapList[i].SetActive(true);
                    }  

                }
            }
        }
    }
}