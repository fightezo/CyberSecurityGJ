using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;
using UnityEngine;

namespace ItemModule.Class.Component
{
    public class AntiVirus : DefenderItem
    {
        public override void Create()
        {
           SetItemState(ItemState.Player);
        }
        public override ItemType GetItemType()
        {
            return ItemType.AntiVirus;
        }
        
        // alerts player when chest is being hacked
        [PunRPC]
        private void RPC_ActivateSkill()
        {
            // GameObject
            // GameManager.Instance.SendAttacker();
        }
    }
}