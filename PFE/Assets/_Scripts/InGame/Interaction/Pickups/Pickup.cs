using System;
using Unity.Netcode.Components;
using UnityEngine;

public class Pickup<T> : Interactable
{
    #region Events

    public event Action OnDropped;
    public event Action OnPickedUp;

    #endregion

    public bool IsOwned;

    protected T linkedObject;

    [SerializeField] Collider _coll;
    [SerializeField] Rigidbody _rb;

    protected PlayerInteraction currentPlayerInteraction;

    protected override void Awake()
    {
        base.Awake();

        TryGetComponent(out _coll);
        TryGetComponent(out _rb);
    }

    public override void Interact(PlayerInteraction interaction)
    {
        base.Interact(interaction);

        TryPickup(interaction);
    }

    protected virtual void TryPickup(PlayerInteraction playerInteraction)
    {
        //désactive gravité collisions etc

        print("picked up");
        currentPlayerInteraction = playerInteraction;

        isInteractable = false;
        _coll.enabled = false;
        _rb.isKinematic = true;

        GetComponent<NetworkRigidbody>().SetIsKinematic(true);

        StopHover();

        OnPickedUp?.Invoke();
    }

    public virtual void Drop()
    {
        //réactive tout

        transform.parent = null;
        isInteractable = true;
        _coll.enabled = true;
        _rb.isKinematic = false;

        _rb.AddForce(Vector3.up * 300 + currentPlayerInteraction.transform.forward * 100);
        _rb.AddTorque(new Vector3(1,1,1));

        currentPlayerInteraction = null;

        OnDropped?.Invoke();
    }
}
