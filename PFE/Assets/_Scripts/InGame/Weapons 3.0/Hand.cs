using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] Transform _holdingSocket;

    [SerializeField] float _grabSpeed = .5f;

    [HideInInspector] public Item heldItem;

    public async void EquipItem(Item item)
    {
        heldItem = item;

        item.transform.parent = _holdingSocket;

        await item.transform.DOMove(_holdingSocket.position, _grabSpeed);
        await item.transform.DORotate(_holdingSocket.rotation.eulerAngles, _grabSpeed);

        item.OnPickup();
    }

    public void DropItem()
    {
        if (heldItem == null)
            return;

        heldItem.transform.parent = null;
        //reset physique

        heldItem = null;

        heldItem.OnDrop();
    }
}
