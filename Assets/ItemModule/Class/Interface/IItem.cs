using ItemModule.Class.Data;

namespace ItemModule.Class.Interface
{
    public interface IItem
    {
        public void Create();
        public ItemType GetItemType();
        public ItemState GetItemState();
        public void SetItemState(ItemState newState);
        public ItemCategory GetItemCategory();
        public ItemOwner GetItemOwner();

        public int GetCost();
        
    }
}