using Client.Managers;
using GameServer.General;
using GameServer.Scenes;
using Lidgren.Network;
using Newtonsoft.Json.Converters;
using Nez;
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
        public static void SendStringToUniqueID(Scene scene, string message, long UniqueID, MessageType type)
        {
            MessageTemplate temp = new MessageTemplate(message, type);

            NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
            var connections = ServerNetworkSceneComponent.GetNetServer().Connections;

            NetConnection reciever = connections.Find(c => c.RemoteUniqueIdentifier.Equals(UniqueID));
            if (reciever != null)
            {
                NetSendResult result = reciever.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableUnordered, 1);
                int size;
                int free;
                reciever.GetSendQueueInfo(NetDeliveryMethod.ReliableUnordered, 1, out size, out free);
                if (free < -size)
                {
                    DisconnectConnection(reciever, scene);
                }
            }

        }

        public static void DisconnectConnection(NetConnection sender, Scene scene)
        {
            NetServer server = ServerNetworkSceneComponent.GetNetServer();

            LoginManagerServer login = MapContainer.GetLoginByID(sender.RemoteUniqueIdentifier);
            if (login != null)
            {
                CharacterPlayer characterPlayer = login.GetCharacter();

                //Saves data to SQL database
                string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(characterPlayer, new StringEnumConverter());
                SQLManager.UpdateToSQL(login.username, characterString);

                //removes login manager
                MapContainer.RemoveLoginByID(login.GetUniqueID());

                if (characterPlayer != null)
                {
                    //removes entity
                    CharacterManager.RemoveCharacterFromScene(scene, characterPlayer._name);
                }
            }

            //removes the connection
            sender.Disconnect("Closed");
            Console.WriteLine("Disconnected! Connected: " + ServerNetworkSceneComponent.GetNetServer().ConnectionsCount);
            MainScene.ConnectedCount.SetText("Current connections: " + server.ConnectionsCount);
        }
    }
}
