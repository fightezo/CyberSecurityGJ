using ItemModule.Class.Data;

namespace ItemModule.Class.Component
{
    public class NormalChest : Chest
    {
        public override void Create()
        {
           SetRestrictedCharacters("");
           SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.NormalChest;
        }

        public override int GetPasswordLength()
        {
            return 20;
        }
    }
}