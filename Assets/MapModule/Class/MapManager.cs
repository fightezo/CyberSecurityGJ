using System;
using System.Collections.Generic;
using System.Linq;
using MapModule.Class.Interface;
using UnityEngine;

namespace MapModule.Class
{
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;
        public List<GameObject> MapList;
        private IMap _currentMap;
        private GameObject _currentMapGameObject;
        private Vector3 _translationToHackerMap;
        private Vector3 _translationToDefenderMap;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentMapGameObject = Instantiate((GameObject) Resources.Load(MapList[0].GetComponent<IMap>().GetResourcesName()), Vector3.zero, Quaternion.identity);
            _currentMap = _currentMapGameObject.GetComponent<IMap>();

            _translationToDefenderMap = GetDefenderMap().transform.position - GetHackerMap().transform.position;
            _translationToHackerMap = GetHackerMap().transform.position - GetDefenderMap().transform.position;
        }

        public GameObject GetDefenderMap()
        {
            return _currentMap.GetDefenderMap();
        }

        public GameObject GetHackerMap()
        {
            return _currentMap.GetHackerMap();
        }
         public Vector3 GetDefenderSpawnPoint()
         {
             return _currentMap.GetDefenderSpawnPointWorldPosition();
         }
         public Vector3 GetHackerSpawnPoint()
        {
            return _currentMap.GetHackerSpawnPointWorldPosition();
        }

         public Vector3 GetTranslateToDefenderMap()
         {
             return _translationToDefenderMap;
         }

         public Vector3 GetTranslationToHackerMap()
         {
             return _translationToHackerMap;
         }

         public List<GameObject> GetDefenderItemSpawnPoints()
         {
             return _currentMap.GetDefenderItemSpawnList().ToList();
         }
         
         public List<GameObject> GetCitizenSlotPoints()
         {
             return _currentMap.GetDefenderSlotList().ToList();
         }
         
         public List<GameObject> GetHackerItemSpawnPoints()
         {
             return _currentMap.GetHackerItemSpawnList().ToList();
         }
         public List<GameObject> GetHackerSlotPoints()
         {
             return _currentMap.GetHackerSlotList().ToList();
         }
         
    }
}