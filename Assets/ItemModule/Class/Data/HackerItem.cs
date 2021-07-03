using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using Photon.Pun;

namespace ItemModule.Class
{
    public abstract class HackerItem : Item
    {
        public override void SetItemState(ItemState newState)
        {
            _currentState = newState;
        }
        public override ItemOwner GetItemOwner()
        {
            return ItemOwner.Hacker;
        }
    }
}