using UnityEngine;
using UnityEngine.InputSystem;

public class UiManager : MonoBehaviour
{
    [SerializeField] PlayerMain _main;

    [SerializeField] ChatManager _chat;

    public void OpenChat(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _chat.chatPanel.SetActive(true);
            _chat.chatOpened = true;
            _main.SwapActionMapToUI();
        }
    }

    public void CloseUI(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (_chat.chatOpened)
            {
                _chat.chatPanel.SetActive(false);
            }

            _main.SwapActionMapToPlayer();
        }
    }


}
