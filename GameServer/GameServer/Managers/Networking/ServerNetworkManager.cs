using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Server.Managers
{
    class ServerNetworkManager
    {
        private static NetServer server;
        public ServerNetworkManager(string ServerString, int port)
        {
            //TODO: proper setup
            NetPeerConfiguration config = new NetPeerConfiguration(ServerString) { Port = port };
            server = new NetServer(config);
            server.Start();
        }

        public static NetServer GetNetServer()
        {
            return server;
        }
    }
}
