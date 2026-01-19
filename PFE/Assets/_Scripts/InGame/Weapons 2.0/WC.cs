using Unity.Netcode;
using UnityEngine;

public class WC : NetworkBehaviour
{
    [field : SerializeField]
    public WCTypeInfo WCData { get; private set; }

    [SerializeField] WCPickup _pickup;

    protected CoreEvent coreEvent;
    protected Core core;

    public virtual void Activate(CoreEvent linkedEvent, Core linkedCore)
    {
        coreEvent = linkedEvent;
        core = linkedCore;

        coreEvent.linkedWC = this;
        linkedEvent.triggerEvent.AddListener(Trigger);

        print(coreEvent.linkedWC);
        print(linkedEvent.linkedWC);

        print(gameObject.name + " was linked to the event " + coreEvent.eventName + " in the " + core.gameObject.name + " core.");
    }

    public virtual void Trigger(CoreEventContext context) { }

    public virtual void Deactivate()
    {
        print("Deactivate " + gameObject.name);

        coreEvent.triggerEvent.RemoveListener(coreEvent.linkedWC.Trigger);

        _pickup.Drop();
    }
}
