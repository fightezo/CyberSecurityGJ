using System;
using System.Collections.Generic;
using System.Linq;
using GameModule.Class;
using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using MapModule.Class;
using Photon.Pun;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;

namespace ItemModule.Class
{
    public class ItemManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static ItemManager Instance;

        public GameObject MiniGameCanvas;
        
        private int _maxPlacementItems = 5;
        private int _maxToolItems = 3;
        
        public GameObject HasGetItemPlaceholder;
        public List<DefenderItem> AvailableDefenderChest = new List<DefenderItem>();
        public List<DefenderItem> AvailableDefenderTools = new List<DefenderItem>();
        [SerializeField] private Item[] _defenderItemPlacementList;
        [SerializeField] private Item[] _defenderItemToolList;
        // private List<DefenderItem> _defenderItemPlacementList = new List<DefenderItem>();
        // private List<DefenderItem> _defenderItemToolList = new List<DefenderItem>();
        public List<Sprite> DefenderChestSpriteList;
        public List<Sprite> DefenderActionToolSpriteList;


        public List<HackerItem> AvailableHackerChest = new List<HackerItem>();
        public List<HackerItem> AvailableHackerTools = new List<HackerItem>();
        [SerializeField] private Item[] _hackerItemPlacementList;
        [SerializeField] private Item[] _hackerItemToolList;

        // private List<HackerItem> _hackerItemPlacementList = new List<HackerItem>();
        // private List<HackerItem> _hackerItemToolList = new List<HackerItem>();
        public List<Sprite> HackerChestSpriteList;
        public List<Sprite> HackerActionToolSpriteList;

        private float _currentTimeToCreateItem = 0f;
        private float _timeToCreateItem = 30f;

