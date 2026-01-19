using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponMenu : PlayerScript
{
    [HideInInspector] bool menuOpened;

    [Header("References")]
    [SerializeField] CoreUi _leftCoreUi;
    [SerializeField] CoreUi _rightCoreUi;
    [SerializeField] CoreUi _newCoreUI;
    [SerializeField] WCUI _newWCUI;

    bool _leftSelected = false;
    bool _rightSelected = false;
    bool _shouldLeave = false;

    CoreEventUI _selectedUIEvent;

    private void Start()
    {
        main.playerWeaponHandler.OnCoreLink += EditCores;

        _leftCoreUi.OnClicked += SelectLeft;
        _rightCoreUi.OnClicked += SelectRight;
    }

    void Activate()
    {
        _shouldLeave = false;

        gameObject.SetActive(true);
        main.SwapActionMapToUI();

        _leftCoreUi.gameObject.SetActive(true);
        _rightCoreUi.gameObject.SetActive(true);

        menuOpened = true;
    }

    void Deactivate()
    {
        _shouldLeave = true;
        _selectedUIEvent = null;

        gameObject.SetActive(false);
        main.SwapActionMapToPlayer();

        _leftCoreUi.gameObject.SetActive(false);
        _rightCoreUi.gameObject.SetActive(false);
        _newCoreUI.gameObject.SetActive(false);
        _newWCUI.gameObject.SetActive(false);

        menuOpened = false;
    }

    void EditCores(Core leftCore, Core rightCore)
    {
        if (leftCore != _leftCoreUi.core)
        {
            _leftCoreUi.SwapCore(leftCore);
        }
        if (rightCore != _rightCoreUi.core)
        {
            _rightCoreUi.SwapCore(rightCore);
        }
    }

    public async UniTask<bool> OpenCoreChoiceMenu(Core newCore)
    {
        Activate();
        _newCoreUI.gameObject.SetActive(true);

        _newCoreUI.SwapCore(newCore);

        while (!_leftSelected && !_rightSelected)
        {
            await UniTask.Yield();

            if (_shouldLeave)
                return false;
        }

        if (_leftSelected)
        {
            main.playerWeaponHandler.LinkCore(newCore, false);
            _leftSelected = false;
        }
        else if (_rightSelected)
        {
            main.playerWeaponHandler.LinkCore(newCore, true);
            _rightSelected = false;
        }

        Deactivate();
        _newCoreUI.gameObject.SetActive(false);
        return true;
    }

    public async UniTask<bool> OpenCoreEventChoiceMenu(WC newWC)
    {
        Activate();

        _newWCUI.gameObject.SetActive(true);
        _newWCUI.SwapWC(newWC);


        foreach(CoreEventUI leftCoreEventUi in _leftCoreUi.coreEventUis)
            leftCoreEventUi.OnClicked += LinkWC;
        foreach(CoreEventUI rightCoreEventUi in _rightCoreUi.coreEventUis)
            rightCoreEventUi.OnClicked += LinkWC;


        while (_selectedUIEvent == null)
        {
            await UniTask.Yield();

            if (_shouldLeave) 
                return false;
        }

        _selectedUIEvent.LinkWC(newWC);

        main.playerWeaponHandler.LinkWC(newWC, _selectedUIEvent.coreEvent, _selectedUIEvent.core);

        foreach (CoreEventUI leftCoreEventUi in _leftCoreUi.coreEventUis)
            leftCoreEventUi.OnClicked -= LinkWC;
        foreach (CoreEventUI rightCoreEventUi in _rightCoreUi.coreEventUis)
            rightCoreEventUi.OnClicked -= LinkWC;

        Deactivate();
        return true;
    }

    void SelectLeft() => _leftSelected = true;
    void SelectRight() => _rightSelected = true;

    void LinkWC(CoreEventUI coreEventUI) => _selectedUIEvent = coreEventUI; 

    public void OpenWeaponMenu(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            print("open Ui");

            if (menuOpened)
                Deactivate();
            else
                Activate();
        }
    }
}
