using Client.Managers;
using GameClient.Scenes;
using GameClient.Types.Components.SceneComponents;
using GameServer.Types;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tweens;
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

        public MessageManager()
        {

        }

        internal static void SendExitMessage()
        {
            List<MessageTemplate> tempQueue = new List<MessageTemplate>();
            tempQueue.Add(new MessageTemplate(Newtonsoft.Json.JsonConvert.SerializeObject(login), MessageType.Disconnected));
            string send = Newtonsoft.Json.JsonConvert.SerializeObject(tempQueue);

            var messageToSend = ClientNetworkManager.client.CreateMessage(send);
            ClientNetworkManager.client.SendMessage(messageToSend, NetDeliveryMethod.ReliableOrdered);
            Debug.WriteLine("Successfully Sent Exit!");
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

        public static void AddToQueue(MessageTemplate TemplateToAdd)
        {
            QueueList.Add(TemplateToAdd);
        }

        public static void SetLoginManagerClient(LoginManagerClient _login)
        {
            login = _login;
        }

        public static LoginManagerClient GetLoginManagerClient()
        {
            return login;
        }

    }
}
