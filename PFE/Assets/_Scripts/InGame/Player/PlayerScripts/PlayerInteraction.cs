using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : PlayerScript
{
    Interactable _currentInteractable;

    [Header("Parameters")]
    [SerializeField] float _interactionWidth;
    [SerializeField] float _interactionRange;

    [SerializeField] LayerMask _interactionmask;

    public bool canInteract;

    private void Update()
    {
        RaycastHit hit;

        if (Physics.SphereCast(main.playerCamera.transform.position, _interactionWidth, main.playerCamera.transform.forward, out hit, _interactionRange, _interactionmask))
        {
            if (hit.collider.gameObject.TryGetComponent(out Interactable interactable))
            {
                _currentInteractable = interactable;
            }
            else
            {
                _currentInteractable = null;
            }
        }
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _currentInteractable.Interact(this);
        }
    }


}
