using ItemModule.Class.Data;

namespace ItemModule.Class.Component
{
    public class AntiVirus : DefenderItem
    {
        public override void Create()
        {
           SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.AntiVirus;
        }
        public override ItemCategory GetItemCategory()
        {
            return ItemCategory.Action;
        }
    }
}