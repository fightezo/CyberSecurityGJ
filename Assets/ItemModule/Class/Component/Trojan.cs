using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;

namespace ItemModule.Class.Component
{

    public class Trojan : HackerItem
    {
        public override void Create()
        {
            SetItemState(ItemState.Player);
        }

        public override ItemType GetItemType()
        {
            return ItemType.Trojan;
        }
        
        //sends attacker helper to attack defender (stuns defender)
        [PunRPC]
        private void RPC_ActivateSkill()
        {
            GameManager.Instance.SendAttacker();
        }
    }
}