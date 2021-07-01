using UnityEngine;

namespace MapModule.Class.Interface
{
    public interface IMap
    {
        string GetResourcesName();
        Vector3 GetSpawnPointWorldPosition();
        GameObject[] GetItemSpawnList();
        GameObject[] GetSlotList();
    }
}