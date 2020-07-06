using Client.Managers;
using GameClient.Managers;
using GameClient.Scenes;
using GameServer.Types;
using Lidgren.Network;
using Microsoft.Xna.Framework;
using Nez;
using Server.Managers;
using Server.Types;
using System.Collections.Generic;
using System.Linq;
using Debug = System.Diagnostics.Debug;

namespace GameClient.Types.Components.SceneComponents
{
    class MessageSceneComponent : SceneComponent
    {
        private static int attempts = 0;
        public static int framesPassed = 0;
        float timeSpan = 0;

        public override void Update()
        {
            
            timeSpan += Time.DeltaTime;
            CheckForMessage();
            if (timeSpan > 0.033)
            {
                MessageManager.SendQueue();
                timeSpan = 0;
            }
            base.Update();
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
        private static void CheckConnection()
        {
            if (ClientNetworkManager.connection == null)
                return;
            framesPassed++;
            if (ClientNetworkManager.connection.Status == NetConnectionStatus.Disconnected && framesPassed > 10)
            {
                attempts++;
                if (!ClientNetworkManager.TryToConnect(MessageManager.GetLoginManagerClient()))
                    Debug.WriteLine("Attemps: " + attempts);
                else
                    attempts = 0;
                if (MessageManager.GetLoginManagerClient() != null || LoginManagerClient.GetCharacter() == null)
                    MessageManager.SendLoginRequest();
            }
        }

        //TODO: Put into their own functions
        private static void CustomMessage(NetIncomingMessage message)
        {
            var template = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageTemplate>(message.ReadString());

            switch (template.MessageType)
            {
                //TODO: Put These into functions
                case MessageType.LoginSuccess:
                    Debug.WriteLine("Login and recieved sucessfully!");
                    MainScene mainScene = new MainScene();
                    Core.StartSceneTransition(new FadeTransition(() => mainScene));
                    CharacterPlayer c = Newtonsoft.Json.JsonConvert.DeserializeObject<CharacterPlayer>(template.JsonMessage);
                    MessageManager.GetLoginManagerClient().SetCharacter(c);
                    break;

                case MessageType.LoginFailure:
                    Debug.WriteLine("Failed to login! Try again");
                    break;

                case MessageType.Movement:
                    Vector2 vector2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Vector2>(template.JsonMessage);
                    LoginManagerClient.GetCharacter().MoveToPos(vector2);
                    break;
                case MessageType.GameUpdate:
                    DataTemplate dataTemplate = Newtonsoft.Json.JsonConvert.DeserializeObject<DataTemplate>(template.JsonMessage);

                    //the recieved position
                    LoginManagerClient.SetRecievedPosition(dataTemplate.RecieverCharacter._pos);
                    LoginManagerClient.GetCharacter().physicalPosition = dataTemplate.RecieverCharacter.physicalPosition;
                    //TODO: Remove element that are not recieved from list

                    //add other character in area to a list
                    if (dataTemplate.OthersCharacters != null && dataTemplate.OthersCharacters.Count > 0)
                    {
                        //Removes all characters that are not in the recieved list, Entities are destroyed in PlayerComponent later
                        //LoginManagerClient.Othercharacters = LoginManagerClient.Othercharacters.Intersect(dataTemplate.OthersCharacters).ToList();
                        //LoginManagerClient.Othercharacters.RemoveAll(x => !dataTemplate.OthersCharacters.Contains(x));
                        LoginManagerClient.Othercharacters = dataTemplate.OthersCharacters;

                        foreach (CharacterPlayer charac in dataTemplate.OthersCharacters)
                        {
                            int i = LoginManagerClient.Othercharacters.FindIndex(tempc => tempc._name.Equals(charac._name));

                            if (i == -1)
                            {
                                LoginManagerClient.Othercharacters.Add(charac);
                            }
                            /*else
                            {
                                LoginManagerClient.Othercharacters[i] = charac;
                            }*/
                        }
                    }

                    break;
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

    }
}
