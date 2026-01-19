using UnityEngine;

public class Hands : MonoBehaviour
{
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
