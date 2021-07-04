using ItemModule.Class.Data;

namespace ItemModule.Class.Component
{
    public class NumericChest : Chest
    {
        public override void Create()
        {
           SetRestrictedCharacters("");
           SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.NumericChest;
        }

        public override int GetPasswordLength()
        {
            return 10;
        }
    }
}