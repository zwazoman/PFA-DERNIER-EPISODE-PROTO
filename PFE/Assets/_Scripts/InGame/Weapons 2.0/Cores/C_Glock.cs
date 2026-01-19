using Unity.Netcode;
using UnityEngine;

public class C_Glock : ClassicCore
{
    [Header("Gun Specific")]
    [SerializeField] GameObject _projectilePrefab;

    public override void StartShootTrigger()
    {
        print("caca tout dur");

        SpawnProjectileRpc();

        TriggerCoreEvent("CACA PARTOUT", SetupContext());
    }

    [Rpc(SendTo.Server)]
    void SpawnProjectileRpc()
    {
        GameObject projectile = Instantiate(_projectilePrefab, transform.position, transform.rotation);
        NetworkObject networkProjectile = projectile.GetComponent<NetworkObject>();

        networkProjectile.Spawn();
    }

    public override void StopShootTrigger()
    {
        TriggerCoreEvent("CHIASSE INFERNALE", SetupContext());
    }
}
