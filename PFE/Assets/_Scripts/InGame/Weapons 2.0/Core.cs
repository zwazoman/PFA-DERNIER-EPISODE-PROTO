using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

public class Core : NetworkBehaviour
{
    [HideInInspector] protected PlayerWeaponHandler weaponHandler;

    [Header("Core Events")]
    public List<CoreEvent> coreEvents;

    [Header("Public References")]
    [SerializeField] public CorePickup pickup;
    [SerializeField] public CoreTypeInfo coreData;

    

    private void Awake()
    {
        TryGetComponent(out pickup);
    }

    public virtual void Equip(PlayerWeaponHandler newWeaponHandler)
    {
        weaponHandler = newWeaponHandler;
    }

    public virtual void UnEquip()
    {
        weaponHandler = null;
        pickup.Drop();
        //GiveOwnerShipToServerRpc();
    }

    //[Rpc(SendTo.Server)]
    //void GiveOwnerShipToServerRpc()
    //{
    //    GetComponent<NetworkObject>().ChangeOwnership(NetworkManager.Singleton.LocalClientId);
    //}

    public virtual void StartShootTrigger() { }

    public virtual void StopShootTrigger() { }

    public virtual void ReloadTrigger() { }

    protected virtual CoreEventContext SetupContext()
    {
        return new CoreEventContext();
    }

    protected void TriggerCoreEvent(string eventName, CoreEventContext context)
    {
        foreach (CoreEvent coreEvent in coreEvents)
        {
            if (coreEvent.eventName == eventName)
            {
                coreEvent.triggerEvent?.Invoke(context);
                return;
            }
        }
        Debug.LogError("no core event named " + eventName);
    }
}

[Serializable]
public class CoreEvent
{
    public string eventName;
    public Sprite sprite;
    public WCTypes types;
    public Transform wcSocket;
    [HideInInspector] public UnityEvent<CoreEventContext> triggerEvent;
    [HideInInspector] public WC linkedWC;
}

public struct CoreEventContext
{

}
