using System.Collections;
using System.Collections.Generic;
using MapModule.Class.Interface;
using UnityEngine;
namespace MapModule
{
    // Sleutelgat :: Keyhole in Dutch
    public class KeyholeMap : MonoBehaviour, IMap
    {
        [SerializeField] private GameObject _citizenSpawnPoint;
        [SerializeField] private GameObject _hackerSpawnPoint;
        [SerializeField] private GameObject[] _itemSpawnList;
        [SerializeField] private GameObject[] _slotPositionList;
        public string GetResourcesName()
        {
            return gameObject.name;
        }

        public Vector3 GetCitizenSpawnPointWorldPosition()
        {
            return _citizenSpawnPoint.transform.position;
        }

        public Vector3 GetHackerSpawnPointWorldPosition()
        {
            return _hackerSpawnPoint.transform.position;
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