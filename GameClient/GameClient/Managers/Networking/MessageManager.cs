using Client.Managers;
using Client.Types;
using GameClient.Scenes;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Nez;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Debug = System.Diagnostics.Debug;

namespace GameClient.Managers
{
    class MessageManager
    {

        private static LoginManagerClient login;
        private static List<MessageTemplate> QueueList = new List<MessageTemplate>();
        public static InputManager inputManager { get; set; }
        public MessageManager()
        {
        }

        public static void CheckForMessage()
        {
            CheckConnection();
            NetIncomingMessage message;
            if ((message = ClientNetworkManager.client.ReadMessage()) != null)
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

                    default:
                        Debug.WriteLine("unhandled message with type: " + message.MessageType + ": " + message.ReadString());
                        break;
                }
            }
        }

        public static void SendQueue()
        {
            if (!QueueList.Any())
                return;
            NetOutgoingMessage message1 = ClientNetworkManager.client.CreateMessage(MessageTemplate.TemplateToJson(QueueList));
            ClientNetworkManager.client.SendMessage(message1, NetDeliveryMethod.ReliableOrdered);

            //Always end with empty list
            if (QueueList.Any())
                QueueList.Clear();
        }

        public static void AddToQueue(MessageTemplate TemplateToAdd)
        {
            QueueList.Add(TemplateToAdd);
        }

        private static void CustomMessage(NetIncomingMessage message)
        {
            string s = message.ReadString();
            var template = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageTemplate>(s);
            switch (template.MessageType)
            {
                case MessageType.LoginSuccess:
                    Console.WriteLine("Login and recieved sucessfully!");
                    Core.StartSceneTransition(new FadeTransition(() => new MainScene() { InputManager = inputManager }));
                    Character c = Newtonsoft.Json.JsonConvert.DeserializeObject<Character>(template.JsonMessage);
                    login.SetCharacter(c);
                    break;

                case MessageType.LoginFailure:

                    break;
                case MessageType.Movement:
                    Vector2 vector2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Vector2>(template.JsonMessage);
                    login.GetCharacter().MoveToPos(vector2);
                    break;
            }
        }

        private static int attempts = 0;
        public static int framesPassed = 0;
        private static void CheckConnection()
        {
            if (ClientNetworkManager.connection == null)
                return;
            framesPassed++;
            if (ClientNetworkManager.connection.Status == NetConnectionStatus.Disconnected && framesPassed > 10)
            {
                attempts++;
                if (!ClientNetworkManager.TryToConnect(login))
                    Debug.WriteLine("Attemps: " + attempts);
                else
                    attempts = 0;
            }
        }

        private static void ConnectionChange(NetIncomingMessage message)
        {
            ClientNetworkManager.connection = message.SenderConnection;
            if (message.SenderConnection.Status == NetConnectionStatus.Connected)
            {
                Debug.WriteLine("Successfully connected!");

            }
            if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
            {
                Debug.WriteLine("Disconnected!");
            }
        }

        public static void SendLoginRequest()
        {
            List<MessageTemplate> tempQueue = new List<MessageTemplate>
            {
                new MessageTemplate(Newtonsoft.Json.JsonConvert.SerializeObject(login), MessageType.Login)
            };

            string send = Newtonsoft.Json.JsonConvert.SerializeObject(tempQueue);

            var messageToSend = ClientNetworkManager.client.CreateMessage(send);
            ClientNetworkManager.client.SendMessage(messageToSend, NetDeliveryMethod.ReliableOrdered);
            Debug.WriteLine("Successfully Sent login request!");
        }

        public static void SendRegisterRequest()
        {
            List<MessageTemplate> tempQueue = new List<MessageTemplate>();
            tempQueue.Add(new MessageTemplate(Newtonsoft.Json.JsonConvert.SerializeObject(login), MessageType.Register));
            string send = Newtonsoft.Json.JsonConvert.SerializeObject(tempQueue);

            var messageToSend = ClientNetworkManager.client.CreateMessage(send);
            ClientNetworkManager.client.SendMessage(messageToSend, NetDeliveryMethod.ReliableOrdered); 
            Debug.WriteLine("Successfully Sent register request!");
        }
        
        public static void SetLoginManagerClient(LoginManagerClient _login)
        {
            login = _login;
        }
    }
}
