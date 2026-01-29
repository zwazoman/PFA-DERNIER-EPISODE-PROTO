using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;
using Unity.VisualScripting;
using Unity.Netcode.Components;

public class Hand: NetworkBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMain _main;
    [SerializeField] Transform _holdingSocket;

    [Header("Parameters")]

    [SerializeField] public ItemType type;

    [SerializeField] int _inventorySize = 1;
    [SerializeField] float _grabSpeed = .5f;

    [HideInInspector] public Item heldItem;
    [HideInInspector] public List<Item> itemSlots = new();

    bool _itemGrabbed;

    private void Awake()
    {
        print(gameObject.name + " can hold " + itemSlots.Count + " items");
    }

    private void Update()
    {
        if(heldItem != null && _itemGrabbed)
        {
            heldItem.transform.position = _holdingSocket.transform.position;
            heldItem.transform.rotation = _holdingSocket.transform.rotation;
        }
    }

    public async void PickupItem(Item item)
    {
        itemSlots.Add(item);

        if (heldItem == null)
            heldItem = item;

        print("ask for ownership");
        TryChangeOwnershipRpc(NetworkManager.Singleton.LocalClientId, heldItem);
        await WaitForOwnership(NetworkManager.Singleton.LocalClientId, heldItem);

        item.OnPickup();

        item.transform.parent = _main.transform;

        _itemGrabbed = false;

        item.transform.DOMove(_holdingSocket.position, _grabSpeed);
        await item.transform.DORotate(_holdingSocket.rotation.eulerAngles, _grabSpeed);

        _itemGrabbed = true;

        print(OwnerClientId);
        print(item.OwnerClientId);
    }

    public bool TryPickupItem(Item item)
    {
        if(itemSlots.Count < _inventorySize)
        {
            PickupItem(item);
            return true;
        }

        return false;
    }

    void EquipItem(Item item)
    {
        heldItem.gameObject.SetActive(false); //marche pas en réseau

        heldItem = item;

        heldItem.gameObject.SetActive(true); //non plus
    }

    public void SwitchToPreviousHeldItem()
    {
        EquipItem(itemSlots.GetPreviousObjectWrapped(heldItem));
    }

    public void SwitchToNextHeldItem()
    {
        EquipItem(itemSlots.GetNextObjectWrapped(heldItem));
    }

    public void DropItem()
    {
        if (heldItem == null)
            return;

        itemSlots.Remove(heldItem);

        heldItem.transform.parent = null;
        _itemGrabbed = false;

        heldItem.OnDrop();

        heldItem.NetworkObject.ChangeOwnership(0);
        heldItem = null;
    }

    //todo : extension networkObj

    [Rpc(SendTo.Server)]
    void TryChangeOwnershipRpc(ulong clientID, NetworkBehaviourReference networkBhvRef)
    {

        if (networkBhvRef.TryGet(out NetworkBehaviour networkBhv))
        {
            if (networkBhv.OwnerClientId == clientID)
                return;
            networkBhv.NetworkObject.ChangeOwnership(clientID);
            print("Ownership granted");
        }
    }

    async UniTask WaitForOwnership(ulong clientID, Item item)
    {
        print("wait for ownership");

        float time = 0;

        while(item.OwnerClientId != clientID)
        {
            time += Time.deltaTime;
            await UniTask.Yield();
        }
        print(time);
    }

    //end

}
