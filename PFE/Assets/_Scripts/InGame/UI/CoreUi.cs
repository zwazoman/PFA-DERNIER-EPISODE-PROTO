using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class CoreUi : MonoBehaviour
{
    public event Action OnClicked;

    [HideInInspector] public Core core;

    [Header("References")]
    [SerializeField] GameObject _coreEventUiPrefab;
    [SerializeField] TMP_Text _coreName;
    [SerializeField] Image _coreImage;
    [SerializeField] VerticalLayoutGroup _coreEventsLayout;

    [HideInInspector] public List<CoreEventUI> coreEventUis = new();

    public void SwapCore(Core newCore)
    {
        core = newCore;

        _coreName.text = core.coreData.coreName;
        _coreImage.sprite = core.coreData.sprite;

        if (coreEventUis.Count > 0)
        {
            foreach (CoreEventUI ui in coreEventUis)
                Destroy(ui.gameObject);
            coreEventUis.Clear();
        }

                


        for (int i = 0; i < core.coreEvents.Count; i++)
        {
            CoreEventUI coreEventUI = Instantiate(_coreEventUiPrefab, _coreEventsLayout.transform).GetComponent<CoreEventUI>();

            coreEventUis.Add(coreEventUI);
            coreEventUI.core = core;
            coreEventUI.SwapCoreEvent(core.coreEvents[i]);
        }
    }

    public void EmptyCore()
    {

    }

    public void Click() => OnClicked?.Invoke();
}
