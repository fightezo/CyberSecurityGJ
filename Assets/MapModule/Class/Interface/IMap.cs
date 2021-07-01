using UnityEngine;

namespace MapModule.Class.Interface
{
    public interface IMap
    {
        // Citizen
        string GetResourcesName();
        Vector3 GetCitizenSpawnPointWorldPosition();
        GameObject[] GetCitizenItemSpawnList();
        GameObject[] GetCitizenSlotList();
        GameObject GetCitizenMap();

        //Hacker        
        Vector3 GetHackerSpawnPointWorldPosition();
        GameObject[] GetHackerItemSpawnList();
        GameObject[] GetHackerSlotList();
        GameObject GetHackerMap();
    }
}