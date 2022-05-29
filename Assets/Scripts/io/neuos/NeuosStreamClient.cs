using UnityEngine;
using System.Net.Sockets;
using System;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Net;
using UnityEngine.Events;
using static io.neuos.NeuosStreamConstants;

namespace io.neuos
{
    
    public class NeuosStreamClient : MonoBehaviour
    {
        [Serializable]
        public class ValueChangedEvent : UnityEvent<string, float> { }
        [Serializable]
        public class QAEvent : UnityEvent<bool, int> { }
        [Serializable]
        public class ErrorEvent : UnityEvent<string> { }
        [Serializable]
        public class ConnectionEvent : UnityEvent<int, int> { }

        [SerializeField]
        private UnityEvent OnServerConnected;
        [SerializeField]
        private UnityEvent OnServerDisconnected;
        [SerializeField]
        private ValueChangedEvent OnValueChanged;
        [SerializeField]
        private ConnectionEvent OnHeadbandConnectionChanged;
        [SerializeField]
        private QAEvent OnQAEvent;
        [SerializeField]
        private ErrorEvent OnError;

        public bool IsConnected { get; private set; }
        public string ApiKey { get; set; }

        private Socket m_Socket;
        private byte[] m_CommandLength = new byte[2];
        private byte[] m_recBuffer = new byte[1024];
        private byte[] m_keepAlive = new byte[1];
        private bool m_blockingState;

        
        

        public void ConnectToServer(string serverIp, int serverPort)
        {
            try
            {
                IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
                m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_Socket.Connect(serverAddress);
#if UNITY_EDITOR
                Debug.Log($"Connections Status: {m_Socket.Connected}");
#endif
                SendAuth();
            }
            catch (FormatException e)
            {
                OnError?.Invoke(e.Message);
#if UNITY_EDITOR
                Debug.LogError(e);
#endif
            }
            catch (SocketException ex)
            {
                OnError?.Invoke(ex.Message);
#if UNITY_EDITOR
                Debug.LogError(ex);
#endif
            }
        }
        public void Disconnect()
        {
            IsConnected = false;
            try
            {
                m_Socket?.Shutdown(SocketShutdown.Both);
            }
            catch (Exception e)
            {
                OnError?.Invoke(e.Message);
#if UNITY_EDITOR
                Debug.Log(e.Message);
#endif
            }
            finally
            {
                m_Socket?.Close();
            }
            m_Socket?.Dispose();
            OnServerDisconnected?.Invoke();
        }
        private void Update()
        {
            if (IsConnected)
            {

                if (m_Socket.Available > 2)
                {
                    var data = GetMessage();
                    var response = JObject.Parse(data);
                    var commandValue = (string)response.Property(StreamObjectKeys.COMMAND)?.Value;
                    // example of pulling the time stamp off the command
                    var timestamp = (long)response.Property(StreamObjectKeys.TIME_STAMP)?.Value;
                    
                    switch (commandValue)
                    {
                        case StreamCommandValues.VALUE_CHANGED:
                            {
                                var key = (string)response.Property(StreamObjectKeys.KEY)?.Value;
                                var value = (float)response.Property(StreamObjectKeys.VALUE)?.Value;
                                OnValueChanged?.Invoke(key, value);
                                break;
                            }
                        case StreamCommandValues.QA:
                            {
                                var passed = (bool)response.Property(StreamObjectKeys.PASSED)?.Value;
                                var failure = (int)response.Property(StreamObjectKeys.TYPE)?.Value;
                                OnQAEvent?.Invoke(passed, failure);
                                break;
                            }
                        case StreamCommandValues.SESSION_COMPLETE:
                            {
                                Disconnect();
                                break;
                            }
                        case StreamCommandValues.CONNECTION:
                            {
                                var previous =  (int)response.Property(StreamObjectKeys.PREVIOUS)?.Value;
                                var current = (int)response.Property(StreamObjectKeys.CURRENT)?.Value;
                                OnHeadbandConnectionChanged?.Invoke(previous, current);
                                break;
                            }
                    }
                }
                else // no data is avalable atm , so we verify our socket is still connected
                {

                    // This is how you can determine whether a socket is still connected.
                    m_blockingState = m_Socket.Blocking;
                    try
                    {
                        m_Socket.Blocking = false;
                        m_Socket.Send(m_keepAlive, 1, 0);
                    }
                    catch (SocketException e)
                    {
                        // 10035 == WSAEWOULDBLOCK
                        if (!e.NativeErrorCode.Equals(10035))
                        {
#if UNITY_EDITOR
                            Debug.Log("Connection lost");
#endif
                            Disconnect();
                        }
                    }
                    finally
                    {
                        if (IsConnected)
                        {
                            m_Socket.Blocking = m_blockingState;
                        }
                    }
                }
            }
        }
        private void SendAuth()
        {
            string toSend = getAuth();
            ushort toSendLen = (ushort)Encoding.UTF8.GetByteCount(toSend);
            byte[] toSendBytes = Encoding.UTF8.GetBytes(toSend);
            byte[] toSendLenBytes = BitConverter.GetBytes(toSendLen);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(toSendLenBytes);
            m_Socket.Send(toSendLenBytes);
            m_Socket.Send(toSendBytes);
#if UNITY_EDITOR
            Debug.Log("Sent Auth");
#endif
            GetAuthResponse();
        }

        private void GetAuthResponse()
        {
            var msg = GetMessage();
            var response = JObject.Parse(msg);
            var commandValue = ((string)response.Property(StreamObjectKeys.COMMAND)?.Value);
            if (commandValue == NeuosStreamConstants.StreamCommandValues.AUTH_SUCCESS)
            {
#if UNITY_EDITOR
                Debug.Log("Auth success");
#endif
                IsConnected = true;
                OnServerConnected?.Invoke();
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Auth failed");
#endif
                OnError?.Invoke("Failed to authenticate with server");
            }
        }

        private string GetMessage()
        {
            // Receiving
            m_Socket.Receive(m_CommandLength);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(m_CommandLength);
            int rcvLen = BitConverter.ToUInt16(m_CommandLength, 0);
            m_Socket.Receive(m_recBuffer, rcvLen, SocketFlags.None);
            string rcv = Encoding.UTF8.GetString(m_recBuffer, 0, rcvLen);
#if UNITY_EDITOR
            Debug.Log("Client received: " + rcv);
#endif
            return rcv;
        }

        private string getAuth()
        {
            var JObject = new JObject();
            JObject.Add(new JProperty(StreamObjectKeys.COMMAND, NeuosStreamConstants.StreamCommandValues.AUTH));
            JObject.Add(new JProperty(StreamObjectKeys.API_KEY, ApiKey));
            return JObject.ToString();

        }

        private void OnDestroy()
        {
            if (IsConnected)
            {
                IsConnected = false;
                m_Socket?.Close();
                m_Socket?.Dispose();
            }
        }
    }
}