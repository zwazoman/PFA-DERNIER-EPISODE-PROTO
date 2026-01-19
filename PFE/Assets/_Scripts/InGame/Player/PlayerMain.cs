using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
public class PlayerMain : NetworkBehaviour
{
    [Header("Objects")]
    [field : SerializeField] 
    public Camera playerCamera { get; private set; }

    [field : SerializeField]
    public PlayerInput playerInput { get; private set; }

    [Header("Scripts")]
    [field : SerializeField]
    public PlayerMovement playerMovement { get; private set; }

    [field : SerializeField]
    public PlayerHealth playerHealth { get; private set; }

    [field : SerializeField]
    public PlayerWeaponHandler playerWeaponHandler { get; private set; }

    [field: SerializeField]
    public PlayerInteraction playerInteraction { get; private set; }

    [field : SerializeField]
    public PlayerUiMain uiMain { get; private set; }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
            playerMovement.main = this;
            playerHealth.main = this;
            playerWeaponHandler.main = this;
            playerInteraction.main = this;
            uiMain.main = this;

            GameObject.Find("Start Camera").SetActive(false);
        }
        else
        {
            //désactive les scripts inutiles et les inputs des non-owners

            playerInput.enabled = false;
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;

            playerMovement.enabled = false;
            playerHealth.enabled = false;
            playerWeaponHandler.enabled = false;
            playerInteraction.enabled = false;
            uiMain.enabled = false;
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
