using UnityEngine;

namespace MapModule.Class.Interface
{
    public interface IMap
    {
        string GetResourcesName();
        Vector3 GetCitizenSpawnPointWorldPosition();
        Vector3 GetHackerSpawnPointWorldPosition();
        GameObject[] GetItemSpawnList();
        GameObject[] GetSlotList();
    }
}