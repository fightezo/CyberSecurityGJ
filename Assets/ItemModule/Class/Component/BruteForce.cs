using System.Collections;
using System.Collections.Generic;
using ItemModule.Class.Data;
using UnityEngine;

namespace ItemModule.Class
{

    public class BruteForce : HackerItem
    {
        public override void Create()
        {
            SetItemState(ItemState.World);
        }

        public override ItemType GetItemType()
        {
            return ItemType.BruteForce;
        }

        public override void SetItemState(ItemState newState)
        {
            _currentState = newState;
        }

        public override ItemCategory GetItemCategory()
        {
            return ItemCategory.Placement;
        }

        public override int GetCost()
        {
            return 1;
        }
    }
}