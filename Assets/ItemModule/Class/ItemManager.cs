using System;
using System.Collections;
using System.Collections.Generic;
using GameModule.Class;
using ItemModule.Class;
using Photon.Pun;
using UnityEngine;

namespace ItemModule
{
    public class ItemManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static ItemManager Instance;
        public List<DefenderItem> AvailableDefenderItems = new List<DefenderItem>();
        private List<DefenderItem> _defenderItemPlacementList = new List<DefenderItem>();
        private List<DefenderItem> _defenderItemActionList = new List<DefenderItem>();

        public List<HackerItem> AvailableHackerItems = new List<HackerItem>();
        private List<HackerItem> _hackerItemPlacementList = new List<HackerItem>();
        private List<HackerItem> _hackerItemActionList = new List<HackerItem>();

        private float _currentTimeToCreateItem = 0f;
        private float _timeToCreateItem = 30f;
        public void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            //set item timers
            if (_currentTimeToCreateItem >= _timeToCreateItem)
            {
                _currentTimeToCreateItem = 0;
                //CreateRandomly
            }
            else
            {
                _currentTimeToCreateItem += Time.deltaTime;
            }
        }

        public void UpdateSecuritySlider()
        {
            var _newLevel = 0;
            GameManager.Instance.UpdateSecurityLevel(_newLevel);
        }
        
        
        #region IPunObservable Implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
        }

        #endregion
    }
}