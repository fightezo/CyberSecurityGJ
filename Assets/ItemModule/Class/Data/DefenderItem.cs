using UnityEngine;

namespace ItemModule.Class.Data
{
    public abstract class DefenderItem : Item
    {
        public GameObject BeingAttackedGameObject;

        // public override void SetItemState(ItemState newState)
        // {
        //     _currentState = newState;
        // }
        // public override ItemOwner GetItemOwner()
        // {
        //     return ItemOwner.Defender;
        // }
    }
}