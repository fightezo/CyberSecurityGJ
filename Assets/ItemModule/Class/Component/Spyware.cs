using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;

namespace ItemModule.Class.Component
{

    public class Spyware : HackerItem
    {
        public override void Create()
        {
            SetItemState(ItemState.Player);
        }

        public override ItemType GetItemType()
        {
            return ItemType.Spyware;
        }
        // keylogger; shows password within a limited time
        [PunRPC]
        private void RPC_ActivateSkill()
        {
            GameManager.Instance.SendAttacker();
        }
    }
}