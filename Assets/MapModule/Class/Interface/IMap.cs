using UnityEngine;

namespace MapModule.Class.Interface
{
    public interface IMap
    {
        // Citizen
        string GetResourcesName();
        Vector3 GetDefenderSpawnPointWorldPosition();
        Collider[] GetDefenderItemSpawnList();
        Collider[] GetDefenderSlotList();
        GameObject GetDefenderMap();

        //Hacker        
        Vector3 GetHackerSpawnPointWorldPosition();
        Collider[] GetHackerItemSpawnList();
        Collider[] GetHackerSlotList();
        GameObject GetHackerMap();
    }
}