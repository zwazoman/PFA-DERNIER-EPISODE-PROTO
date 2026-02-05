using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class NetworkPoolTest : NetworkBehaviour, INetworkPrefabInstanceHandler
{
    [SerializeField] GameObject _prefab;
    [SerializeField] int _poolSize;

    Queue<GameObject> _pool = new();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer)
            return;

        NetworkManager.PrefabHandler.AddHandler(_prefab, this);
        InitPool();
    }

    public void InitPool()
    {
        print("init pool");
        for(int i = 0; i < _poolSize; i++)
        {
            GameObject newObject = Instantiate(_prefab, transform);
            _pool.Enqueue(newObject);
            newObject.SetActive(false);
        }
    }

    public void ReturnToPool()
    {

    }

    public void Destroy(NetworkObject networkObject)
    {
        print("destroy via pool");
        networkObject.gameObject.SetActive(false);
        networkObject.transform.SetPositionAndRotation(transform.position, transform.rotation);

        //networkObject.transform.parent = transform;
        _pool.Enqueue(networkObject.gameObject);
    }

    public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
    {
        print("instantiate via pool");
        GameObject newObject = _pool.Dequeue();
        newObject.transform.SetPositionAndRotation(position, rotation);
        newObject.SetActive(true);

        NetworkObject networkBhv = newObject.GetComponent<NetworkObject>();
        return networkBhv;
    }
}
