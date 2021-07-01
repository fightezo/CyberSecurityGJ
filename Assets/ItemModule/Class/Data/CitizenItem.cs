using ItemModule.Class.Data;
using ItemModule.Class.Interface;
using Photon.Pun;

namespace ItemModule.Class
{
    public abstract class CitizenItem : MonoBehaviourPun, IItem
    {
        public void Create()
        {
        }

        public abstract ItemType GetItemType();
        public abstract ItemState GetItemState();
        public abstract void SetState(ItemState newState);
        public abstract ItemCategory GetItemCategory();

        public ItemOwner GetItemOwner()
        {
            return ItemOwner.Citizen;
        }
    }
}