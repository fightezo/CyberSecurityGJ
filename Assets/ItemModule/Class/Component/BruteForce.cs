using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;

namespace ItemModule.Class.Component
{
    public class BruteForce : HackerItem
    {
        public override void Create()
        {
            SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.BruteForce;
        }

        // slowly displays full password
        [PunRPC]
        private void RPC_ActivateSkill()
        {
        }
    }
}