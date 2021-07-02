using UnityEngine;

namespace MapModule.Class.Interface
{
    public interface IMap
    {
        // Citizen
        string GetResourcesName();
        Vector3 GetDefenderSpawnPointWorldPosition();
        GameObject[] GetDefenderItemSpawnList();
        GameObject[] GetDefenderSlotList();
        GameObject GetDefenderMap();

        //Hacker        
        Vector3 GetHackerSpawnPointWorldPosition();
        GameObject[] GetHackerItemSpawnList();
        GameObject[] GetHackerSlotList();
        GameObject GetHackerMap();
    }
}