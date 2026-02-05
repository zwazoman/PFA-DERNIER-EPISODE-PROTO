using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PeriodicSpawner : NetworkBehaviour
{
    [SerializeField] NetworkPoolTest _pool;

    [SerializeField] GameObject _connard;

    [SerializeField] float _delay = 3;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer)
            return;

        TestSpawn();
    }

     async void TestSpawn()
    {
        while (true)
        {
            await Awaitable.WaitForSecondsAsync(_delay);
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        NetworkObject.InstantiateAndSpawn(_connard, NetworkManager);
    }
}
