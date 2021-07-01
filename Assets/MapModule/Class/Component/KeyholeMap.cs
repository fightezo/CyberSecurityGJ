using System.Collections;
using System.Collections.Generic;
using MapModule.Class.Interface;
using UnityEngine;
namespace MapModule
{
    // Sleutelgat :: Keyhole in Dutch
    public class KeyholeMap : MonoBehaviour, IMap
    {
        [SerializeField] private GameObject _spawnPoint;
        [SerializeField] private GameObject[] _itemSpawnList;
        [SerializeField] private GameObject[] _slotPositionList;
        public string GetResourcesName()
        {
            return gameObject.name;
        }

        public Vector3 GetSpawnPointWorldPosition()
        {
            return _spawnPoint.transform.position;
        }

        public GameObject[] GetItemSpawnList()
        {
            return _itemSpawnList;
        }

        public GameObject[] GetSlotList()
        {
            return _slotPositionList;
        }

    }
}