using Unity.Netcode;
using UnityEngine;

public class Interactable : NetworkBehaviour
{
    [HideInInspector] public bool isInteractable = true;
    [SerializeField] Material _hoveredMaterial;

    MeshRenderer _mR;
    Material _initialMaterial;

    protected virtual void Awake()
    {
        TryGetComponent(out _mR);
        _initialMaterial = _mR.material;

        if(gameObject.layer != 6)
            gameObject.layer = 6;
    }

    public virtual void Interact(PlayerInteraction interaction)
    {
        
    }

    public virtual void StartHover()
    {
        //feedback

        if(_hoveredMaterial != null)
            _mR.material = _hoveredMaterial;
    }

    public virtual void StopHover()
    {
        //feedback

        _mR.material = _initialMaterial;
    }

    //gérer feedback
}
