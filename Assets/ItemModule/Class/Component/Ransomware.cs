using System;
using System.Collections;
using System.Collections.Generic;
using ItemModule.Class.Data;
using UnityEngine;

namespace ItemModule.Class
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

        public override ItemCategory GetItemCategory()
        {
            return ItemCategory.Placement;
        }

    }
}