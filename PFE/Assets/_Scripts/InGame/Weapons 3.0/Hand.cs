using UnityEngine;

public class Hand : MonoBehaviour
{

    [SerializeField] Transform _holdingSocket;

    Item _heldItem;

    void UseItem()
    {
        if (_heldItem == null)
            return;

        _heldItem.Use();
    }

    void DropItem()
    {
        if (_heldItem == null)
            return;

        _heldItem.OnDrop();
    }
}
