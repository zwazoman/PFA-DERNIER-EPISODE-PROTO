using AYellowpaper.SerializedCollections;
using UnityEngine;

public enum PoolType
{

}

public class PoolManager : MonoBehaviour
{
    #region Singleton
    private static PoolManager instance;

    public static PoolManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("Pool Manager");
                instance = go.AddComponent<PoolManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null || instance == this)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    [SerializedDictionary("PoolType", "Pool")] public SerializedDictionary<PoolType, Pool> pools;

    public Pool GetPool(PoolType pool)
    {
        if (!pools.ContainsKey(pool))
        {
            Debug.LogError("Wrong Name");
            return null;
        }

        return pools[pool];
    }
}
