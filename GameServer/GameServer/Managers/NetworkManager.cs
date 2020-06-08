using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Server.Types;

namespace Server.Managers
{
    class NetworkManager
    {
        private static ServerNetworkManager _server;
        private static byte[] _buffer = new byte[1024];
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static HashSet<Socket> _clientSockets = new HashSet<Socket>();


        public static Socket RequestSocket;

        public NetworkManager(ServerNetworkManager server)
        {
            _server = server;
            ServerSetup();
        }

        private static void ServerSetup()
        {
            Debug.WriteLine("Setting up server...");
            _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 100));
            _serverSocket.Listen(5);
            _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void AcceptCallback(IAsyncResult result)
        {
            try
            {
                Socket socket = _serverSocket.EndAccept(result);
                _clientSockets.Add(socket);
                Debug.WriteLine("Client connected. Character count: " + CharacterManager.LoginManagerServerList.Count + ", Connection count: " + NetworkManager.GetNetworkCount());
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (ObjectDisposedException)
            {

            }
            
        }

        private static string response;
        private static void RecieveCallback(IAsyncResult result)
        {
            response = State.False.ToString();
            try
            {
                //fetch the info
                Socket socket = (Socket)result.AsyncState;
                RequestSocket = socket;
                if (socket.Connected && CheckConnection(socket))
                {
                    int recieved = socket.EndReceive(result);
                    byte[] dataBuf = new byte[recieved];
                    Array.Copy(_buffer, dataBuf, recieved);

                    //converts the info
                    string responseText = Encoding.ASCII.GetString(dataBuf);
                    Debug.WriteLine(responseText);

                    if (responseText != "" && responseText != null)
                    {
                        //string[] split = text.Split(new string[] { STRINGPLITER }, StringSplitOptions.RemoveEmptyEntries); //[0] is method name, [1] is information sent
                        //string methodLower = methodLower = split[0].ToLower();
                        //string informationString = split[1];                        
                        FunctionManager fm = Newtonsoft.Json.JsonConvert.DeserializeObject<FunctionManager>(responseText);
                        response = fm.CheckForMethod(socket);
                        //check for method
                        //response = FunctionManager.CheckForMethod(methodLower, informationString, socket);
                    }
                    //respond with either success or not.
                    SendText(response, socket);
                }
            }
            catch (ObjectDisposedException ex)
            {
                //TODO
                Debug.WriteLine("Error in socket: " + ex.Message);
            }
        }

        private static void SendText(string text, Socket socket)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(text);
                socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(RecieveCallback), socket);
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine("Error in socket reading or sending: " + ex.Message);
            }
        }

        public static void ReleaseAllSockets()
        {
            _serverSocket.Close();
            foreach (Socket socket in _clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            _clientSockets.Clear();
            
        }

        public static void RemoveSocket(Socket socket)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            _clientSockets.Remove(socket);
        }

        private static void SendCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndSend(result);
        }

        public static bool CheckConnection(Socket socket)
        {
            bool test1 = socket.Poll(1000, SelectMode.SelectRead);
            bool test2 = socket.Available == 0;
            if (test1 && test2)
                return false;
            else
                return true;
        }


        //Get set functions

        public static int GetNetworkCount()
        {
            return _clientSockets.Count();
        }

    }
    public enum State
    {
        True,
        False
    }
}
