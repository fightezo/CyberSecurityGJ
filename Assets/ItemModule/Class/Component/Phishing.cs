using ItemModule.Class.Data;

namespace ItemModule.Class.Component
{

    public class Phishing : HackerItem
    {
        public override void Create()
        {
            SetItemState(ItemState.Player);
        }

        public override ItemType GetItemType()
        {
            return ItemType.Phishing;
        }

    }
}