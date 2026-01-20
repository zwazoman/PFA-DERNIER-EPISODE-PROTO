using UnityEngine;

public class PlayerHands : PlayerScript
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
}