        private float _currentTimeToUpdateSecurityLevel = 0f;
        private float _timeToUpdateSecurityLevel = 30f;
        private PlayerManager _playerManager;

 
        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            _defenderItemPlacementList = new DefenderItem[_maxPlacementItems];
            _defenderItemToolList = new DefenderItem[_maxToolItems];
            _hackerItemPlacementList = new HackerItem[_maxPlacementItems];
            _hackerItemToolList = new HackerItem[_maxToolItems];
        }

        public void Update()
        {
            if (GameManager.Instance.GetGameState() != GameManager.GameState.Battle)
                return;
            if (_playerManager == null)
                _playerManager = GameManager.Instance.GetPlayerManager();

            //set item timers
            if (_currentTimeToCreateItem >= _timeToCreateItem)
            {
                _currentTimeToCreateItem = 0;
                
                if (_playerManager.GetTeam() == Team.Defender)
                {
                    _EnableDefenderItemSpawnPointsRandomly(true);
                    _EnableDefenderItemSpawnPointsRandomly(false);
                }

                if (_playerManager.GetTeam() == Team.Hacker)
                {
                    _EnableHackerItemSpawnPointsRandomly(true);
                    _EnableHackerItemSpawnPointsRandomly(false);
                }
            }
            else
            {
                _currentTimeToCreateItem += Time.deltaTime;
            }
            
            //Update based on # of Defender Chest
            if (_currentTimeToUpdateSecurityLevel >= _timeToUpdateSecurityLevel)
            {
                var newSecurityLevelChange = ItemHelper.GetSecurityLevelPoints(GetDefendingSecurityLevel());
                _UpdateSecuritySlider(newSecurityLevelChange);
                _currentTimeToUpdateSecurityLevel = 0;
            }
            else
            {
                _currentTimeToUpdateSecurityLevel += Time.deltaTime;
            }
            
        }

        private int GetDefendingSecurityLevel()
        {
            var count = 0;
            foreach (var item in _defenderItemPlacementList)
            {
                if(item == null) continue;
                if (item.GetItemState() == ItemState.Active)
                    count++;
            }

            return count;
        }

        #region Public Methods
        // Preparation Phase
        public Item[] GetPlacementList(Team team)
        {
            if(team == Team.Defender) 
                return _defenderItemPlacementList;
            if (team == Team.Hacker)
                return _hackerItemPlacementList;
            return new Item[]{};
        }
        public Item[] GeToolList(Team team)
        {
            if(team == Team.Defender) 
                return _defenderItemToolList;
            if (team == Team.Hacker)
                return _hackerItemToolList;
            return new Item[]{};
        }

        // Gameplay Phase
        public void PlaceItemOnSlotPoint(int index, Collider slotCollider)
        {
            Item item = null;
            if (_playerManager.GetTeam() == Team.Defender)
            {
                item = _defenderItemPlacementList[index];
                item.transform.position = slotCollider.gameObject.transform.position;
            }
            if (_playerManager.GetTeam() == Team.Hacker)
            {
                item = _hackerItemPlacementList[index];
                _hackerItemPlacementList[index].transform.position = slotCollider.gameObject.transform.position;
 
            }
            // if has opposing item activate; else wait
            if(item != null)
                item.ActivateSkill();
            GameManager.Instance.UpdateGameplayUIView();

            Debug.Log($"");
        }
        
        public void UseTool(int index)
        {
            Item item = null;
            if (_playerManager.GetTeam() == Team.Defender)
            {
                if (_defenderItemToolList[index] == null) return;
                item = _defenderItemToolList[index];
            }
            if (_playerManager.GetTeam() == Team.Hacker)
            {
                if (_hackerItemToolList[index] == null) return;
                item = _hackerItemToolList[index];
            }

            if(item != null)
                item.ActivateSkill(); 
            // ActivateItemAbility(itemType);
            GameManager.Instance.UpdateGameplayUIView();

        }
        #endregion
        private void _EnableHackerItemSpawnPointsRandomly(bool isOn)
        {
            Debug.Log($"{name}::EnableHackerItem::{isOn}");
            var randomItemIndex = UnityEngine.Random.Range(0, AvailableHackerChest.Count);
            var hackerItemSpawnPoints = MapManager.Instance.GetHackerItemSpawnPoints();
            var randomItemPosition = UnityEngine.Random.Range(0, hackerItemSpawnPoints.Count);
            hackerItemSpawnPoints[randomItemPosition].enabled = isOn;
            // var item = PhotonNetwork.Instantiate(HasGetItemPlaceholder.name, hackerItemSpawnPoints[randomItemPosition].transform.position, Quaternion.identity);

            // var item = PhotonNetwork.Instantiate(AvailableHackerItems[randomItemIndex].name, hackerItemSpawnPoints[randomItemPosition].transform.position, 
            // Quaternion.identity);
        }
        private void _EnableDefenderItemSpawnPointsRandomly(bool isOn)
        {
            Debug.Log($"{name}::EnableDefenderItem::{isOn}");
            var randomItemIndex = UnityEngine.Random.Range(0, AvailableDefenderChest.Count);
            var defenderItemSpawnPoints = MapManager.Instance.GetDefenderItemSpawnPoints();
            var randomItemPosition = UnityEngine.Random.Range(0, defenderItemSpawnPoints.Count);
            defenderItemSpawnPoints[randomItemPosition].enabled = isOn;

            // var item = PhotonNetwork.Instantiate(HasGetItemPlaceholder.name, defenderItemSpawnPoints[randomItemPosition].transform.position, Quaternion.identity);
            // var item = PhotonNetwork.Instantiate(AvailableDefenderItems[randomItemIndex].name, defenderItemSpawnPoints[randomItemPosition].transform.position, 
            // Quaternion.identity);
        }

        private void _UpdateSecuritySlider(int newSecurityLevelChange)
        {
            GameManager.Instance.photonView.RPC("RPC_UpdateSecurityLevel", RpcTarget.AllBuffered, newSecurityLevelChange);
        }

        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion

        #region PUNRPC Methods

        [PunRPC]
        private void RPC_PlaceItemOnSlotPoint(int data)
        {
            
        }
        [PunRPC]
        private void RPC_UpdateSceneItem(int[] data)
        {
            Debug.Log($"RPC_UpdateSceneItem::{string.Join("|", data)}");
            _UpdateSceneItem(data.ToList());
        }

        #endregion

        public void PopupMiniGameUI()
        {
            MiniGameCanvas.SetActive(true);
        }

        public void DismissMiniGameUI()
        {
            MiniGameCanvas.SetActive(false);
        }

        #region OnButtonClicks Methods

        public void OnExitMiniGameClicked()
        {
            DismissMiniGameUI();
        }

        #endregion

        private void _UpdateSceneItem(List<int> itemTypeList)
        {
            foreach (var itemTypeInt in itemTypeList)
            {
                var itemType = (ItemType)itemTypeInt;
                var resourcesName = ItemHelper.GetResourcesName(itemType);
                var itemOwner = ItemHelper.GetItemOwner(itemType);
                var itemCategory = ItemHelper.GetItemCategory(itemType);
                var newItem = PhotonNetwork.Instantiate(resourcesName, GameHelper.GetResourceArea(), Quaternion.identity);
                if (itemOwner == Team.Defender)
                {
                    var dItem = newItem.GetComponent<DefenderItem>();
                    if (itemCategory == ItemCategory.Placement)
                    {
                        var emptyIndex = Array.IndexOf(_defenderItemPlacementList, null, 0);
                        if (emptyIndex != -1)
                        {
                            _defenderItemPlacementList[emptyIndex] = dItem;
                        }
                    }

                    if (itemCategory == ItemCategory.Tool)
                    {
                        var emptyIndex = Array.IndexOf(_defenderItemToolList, null, 0);
                        if (emptyIndex != -1)
                        {
                            _defenderItemToolList[emptyIndex] = dItem;
                        }
                    }
                }

                if (itemOwner == Team.Hacker)
                {
                    var hItem = newItem.GetComponent<HackerItem>();

                    if (itemCategory == ItemCategory.Placement)
                    {
                        var emptyIndex = Array.IndexOf(_hackerItemPlacementList, null, 0);
                        if (emptyIndex != -1)
                        {
                            _hackerItemPlacementList[emptyIndex] = hItem;
                        }     
                    }

                    if (itemCategory == ItemCategory.Tool)
                    {
                        var emptyIndex = Array.IndexOf(_hackerItemToolList, null, 0);
                        if (emptyIndex != -1)
                        {
                            _hackerItemToolList[emptyIndex] = hItem;
                        }
                    }
                }
            }
        }
        // TODO item UI; change password; new password; hack password
        public void PopupItemUI()
        {
            throw new NotImplementedException();
        }
    }
}