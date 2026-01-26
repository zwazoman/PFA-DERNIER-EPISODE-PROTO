using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using System;
using System.Text;
using System.IO;


public class ServerBehaviour : MonoBehaviour
{
    public event Action<string, string> OnMessageReceived;
    [HideInInspector] public string Username;

    bool _initialized = false;

    NetworkDriver m_Driver;
    NativeList<NetworkConnection> m_Connections;
    
    public void Initialize()
    {
        m_Driver = NetworkDriver.Create();
        m_Connections = new(16, Allocator.Persistent);

        var endPoint = NetworkEndpoint.AnyIpv4.WithPort(1000);
        if(m_Driver.Bind(endPoint) != 0)
        {
            Debug.LogError("NON");
            return;
        }
        m_Driver.Listen();

        _initialized = true;
    }

    void Update()
    {
        if (!_initialized)
            return;

        m_Driver.ScheduleUpdate().Complete();

        //clean up connections
        for (int i = 0; i < m_Connections.Length; i++)
        {
            if (!m_Connections[i].IsCreated)
            {
                m_Connections.RemoveAtSwapBack(i);
                i--;
            }
        }

        //Accept new connections
        NetworkConnection c;
        while ((c = m_Driver.Accept()) != default)
        {
            m_Connections.Add(c);
            Debug.Log("Accept a connection");
        }

        for(int i = 0; i < m_Connections.Length; i++)
        {
            DataStreamReader stream;
            Unity.Networking.Transport.NetworkEvent.Type cmd;
            while((cmd = m_Driver.PopEventForConnection(m_Connections[i], out stream)) != Unity.Networking.Transport.NetworkEvent.Type.Empty)
            {
                if(cmd == Unity.Networking.Transport.NetworkEvent.Type.Data)
                {

                    int bufferLength = stream.ReadInt();
                    NativeArray<byte> buffer = new(bufferLength, Allocator.Temp);

                    stream.ReadBytes(buffer);

                    string message = Encoding.UTF8.GetString(buffer.ToArray());
                    OnMessageReceived(Username, message);
                }
                else if (cmd == Unity.Networking.Transport.NetworkEvent.Type.Disconnect)
                {
                    Debug.Log("Client disconnected from the server.");
                    m_Connections[i] = default;
                    break;
                }
            }
        }
    }

    public void WriteNewMessage(string content)
    {
        //encode le content
        byte[] encodedString = Encoding.Default.GetBytes(content);

        for (int i = 0; i < m_Connections.Length; i++)
        {
            m_Driver.BeginSend(NetworkPipeline.Null, m_Connections[i], out var writer);

            writer.WriteInt(encodedString.Length);
            writer.WriteBytes(encodedString);
            m_Driver.EndSend(writer);
        }
    }

    private void OnDestroy()
    {
        if (m_Driver.IsCreated)
        {
            m_Driver.Dispose();
            m_Connections.Dispose();
        }
    }
}
