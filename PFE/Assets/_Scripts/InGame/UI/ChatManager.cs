using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : PlayerScript
{
    [Header("References")]
    [SerializeField] Transform _chatContent;
    [SerializeField] TMP_InputField _chatInputField;

    [SerializeField] ServerBehaviour _serverBehaviour;
    [SerializeField] ClientBehavour _clientBehavour;


    [Header("Parameters")]
    [SerializeField] GameObject _newMessagePrefab;
    [SerializeField] string _username = "George";

    void Awake()
    {
        TryGetComponent(out _serverBehaviour);
        TryGetComponent(out _clientBehavour);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsHost)
        {
            _serverBehaviour.Initialize();
            _serverBehaviour.Username = _username;
            _serverBehaviour.OnMessageReceived += ReceiveNewMessage;
        }
        else
        {
            _clientBehavour.Initialize();
            _clientBehavour.Username = _username;
            _clientBehavour.OnMessageReceived += ReceiveNewMessage;
        }

        _chatInputField.onSubmit.AddListener(SendNewMessage);
    }

    void SendNewMessage(string message)
    {
        _chatInputField.text = null;
        CreateNewMessageObj(_username, message, true);

        if (IsHost)
            _serverBehaviour.WriteNewMessage(message);
        else
            _clientBehavour.WriteNewMessage(message);

    }

    void ReceiveNewMessage(string senderName, string message)
    {
        CreateNewMessageObj(senderName, message, false);
    }

    void CreateNewMessageObj(string senderName, string message, bool sent = true)
    {
        GameObject _messageObject = Instantiate(_newMessagePrefab, _chatContent);
        _messageObject.TryGetComponent(out OnlineChatMessage chatMessage);

        chatMessage.SetupMessage(senderName, message, sent);
    }
}
