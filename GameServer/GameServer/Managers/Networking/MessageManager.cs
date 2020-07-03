using Lidgren.Network;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers.Networking
{
    class MessageManager
    {
        public static void SendStringToUniqueID(string message, long UniqueID, MessageType type)
        {
            MessageTemplate temp = new MessageTemplate(message, type);

            NetOutgoingMessage mvmntMessage = ServerNetworkManager.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
            var connections = ServerNetworkManager.GetNetServer().Connections;

            NetConnection reciever = connections.Find(c => c.RemoteUniqueIdentifier.Equals(UniqueID));
            if (reciever != null)
            {
                reciever.SendMessage(mvmntMessage, NetDeliveryMethod.UnreliableSequenced, 0);

            }

        }
    }
}
