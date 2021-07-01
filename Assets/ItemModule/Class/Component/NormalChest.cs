using ItemModule.Class.Data;
using Photon.Pun;
using UnityEngine;

namespace ItemModule.Class.Component
{
    public class NormalChest : Chest
    {
        public override ItemType GetItemType()
        {
            return ItemType.NormalChest;
        }

    }
}