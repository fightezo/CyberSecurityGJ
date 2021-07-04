using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;

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

        public override int GetPasswordLength()
        {
        return 15;
        }
    }
}