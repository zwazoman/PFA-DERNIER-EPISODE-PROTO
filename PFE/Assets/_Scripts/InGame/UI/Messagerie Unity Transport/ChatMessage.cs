using TMPro;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public TMP_Text _nameText;
    [SerializeField] public TMP_Text _messageText;

    [Header("Parameters")]
    [SerializeField] Color _sentColor;
    [SerializeField] Color _receivedColor;

    public void SetupMessage(string senderName, string message, bool sent = true)
    {
        if (sent)
            _nameText.color = Color.blue;
        else
            _nameText.color = Color.red;

        _nameText.text = senderName + " :";
        _messageText.text = message;
    }
}

public struct Message
{
    public string senderName;
    public string message;

    public Message(string senderName, string message)
    {
        this.senderName = senderName;
        this.message = message;
    }
}
