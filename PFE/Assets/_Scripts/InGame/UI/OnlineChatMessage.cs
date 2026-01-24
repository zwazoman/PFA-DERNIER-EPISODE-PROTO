using TMPro;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class OnlineChatMessage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TMP_Text _nameText;
    [SerializeField] public TMP_Text _messageText;

    [Header("Parameters")]
    [SerializeField] Color _sentColor;
    [SerializeField] Color _receivedColor;

    public void SetupMessage(string senderName, string message, bool sent = true)
    {
        _nameText.text = senderName;
        _messageText.text = message;

        if (sent)
            _nameText.color = _sentColor;
        else
            _nameText.color = _receivedColor;
    }
}
