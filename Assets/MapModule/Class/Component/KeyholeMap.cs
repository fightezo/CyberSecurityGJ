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
        [SerializeField] private GameObject[] _citizenItemSpawnList;
        [SerializeField] private GameObject[] _citizenSlotList;
        [SerializeField] private GameObject _citizenMap;
        
        [SerializeField] private GameObject _hackerSpawnPoint;
        [SerializeField] private GameObject[] _hackerItemSpawnList;
        [SerializeField] private GameObject[] _hackerSlotList; 
        [SerializeField] private GameObject _hackerMap;

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

        public GameObject[] GetHackerItemSpawnList()
        {
            throw new System.NotImplementedException();
        }

        public GameObject[] GetHackerSlotList()
        {
            throw new System.NotImplementedException();
        }

        public GameObject[] GetCitizenItemSpawnList()
        {
            return _citizenItemSpawnList;
        }

        public GameObject[] GetCitizenSlotList()
        {
            return _citizenSlotList;
        }

        public GameObject GetCitizenMap()
        {
            return _citizenMap;
        }
        

        public GameObject GetHackerMap()
        {
            return _hackerMap;
        }
    }
}