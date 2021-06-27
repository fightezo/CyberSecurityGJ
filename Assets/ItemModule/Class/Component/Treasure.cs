using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using Photon.Pun;
using UnityEngine;

namespace ItemModule.Class.Component
{
    public class Treasure : MonoBehaviourPun, IItem
    {
        public ItemType GetItemType()
        {
            return ItemType.Treasure;
        }

        public ItemState GetItemState()
        {
            return ItemState.World;
        }
        
    }
}