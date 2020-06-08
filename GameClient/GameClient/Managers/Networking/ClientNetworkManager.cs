using Lidgren.Network;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            connection = client.Connect(host: "127.0.0.1", port: 100);
            if (connection.Status == NetConnectionStatus.Connected)
            {
                return true;
            }
            return false;
        }
    }
}
