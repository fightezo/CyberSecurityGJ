using ItemModule.Class.Data;

namespace ItemModule.Class.Interface
{
    public interface IItem
    {
        public void Create();
        public ItemType GetItemType();
        
        public ItemState GetItemState();
        public void SetState(ItemState newState);
        
        public ItemCategory GetItemCategory();
        public ItemOwner GetItemOwner();
    }
}