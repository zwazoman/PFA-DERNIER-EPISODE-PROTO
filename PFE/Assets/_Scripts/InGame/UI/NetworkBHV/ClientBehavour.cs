using System;
using System.Text;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class ClientBehavour : MonoBehaviour
{
    public event Action<string, string> OnMessageReceived;
    [HideInInspector] public string Username;

    bool _initialized = false;

    NetworkDriver m_Driver;
    NetworkConnection m_Connection;

    public void Initialize()
    {
        m_Driver = NetworkDriver.Create();

        var endpoint = NetworkEndpoint.LoopbackIpv4.WithPort(1000);
        m_Connection = m_Driver.Connect(endpoint);

        _initialized = true;
    }

    void Update()
    {
        if (!_initialized)
            return;

        m_Driver.ScheduleUpdate().Complete();

        if (!m_Connection.IsCreated)
        {
            return;
        }

        Unity.Collections.DataStreamReader stream;
        Unity.Networking.Transport.NetworkEvent.Type cmd;
        while ((cmd = m_Connection.PopEvent(m_Driver, out stream)) != Unity.Networking.Transport.NetworkEvent.Type.Empty)
        {
            if (cmd == Unity.Networking.Transport.NetworkEvent.Type.Connect)
            {
                Debug.Log("We are now connected to the server.");
            }
            else if (cmd == Unity.Networking.Transport.NetworkEvent.Type.Data)
            {
                int bufferLength = stream.ReadInt();
                NativeArray<byte> buffer = new(bufferLength, Allocator.Temp);

                stream.ReadBytes(buffer);

                string message = Encoding.UTF8.GetString(buffer.ToArray());
                OnMessageReceived(Username, message);
            }
            else if (cmd == Unity.Networking.Transport.NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server.");
                m_Connection = default;
            }
        }
    }

    public void WriteNewMessage(string content)
    {
        //encode le content
        byte[] encodedString = Encoding.Default.GetBytes(content);

        m_Driver.BeginSend(NetworkPipeline.Null, m_Connection, out var writer);
        writer.WriteInt(encodedString.Length);
        writer.WriteBytes(encodedString);
        m_Driver.EndSend(writer);
    }

    private void OnDestroy()
    {
        m_Driver.Dispose();
    }
}
