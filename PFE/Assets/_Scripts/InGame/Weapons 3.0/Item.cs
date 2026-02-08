using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class Item : NetworkBehaviour
{
    [SerializeField] public ItemType type;

    protected bool isUsing;

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

    public virtual void OnDrop()
    {
        print(gameObject.name + "Dropped !");

        //_coll.enabled = true;
        //_rb.isKinematic = false;
        ////_rbNetwork.SetIsKinematic(false);
        //isInteractable = true;
    }

    public virtual void OnPickup()
    {
        print(gameObject.name + "Picked up !");

        //_coll.enabled = false;
        ////_rb.isKinematic = true;
        //_rbNetwork.SetIsKinematic(true);
        //isInteractable = false;
    }

    public virtual void OnEquip()
    {

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
