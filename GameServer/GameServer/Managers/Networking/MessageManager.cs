using Client.Managers;
using GameServer.Scenes;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using Server.Types;
using System;
using System.Collections.Generic;
using Debug = System.Diagnostics.Debug;

namespace Server.Managers
{
    class MessageManager
    {

        public static void CheckForMessageAvailable()
        {
            NetIncomingMessage message;
            if ((message = ServerNetworkManager.server.ReadMessage()) != null)
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
            Character c = CharacterManager.GetCharacterFromUniqueID(message.SenderConnection.RemoteUniqueIdentifier);

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
                }
            });

            //Gets the the position and returns it
            if (c != null)
            {
                string posString = Newtonsoft.Json.JsonConvert.SerializeObject(c._pos);
                MessageTemplate temp = new MessageTemplate(posString, MessageType.Movement);
                NetOutgoingMessage mvmntMessage = ServerNetworkManager.server.CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                NetConnection sender = message.SenderConnection;
                sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                Console.WriteLine("Reply successful.");
            }
            var data = message.ReadString();
            Debug.WriteLine(data);
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
                    Character temps = new Character(0,0);
                    string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(temps);
                    MessageTemplate temp = new MessageTemplate(characterString, MessageType.LoginSuccess);
                    NetOutgoingMessage mvmntMessage = ServerNetworkManager.server.CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                }
                else
                {
                    MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                    NetOutgoingMessage mvmntMessage = ServerNetworkManager.server.CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                }
                
            }
            else
            {
                MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                NetOutgoingMessage mvmntMessage = ServerNetworkManager.server.CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
            }

            CharacterManager.AddLoginManagerServerToList(temsp);
            
        }

        private static void RegisterUser(LoginManagerServer login)
        {
            //TODO: Move register to its own function
            //TODO: add better character creation later
            login.SetCharacter(new Character(0, 0));
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
                Debug.WriteLine("Disconnected! Connected: " + ServerNetworkManager.server.ConnectionsCount);

            }
            MainScene.ConnectedCount.SetText("Current connections: " + ServerNetworkManager.server.ConnectionsCount);
        }

    }
}
