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
        private Vector3 _translationToCitizenMap;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentMapGameObject = Instantiate((GameObject) Resources.Load(MapList[0].GetComponent<IMap>().GetResourcesName()), Vector3.zero, Quaternion.identity);
            _currentMap = _currentMapGameObject.GetComponent<IMap>();

            _translationToCitizenMap = GetCitizenMap().transform.position - GetHackerMap().transform.position;
            _translationToHackerMap = GetHackerMap().transform.position - GetCitizenMap().transform.position;
        }

        public GameObject GetCitizenMap()
        {
            return _currentMap.GetCitizenMap();
        }

        public GameObject GetHackerMap()
        {
            return _currentMap.GetHackerMap();
        }
         public Vector3 GetCitizenSpawnPoint()
         {
             return _currentMap.GetCitizenSpawnPointWorldPosition();
         }
         public Vector3 GetHackerSpawnPoint()
        {
            return _currentMap.GetHackerSpawnPointWorldPosition();
        }

         public Vector3 GetTranslateToCitizenMap()
         {
             return _translationToCitizenMap;
         }

         public Vector3 GetTranslationToHackerMap()
         {
             return _translationToHackerMap;
         }

         public List<GameObject> GetCitizenItemSpawnPoints()
         {
             return _currentMap.GetCitizenItemSpawnList().ToList();
         }
         
         public List<GameObject> GetCitizenSlotPoints()
         {
             return _currentMap.GetCitizenSlotList().ToList();
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