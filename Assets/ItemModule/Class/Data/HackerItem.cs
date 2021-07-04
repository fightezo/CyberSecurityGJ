using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace ItemModule.Class
{
    public abstract class HackerItem : Item
    {
        public GameObject CountDownGameObject;
        public Text TimeLeftText;
        // public override void SetItemState(ItemState newState)
        // {
        //     _currentState = newState;
        // }
        // public override ItemOwner GetItemOwner()
        // {
        //     return ItemOwner.Hacker;
        // }
    }
}