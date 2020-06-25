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
        public ServerNetworkManager()
        {
            NetPeerConfiguration config = new NetPeerConfiguration("test") { Port = 100 };
            server = new NetServer(config);
            server.Start();
        }

        public static NetServer GetNetServer()
        {
            return server;
        }
    }
}
