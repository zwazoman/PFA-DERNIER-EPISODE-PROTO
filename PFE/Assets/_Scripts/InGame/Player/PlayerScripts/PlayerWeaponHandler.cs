using Cysharp.Threading.Tasks;
using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : PlayerScript
{
    #region Events

    public event Action<Core, Core> OnCoreLink;

    #endregion

    [HideInInspector] public Core leftWeaponCore;
    [HideInInspector] public Core rightWeaponCore;

    [HideInInspector] public bool canUseCores = true;

    Core _lastUsedCore;

    [Header("References")]
    [SerializeField] Transform _leftCoreSocket;
    [SerializeField] Transform _rightCoreSocket;

    [Header("Parameters")]
    [SerializeField] float _reloadHoldWindow = .5f;

    float _reloadTimer = 0f;

    private void Start()
    {
        LinkExistingCores();

        if (leftWeaponCore != null)
            leftWeaponCore.Equip(this); ;

        if (rightWeaponCore != null)
            rightWeaponCore.Equip(this);
    }

    void LinkExistingCores()
    {
        leftWeaponCore = _leftCoreSocket.GetComponentInChildren<Core>();
        rightWeaponCore = _rightCoreSocket.GetComponentInChildren<Core>();
    }


    public async UniTask<bool> LinkCore(Core newCore)
    {
        return await main.uiMain.weaponMenu.OpenCoreChoiceMenu(newCore);
    }

    public async UniTask<bool> LinkWC(WC newWC)
    {
        return await main.uiMain.weaponMenu.OpenCoreEventChoiceMenu(newWC);
    }

    public async void LinkWC(WC newWC, CoreEvent coreEvent, Core linkedCore)
    {
        ChangeOwnershipRpc(NetworkManager.Singleton.LocalClientId, newWC);

        while (newWC.IsOwnedByServer)
        {
            if (IsOwnedByServer)
                break;

            await UniTask.Yield();
        }

        if (coreEvent.linkedWC != null)
            coreEvent.linkedWC.Deactivate();

        newWC.Activate(coreEvent, linkedCore);

        newWC.transform.parent = main.transform;
        newWC.GetComponent<NetworkTransform>().SetState(coreEvent.wcSocket.position, coreEvent.wcSocket.rotation);
    }


    public async void LinkCore(Core core, bool RightSelected)
    {
        ChangeOwnershipRpc(NetworkManager.Singleton.LocalClientId, core);

        while (core.IsOwnedByServer)
        {
            if (IsOwnedByServer)
                break;

            await UniTask.Yield();
        }

        Transform socket = null;

        if (RightSelected)
        {
            if (rightWeaponCore != null)
            {
                rightWeaponCore.UnEquip();
            }

            socket = _rightCoreSocket;
            rightWeaponCore = core;
            rightWeaponCore.Equip(this);
        }
        else
        {
            if (leftWeaponCore != null)
            {
                leftWeaponCore.UnEquip();
            }

            socket = _leftCoreSocket;
            leftWeaponCore = core;
            leftWeaponCore.Equip(this);
        }

        core.transform.parent = main.transform;
        core.GetComponent<NetworkTransform>().SetState(socket.position, socket.rotation);

        OnCoreLink?.Invoke(leftWeaponCore, rightWeaponCore);
    }

    async void PositionWC(WC wc, CoreEvent coreEvent)
    {
        ChangeOwnershipRpc(NetworkManager.Singleton.LocalClientId, wc);

        while (wc.IsOwnedByServer)
        {
            if (IsOwnedByServer)
                break;

            await UniTask.Yield();
        }



        wc.transform.parent = main.transform;
        wc.GetComponent<NetworkTransform>().SetState(coreEvent.wcSocket.position, coreEvent.wcSocket.rotation);
    }

    [Rpc(SendTo.Server)]
    void ChangeOwnershipRpc(ulong clientID, NetworkBehaviourReference NetworkBhvRef)
    {
        NetworkBehaviour networkBhv;

        if (NetworkBhvRef.TryGet(out networkBhv))
        {
            NetworkObject netObj = networkBhv.GetComponent<NetworkObject>();
            netObj.ChangeOwnership(clientID);
        }
    }

    #region Inputs

    public void HandleLeftCoreInputs(InputAction.CallbackContext ctx)
    {
        if (leftWeaponCore == null || !main.CheckActionmap(ctx.action.actionMap) || !canUseCores)
            return;

        if (ctx.started)
            leftWeaponCore.StartShootTrigger();
        else if (ctx.canceled)
            leftWeaponCore.StopShootTrigger();

        _lastUsedCore = leftWeaponCore;
    }

    public void HandleRightCoreInputs(InputAction.CallbackContext ctx)
    {
        if (rightWeaponCore == null || !main.CheckActionmap(ctx.action.actionMap) || !canUseCores)
            return;

        if (ctx.started)
            rightWeaponCore.StartShootTrigger();
        else if (ctx.canceled)
            rightWeaponCore.StopShootTrigger();

        _lastUsedCore = rightWeaponCore;
    }

    public void Reload(InputAction.CallbackContext ctx)
    {
        if (!main.CheckActionmap(ctx.action.actionMap) || (rightWeaponCore != null && leftWeaponCore != null) || !canUseCores)
            return;

        if (_reloadTimer < _reloadHoldWindow)
        {
            if (_lastUsedCore == null)
                ChooseReload();
            else
                _lastUsedCore.ReloadTrigger();
        }
        else
            ChooseReload();

        _reloadTimer = 0;
    }

    void ChooseReload()
    {
        print("Reload choosen");


        //fenetre pour choisir quel core reload
        //if (_lastUsedCore == null)
        //    ChooseReload();
        //else
        //    _lastUsedCore.Reload();
        //tmp
    }

    #endregion
}
