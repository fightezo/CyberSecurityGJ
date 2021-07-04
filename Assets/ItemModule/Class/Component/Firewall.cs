using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;
using PlayerModule.Class;

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
        // freeze hacker

        [PunRPC]
        private void RPC_ActivateSkill()
        {
            
        }
    }
}