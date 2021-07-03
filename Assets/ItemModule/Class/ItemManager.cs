using System;
using System.Collections;
using System.Collections.Generic;
using GameModule.Class;
using ItemModule.Class;
using ItemModule.Class.Interface;
using MapModule.Class;
using Photon.Pun;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;
using Random = System.Random;

namespace ItemModule
{
    public class ItemManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static ItemManager Instance;

        public GameObject MiniGameCanvas;
        
        public GameObject HasGetItemPlaceholder;
        public List<DefenderItem> AvailableDefenderChest = new List<DefenderItem>();
        public List<DefenderItem> AvailableDefenderTools = new List<DefenderItem>();
        private List<DefenderItem> _defenderItemPlacementList = new List<DefenderItem>();
        private List<DefenderItem> _defenderItemActionList = new List<DefenderItem>();
        public List<Sprite> DefenderChestSpriteList;
        public List<Sprite> DefenderActionToolSpriteList;
        

        public List<HackerItem> AvailableHackerChest = new List<HackerItem>();
        public List<HackerItem> AvailableHackerTools = new List<HackerItem>();
        private List<HackerItem> _hackerItemPlacementList = new List<HackerItem>();
        private List<HackerItem> _hackerItemActionList = new List<HackerItem>();
        public List<Sprite> HackerChestSpriteList;
        public List<Sprite> HackerActionToolSpriteList;

        private float _currentTimeToCreateItem = 0f;
        private float _timeToCreateItem = 30f;
        

        private PlayerManager _playerManager;

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
        }

        public void Update()
        {
            if (GameManager.Instance.GetGameState() != GameManager.GameState.Battle)
                return;
            if(_playerManager == null) 
                _playerManager = GameManager.Instance.GetPlayerManager();

            //set item timers
            if (_currentTimeToCreateItem >= _timeToCreateItem)
            {
                _currentTimeToCreateItem = 0;
                //CreateRandomly
                Debug.Log($"{name}::CreateDefenderItem");
                // TODO keep track and 
                if(_playerManager.GetTeam() == Team.Defender)
                    _CreateDefenderItemRandomly();
 
                if(_playerManager.GetTeam() == Team.Hacker)
                    _CreateHackerItemRandomly();
            }
            else
            {
                _currentTimeToCreateItem += Time.deltaTime;
            }
        }
        // Planning Phase

        private void _CreateHackerItemRandomly()
        {
            Debug.Log($"{name}::CreateHackerItem");
            var randomItemIndex = UnityEngine.Random.Range(0, AvailableDefenderChest.Count);
            var hackerItemSpawnPoints = MapManager.Instance.GetHackerItemSpawnPoints();
            var randomItemPosition = UnityEngine.Random.Range(0, hackerItemSpawnPoints.Count);
            var item = PhotonNetwork.Instantiate(HasGetItemPlaceholder.name, hackerItemSpawnPoints[randomItemPosition].transform.position, 
            Quaternion.identity);
            
            // var item = PhotonNetwork.Instantiate(AvailableHackerItems[randomItemIndex].name, hackerItemSpawnPoints[randomItemPosition].transform.position, 
                // Quaternion.identity);
        }

        private void _CreateDefenderItemRandomly()
        {
            Debug.Log($"{name}::CreateDefenderItem");
            var randomItemIndex = UnityEngine.Random.Range(0, AvailableDefenderChest.Count);
            var defenderItemSpawnPoints = MapManager.Instance.GetDefenderItemSpawnPoints();
            var randomItemPosition = UnityEngine.Random.Range(0, defenderItemSpawnPoints.Count);
            var item = PhotonNetwork.Instantiate(HasGetItemPlaceholder.name, defenderItemSpawnPoints[randomItemPosition].transform.position, 
                Quaternion.identity);
            // var item = PhotonNetwork.Instantiate(AvailableDefenderItems[randomItemIndex].name, defenderItemSpawnPoints[randomItemPosition].transform.position, 
                // Quaternion.identity);
        }

        public void UpdateSecuritySlider()
        {
            var _newLevel = 0;
            GameManager.Instance.UpdateSecurityLevel(_newLevel);
        }
        
        // public void 
        
        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion
        
        #region PUNRPC Methods
        #endregion

        public void StartMiniGame()
        {
            MiniGameCanvas.SetActive(true);
        }
        public void EndMiniGame()
        {
            MiniGameCanvas.SetActive(false);
        }

        #region OnButtonClicks Methods

        
        public void OnExitMiniGameClicked()
        {
            EndMiniGame();
        }
        #endregion

        public void UpdateItem()
        {
            
        }
    }
}