using ItemModule.Class.Data;

namespace ItemModule.Class.Component
{
    public class AlphanumericChest : Chest
    {
        public override void Create()
        {
           SetRestrictedCharacters("");
           SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.AlphanumericChest;
        }
    }
}