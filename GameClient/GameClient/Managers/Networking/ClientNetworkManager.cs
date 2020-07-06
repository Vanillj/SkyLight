using Lidgren.Network;

namespace Client.Managers
{
    class ClientNetworkManager
    {
        public static NetClient client;
        public static NetConnection connection;

        public static void SetupClient()
        {
            var config = new NetPeerConfiguration("test");
            client = new NetClient(config);
            client.Start();
        }

        public static bool TryToConnect(LoginManagerClient loginManager)
        {
            if (client.ServerConnection != null)
                return true;

            //TODO: ADD LATER
            connection = client.Connect(host: "127.0.0.1", port: 100);
            if (connection != null && connection.Status == NetConnectionStatus.Connected)
            {
                return true;
            }
            return false;
        }
    }
}
