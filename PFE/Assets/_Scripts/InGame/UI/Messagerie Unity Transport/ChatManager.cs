using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerMain _main;

    [SerializeField] public GameObject chatPanel;

    [SerializeField] Transform _chatContent;
    [SerializeField] TMP_InputField _chatInputField;

    [SerializeField] ServerBehaviour _serverBehaviour;
    [SerializeField] ClientBehavour _clientBehavour;


    [Header("Parameters")]
    [SerializeField] GameObject _newMessagePrefab;
    [SerializeField] string _clientUsername = "George";
    [SerializeField] string _serverUsername = "Marco";

    [HideInInspector] public bool chatOpened = false;


    void Awake()
    {
        TryGetComponent(out _serverBehaviour);
        TryGetComponent(out _clientBehavour);

        _main.OnNetworkSpawned += OnNetworkSpawn;
    }

    void OnNetworkSpawn()
    {
        if (_main.IsHost)
        {
            _serverBehaviour.Initialize();
            _serverBehaviour.OnMessageReceived += ReceiveNewMessage;
        }
        else
        {
            _clientBehavour.Initialize();
            _clientBehavour.OnMessageReceived += ReceiveNewMessage;
        }

        _chatInputField.onSubmit.AddListener(SendNewMessage);
    }

    void SendNewMessage(string content)
    {
        print("submit input");

        _chatInputField.text = null;
        Message message;

        if (_main.IsHost)
        {
            message = new(_serverUsername, content);
            CreateNewMessageObj(_serverUsername, content, true);
            _serverBehaviour.WriteNewMessage(message);
        }
        else
        {
            message = new(_clientUsername, content);
            CreateNewMessageObj(_clientUsername, content, true);
            _clientBehavour.WriteNewMessage(message);
        }
    }

    void ReceiveNewMessage(Message message)
    {
        CreateNewMessageObj(message.senderName, message.message, false);
    }

    void CreateNewMessageObj(string senderName, string message, bool sent = true)
    {
        GameObject _messageObject = Instantiate(_newMessagePrefab, _chatContent);
        _messageObject.TryGetComponent(out ChatMessage chatMessage);

        chatMessage.SetupMessage(senderName, message, sent);
    }
}
