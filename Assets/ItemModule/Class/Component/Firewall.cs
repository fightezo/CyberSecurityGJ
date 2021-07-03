using ItemModule.Class.Data;

namespace ItemModule.Class.Component
{
    public class Firewall : DefenderItem
    {
        public override void Create()
        {
           SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.Firewall;
        }

    }
}