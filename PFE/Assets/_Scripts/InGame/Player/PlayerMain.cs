using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine.EventSystems;
public class PlayerMain : NetworkBehaviour
{
    [Header("Objects")]
    [field : SerializeField] 
    public Camera playerCamera { get; private set; }

    [field : SerializeField]
    public PlayerInput playerInput { get; private set; }

    [field : SerializeField]
    public EventSystem eventSystem { get; private set; }

    [Header("Scripts")]
    [field : SerializeField]
    public PlayerMovement playerMovement { get; private set; }

    [field : SerializeField]
    public PlayerHealth playerHealth { get; private set; }

    [field: SerializeField]
    public PlayerInteraction playerInteraction { get; private set; }

    [field: SerializeField]
    public PlayerHands playerHands { get; private set; }


    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            playerMovement.main = this;
            playerHealth.main = this;
            playerInteraction.main = this;
            playerHands.main = this;

            GameObject.Find("Start Camera").SetActive(false);
        }
        else
        {
            //désactive les scripts inutiles et les inputs des non-owners

            playerInput.enabled = false;
            playerCamera.enabled = false;
            eventSystem.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;

            playerMovement.enabled = false;
            playerHealth.enabled = false;
            playerInteraction.enabled = false;
            playerHands.enabled = false;
        }
    }

    public bool CheckActionmap(InputActionMap actionMap)
    {
        if (actionMap == playerInput.currentActionMap)
            return true;
        return false;
    }

    public void SwapActionMapToUI()
    {
        Cursor.lockState = CursorLockMode.Confined;

        playerInput.SwitchCurrentActionMap("UI");
    }

    public void SwapActionMapToPlayer()
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerInput.SwitchCurrentActionMap("Player");
    }
}
