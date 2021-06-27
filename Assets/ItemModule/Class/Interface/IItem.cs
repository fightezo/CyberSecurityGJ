using ItemModule.Class.Data;

namespace ItemModule.Class.Interface
{
    public interface IItem
    {
        public ItemType GetItemType();
        public ItemState GetItemState();
    }
}