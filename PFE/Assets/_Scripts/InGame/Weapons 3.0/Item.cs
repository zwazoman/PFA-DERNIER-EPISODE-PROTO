using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void Use() { }
    public virtual void Break() { }
    public virtual void OnDrop() { }
    public virtual void OnPickup() { }
}
