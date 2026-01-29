using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHands : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;

    public Hand[] hands = { null, null };

    private void Awake()
    {
        hands[0] = leftHand;
        hands[1] = rightHand;
    }

    public void Equip(Item item)
    {
        bool itemEquipped = false;

        foreach(Hand hand in hands)
        {
            if(hand.type == item.type && !itemEquipped)
            {
                if (hand.TryPickupItem(item))
                    return;
            }
        }
    }

    #region Inputs

    public void UseRight(InputAction.CallbackContext ctx)
    {
        if (rightHand.heldItem == null)
            return;

        if (ctx.started)
            rightHand.heldItem.StartUsing();
        if (ctx.canceled)
            rightHand.heldItem.StopUsing();
    }

    public void UseLeft(InputAction.CallbackContext ctx)
    {
        if (leftHand.heldItem == null)
            return;

        if (ctx.started)
            leftHand.heldItem.StartUsing();
        if (ctx.canceled)
            leftHand.heldItem.StopUsing();
    }

    public void DropLeft(InputAction.CallbackContext ctx)
    {
        if (leftHand.heldItem == null)
            return;

        if (ctx.started)
        {
            leftHand.DropItem();
        }

    }

    public void SwitchEquip(InputAction.CallbackContext ctx)
    {

    }

    #endregion
}

public enum ItemType
{
    Weapon,
    Consummable
}
