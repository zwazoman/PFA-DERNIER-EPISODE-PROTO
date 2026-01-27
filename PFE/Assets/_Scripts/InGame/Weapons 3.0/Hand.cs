using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Netcode;
using UnityEngine;

public class Hand : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMain _main;
    [SerializeField] Transform _holdingSocket;


    [Header("Parameters")]

    [SerializeField] int _inventorySize;
    [SerializeField] float _grabSpeed = .5f;

    [HideInInspector] public Item heldItem;

    bool _itemGrabbed;


    private void Update()
    {
        if(heldItem != null && _itemGrabbed)
        {
            heldItem.transform.position = _holdingSocket.transform.position;
            heldItem.transform.rotation = _holdingSocket.transform.rotation;
        }
    }

    public async void EquipItem(Item item)
    {
        heldItem = item;

        TryChangeOwnershipRpc(NetworkManager.Singleton.LocalClientId, heldItem);
        await WaitForOwnership(NetworkManager.Singleton.LocalClientId, heldItem);

        item.OnPickup();

        item.transform.parent = _main.transform;

        item.transform.DOMove(_holdingSocket.position, _grabSpeed);
        await item.transform.DORotate(_holdingSocket.rotation.eulerAngles, _grabSpeed);
        _itemGrabbed = true;
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
        }
    }

    async UniTask WaitForOwnership(ulong clientID, Item item)
    {
        while(item.OwnerClientId != clientID)
        {
            await UniTask.Yield();
        }
    }

    //end

    public void DropItem()
    {
        if (heldItem == null)
            return;

        heldItem.transform.parent = null;
        _itemGrabbed = false;

        heldItem.OnDrop();

        heldItem.NetworkObject.ChangeOwnership(0);
        heldItem = null;
    }
}
