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
        private List<CitizenItem> _citizenItemPlacementList = new List<CitizenItem>();
        private List<CitizenItem> _citizenItemActionList = new List<CitizenItem>();

        private List<HackerItem> _hackerItemPlacementList = new List<HackerItem>();
        private List<HackerItem> _hackerItemActionList = new List<HackerItem>();

        private float _currentTimeToCreateItem = 0f;
        private float _TimeToCreateItem = 30f;
        public void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            //set item timers
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