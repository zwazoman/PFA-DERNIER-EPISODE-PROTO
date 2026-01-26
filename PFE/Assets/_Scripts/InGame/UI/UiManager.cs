using UnityEngine;
using UnityEngine.InputSystem;

public class UiManager : PlayerScript
{
    [SerializeField] ChatManager _chat;

    public void OpenChat(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _chat.gameObject.SetActive(true);
            _chat.chatOpened = true;
            main.SwapActionMapToUI();
        }
    }

    public void CloseUI(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            if (_chat.chatOpened)
            {
                _chat.gameObject.SetActive(false);
            }

            main.SwapActionMapToPlayer();
        }
    }


}
