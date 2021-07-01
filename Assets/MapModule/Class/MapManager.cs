using System;
using System.Collections.Generic;
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
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _currentMapGameObject = (GameObject) Resources.Load(MapList[0].GetComponent<IMap>().GetResourcesName());
            _currentMap = _currentMapGameObject.GetComponent<IMap>();
       }

        // Update is called once per frame
        void Update()
        {

        }

        public Vector3 GetSpawnPoint()
        {
            return _currentMap.GetSpawnPointWorldPosition();
        }
    }
}