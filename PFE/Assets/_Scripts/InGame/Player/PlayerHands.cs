using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHands : MonoBehaviour
{
    public Hand leftHand;
    public Hand rightHand;

    public void Equip(Item item, bool EquipInLeftHand = true)
    {
        if(EquipInLeftHand)
            leftHand.EquipItem(item);
        else 
            rightHand.EquipItem(item);
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

    #endregion
}
