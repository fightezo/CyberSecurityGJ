using System.Collections;
using System.Collections.Generic;
using MapModule.Class.Interface;
using UnityEngine;

namespace MapModule
{
    // Sleutelgat :: Keyhole in Dutch
    public class KeyholeMap : MonoBehaviour, IMap
    {
        [SerializeField] private GameObject _defenderSpawnPoint;
        [SerializeField] private GameObject _defenderMap;
        [SerializeField] private Collider[] _defenderItemTriggerAreaList;
        [SerializeField] private Collider[] _defenderSlotList;

        [SerializeField] private GameObject _hackerSpawnPoint;
        [SerializeField] private GameObject _hackerMap;
        [SerializeField] private Collider[] _hackerItemTriggerAreaList;
        [SerializeField] private Collider[] _hackerSlotList;

        public string GetResourcesName()
        {
            return gameObject.name;
        }

        #region Defender Methods

        public Vector3 GetDefenderSpawnPointWorldPosition()
        {
            return _defenderSpawnPoint.transform.position;
        }

        public Collider[] GetDefenderItemSpawnList()
        {
            return _defenderItemTriggerAreaList;
        }

        public Collider[] GetDefenderSlotList()
        {
            return _defenderSlotList;
        }

        public GameObject GetDefenderMap()
        {
            return _defenderMap;
        }

        #endregion

        #region Hacker Methods

        public Vector3 GetHackerSpawnPointWorldPosition()
        {
            return _hackerSpawnPoint.transform.position;
        }

        public Collider[] GetHackerItemSpawnList()
        {
            return _hackerItemTriggerAreaList;
        }

        public Collider[] GetHackerSlotList()
        {
            return _hackerSlotList;
        }

        public GameObject GetHackerMap()
        {
            return _hackerMap;
        }

        #endregion
    }
}