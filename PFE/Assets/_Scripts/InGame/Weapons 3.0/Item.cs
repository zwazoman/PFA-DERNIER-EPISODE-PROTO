using Unity.VisualScripting;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField] Collider _coll;
    [SerializeField] Rigidbody _rb;

    public virtual void Use() { }
    public virtual void Break() { }

    public void OnDrop()
    {
        print(gameObject.name + "Dropped !");

        _coll.enabled = true;
        _rb.isKinematic = false;
        isInteractable = true;
    }

    public void OnPickup()
    {
        print(gameObject.name + "Picked up !");

        _coll.enabled = false;
        _rb.isKinematic = true;
        isInteractable = false;
    }

    public override void Interact(PlayerInteraction interaction)
    {
        base.Interact(interaction);

        interaction.main.playerHands.Equip(this);
    }
}
