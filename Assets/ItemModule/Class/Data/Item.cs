using System.Collections;
using System.Collections.Generic;
using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using Photon.Pun;
using UnityEngine;

namespace ItemModule.Class
{

    public abstract class Item : MonoBehaviourPun, IItem
    {
        protected ItemState _currentState;
        protected ItemType _itemType;
        protected ItemCategory _itemCategory;
        public abstract void Create();


        public abstract ItemType GetItemType();

        public ItemState GetItemState()
        {
            return _currentState;
        }
        public void SetItemState(ItemState newState)
        {
            _currentState = newState;
        }
        // public abstract void SetItemState(ItemState newState);

        // public abstract ItemCategory GetItemCategory();

        // public abstract ItemOwner GetItemOwner();
        public void ActivateSkill()
        {
            photonView.RPC("RPC_ActivateSkill", RpcTarget.All);
        }
        public void DeactivateSkill()
        {
            photonView.RPC("RPC_DeactivateSkill", RpcTarget.All);
        }
    }
}