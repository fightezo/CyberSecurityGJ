using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;

namespace ItemModule.Class.Component
{

    public class Ransomware : HackerItem
    {
        public override void Create()
        {
            SetItemState(ItemState.Player);
        }

        public override ItemType GetItemType()
        {
            return ItemType.Ransomware;
        }

        // freeze defender
        [PunRPC]
        private void RPC_ActivateSkill()
        {
        }
    }
}