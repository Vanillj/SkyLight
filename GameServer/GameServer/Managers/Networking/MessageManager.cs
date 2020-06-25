using Client.Managers;
using GameServer;
using GameServer.Scenes;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using Nez;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Debug = System.Diagnostics.Debug;

namespace Server.Managers
{
    class MessageManager
    {

        public static void CheckForMessageAvailable()
        {
            NetIncomingMessage message;
            NetServer server = ServerNetworkManager.GetNetServer();
            if ((message = server.ReadMessage()) != null)
            {
                CheckForMessage(message);
            }

        }

        private static void CheckForMessage(NetIncomingMessage message)
        {
            switch (message.MessageType)
            {
                case NetIncomingMessageType.Data:
                    // handle custom messages
                    CustomMessage(message);
                    break;

                case NetIncomingMessageType.StatusChanged:
                    // handle connection status messages
                    ConnectionChange(message);
                    break;

                case NetIncomingMessageType.DebugMessage:
                    // handle debug messages
                    // (only received when compiled in DEBUG mode)
                    Debug.WriteLine(message.ReadString());
                    break;

                /* .. */
                default:
                    Debug.WriteLine("unhandled message with type: "
                        + message.MessageType);
                    break;
            }
        }

        private static void CustomMessage(NetIncomingMessage message)
        {
            string s = message.ReadString();
            List<MessageTemplate> QueueList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageTemplate>>(s);
            CharacterPlayer c = CharacterManager.GetCharacterFromUniqueID(message.SenderConnection.RemoteUniqueIdentifier);

            QueueList.ForEach((template) =>
            {
                switch (template.MessageType)
                {
                    case MessageType.Movement:
                        Keys[] state = Newtonsoft.Json.JsonConvert.DeserializeObject<Keys[]>(template.JsonMessage);
                        InputManager.CalculateMovement(c, state);
                        break;

                    case MessageType.Login:
                        LoginAttempt(message, template);
                        break;

                    case MessageType.Register:
                        LoginManagerServer login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginManagerServer>(template.JsonMessage);
                        RegisterUser(login);
                        break;

                    case MessageType.Disconnected:
                        NetServer server = ServerNetworkManager.GetNetServer();
                        CharacterManager.RemoveLoginManagerServerFromList(message.SenderConnection.RemoteUniqueIdentifier);
                        server.Connections.Remove(message.SenderConnection);
                        MainScene.ConnectedCount.SetText("Current connections: " + server.ConnectionsCount);
                        LoginManagerServer loginManager = CharacterManager.GetLoginManagerServerList().First(l => l.GetUniqueID().Equals(message.SenderConnection.RemoteUniqueIdentifier));
                        Entity e = GameServer.Server.Scene.FindEntity(loginManager.GetCharacter()._name);
                        GameServer.Server.Scene.Entities.Remove(e);
                        break;

                }
            });

        }

        private static void LoginAttempt(NetIncomingMessage message, MessageTemplate template)
        {
            var temsp = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginManagerServer>(template.JsonMessage);

            temsp.SetUniqueID(message.SenderConnection.RemoteUniqueIdentifier);
            Console.WriteLine("Login attempt by: " + temsp.username);
            NetConnection sender = message.SenderConnection;

            if (SQLManager.CheckIfExistInSQL(temsp.username))
            {
                //true if logged in
                bool success = temsp.SetupLogin();
                if (success)
                {
                    Console.WriteLine("Logged in with \"" + temsp.username + "\" to Database");

                    //remove later
                    CharacterPlayer temps = new CharacterPlayer(temsp.AccountCharacter._pos.X, temsp.AccountCharacter._pos.Y, temsp.username);

                    string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(temps);
                    MessageTemplate temp = new MessageTemplate(characterString, MessageType.LoginSuccess);

                    NetOutgoingMessage mvmntMessage = ServerNetworkManager.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                }
                else
                {
                    MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                    NetOutgoingMessage mvmntMessage = ServerNetworkManager.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                }

            }
            else
            {
                MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                NetOutgoingMessage mvmntMessage = ServerNetworkManager.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
            }

            CharacterManager.AddLoginManagerServerToList(temsp);

        }

        private static void RegisterUser(LoginManagerServer login)
        {
            //TODO: Move register to its own function
            //TODO: add better character creation later
            //TODO: Change login.username to requrested character name
            login.SetCharacter(new CharacterPlayer(0, 0, login.username));
            string tempC = Newtonsoft.Json.JsonConvert.SerializeObject(login.GetCharacter());
            SQLManager.AddToSQL(login.username, login.password, tempC);
        }

        private static void ConnectionChange(NetIncomingMessage message)
        {
            if (message.SenderConnection.Status == NetConnectionStatus.Connected || message.SenderConnection.Status == NetConnectionStatus.RespondedConnect)
            {
                Debug.WriteLine("New connection!");
            }
            if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
            {
                CharacterManager.GetLoginManagerServerList().RemoveWhere(l => l.GetUniqueID().Equals(message.SenderConnection.RemoteUniqueIdentifier));
                Debug.WriteLine("Disconnected! Connected: " + ServerNetworkManager.GetNetServer().ConnectionsCount);

            }
            MainScene.ConnectedCount.SetText("Current connections: " + ServerNetworkManager.GetNetServer().ConnectionsCount);
        }

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
