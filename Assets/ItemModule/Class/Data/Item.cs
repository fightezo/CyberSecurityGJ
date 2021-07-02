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

        public abstract void SetItemState(ItemState newState);

        public abstract ItemCategory GetItemCategory();

        public abstract ItemOwner GetItemOwner();

        public abstract int GetCost();
    }
}