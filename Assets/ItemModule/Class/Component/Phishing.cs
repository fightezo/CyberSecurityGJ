using GameModule.Class;
using ItemModule.Class.Data;
using Photon.Pun;

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
// creates a fake UI that “simulates” original UI
// defender can dismiss the fake UI if they notice it
// if defender is tricked by the fake UI
// show password?

        [PunRPC]
        private void RPC_ActivateSkill()
        {
            GameManager.Instance.SendAttacker();
        }
    }
}