using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using Photon.Pun;

namespace ItemModule.Class
{
    public abstract class DefenderItem : Item
    {
        public override void SetItemState(ItemState newState)
        {
            _currentState = newState;
        }
        public override ItemOwner GetItemOwner()
        {
            return ItemOwner.Defender;
        }
    }
}