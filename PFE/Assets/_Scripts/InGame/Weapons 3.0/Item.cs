using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class Item : Interactable
{
    [SerializeField] Collider _coll;

    [SerializeField] Rigidbody _rb;
    [SerializeField] NetworkRigidbody _rbNetwork;

    protected bool isUsing;

    protected override void Awake()
    {
        base.Awake();

        TryGetComponent(out  _coll);

        TryGetComponent(out _rb);
        TryGetComponent(out _rbNetwork);
    }

    public virtual void StartUsing()
    {
        print(gameObject.name + " : Start Using");

        isUsing = true;
        Use();
    }

    public virtual void UseUpdate() { }

    public virtual void StopUsing()
    {
        print(gameObject.name + " : Stop Using");

        isUsing = false;
    }

    public virtual void Break() { }

    public void OnDrop()
    {
        print(gameObject.name + "Dropped !");

        _coll.enabled = true;
        _rb.isKinematic = false;
        //_rbNetwork.SetIsKinematic(false);
        isInteractable = true;
    }

    public void OnPickup()
    {
        print(gameObject.name + "Picked up !");

        _coll.enabled = false;
        _rb.isKinematic = true;
        //_rbNetwork.SetIsKinematic(true);
        isInteractable = false;
    }

    public override void Interact(PlayerInteraction interaction)
    {
        base.Interact(interaction);

        interaction.main.playerHands.Equip(this);
    }

    async void Use()
    {
        while (isUsing)
        {
            UseUpdate();
            await UniTask.Yield();
        }
    }
   
}
