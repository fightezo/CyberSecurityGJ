using System;
using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;
using PlayerModule.Class;
using PlayerModule.Class.Data;
using UnityEngine;

namespace ItemModule.Class.Component
{
    public class Firewall : DefenderItem
    {
        private bool _activated = false;
        private float _currentTime = 0;
        private float _activeTime = 30;

        public override void Create()
        {
           SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.Firewall;
        }
        // freeze hacker
        private void Update()
        {
            if (!_activated) return;
            if (_currentTime >= _activeTime)
            {
                DeactivateSkill();
            }
            else
            {
                _currentTime += Time.deltaTime;
            }
        }

        [PunRPC]
        private void RPC_ActivateSkill()
        {
            PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>().SetIsFrozen(true, Team.Hacker);
            _activated = true;
        }

        [PunRPC]
        private void RPC_DeactivateSkill()
        {
            PlayerManager.LocalPlayerInstance.GetComponent<PlayerManager>().SetIsFrozen(false, Team.Hacker);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}