using Client.Managers;
using GameClient.Types.Item;
using GameServer.General;
using GameServer.Scenes;
using GameServer.Types.Item;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using Nez;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Debug = System.Diagnostics.Debug;

namespace GameServer.Types.Components.SceneComponents
{
    class MessageSceneComponent : SceneComponent
    {

        public override void Update()
        {
            CheckForMessageAvailable();
            base.Update();
        }

        public void CheckForMessageAvailable()
        {
            NetIncomingMessage message;
            NetServer server = ServerNetworkSceneComponent.GetNetServer();
            if ((message = server.ReadMessage()) != null)
            {
                CheckForMessage(message);
            }

        }

        private void CheckForMessage(NetIncomingMessage message)
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

        private void CustomMessage(NetIncomingMessage message)
        {
            string s = message.ReadString();
            List<MessageTemplate> QueueList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageTemplate>>(s);

            QueueList.ForEach(template =>
            {
                switch (template.MessageType)
                {
                    case MessageType.Movement:
                        Keys[] KeyState = Newtonsoft.Json.JsonConvert.DeserializeObject<Keys[]>(template.JsonMessage);
                        CharacterPlayer c = CharacterManager.GetLoginManagerFromUniqueID(message.SenderConnection.RemoteUniqueIdentifier).GetCharacter();
                        InputManager.CalculateMovement(c, KeyState);
                        break;

                    case MessageType.Login:
                        LoginAttempt(message, template);
                        break;

                    case MessageType.Register:
                        LoginManagerServer login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginManagerServer>(template.JsonMessage);
                        RegisterUser(login);
                        break;

                    //send by selfmade function, not Lidgren
                    case MessageType.Disconnected:
                        //OnDisconnected(message.SenderConnection);
                        break;
                    case MessageType.EquipItem:
                        int index = -1;
                        try
                        {
                            int.TryParse(template.JsonMessage, out index);
                        }
                        catch
                        {

                        }
                        if (index != -1)
                        {
                            CharacterPlayer characterPlayer = CharacterManager.GetLoginManagerFromUniqueID(message.SenderConnection.RemoteUniqueIdentifier).GetCharacter();
                            WeaponItem item = characterPlayer.Inventory.ElementAt(index);
                            if (item != null)
                            {
                                characterPlayer.Inventory[index] = null;
                                int firstnull = Array.IndexOf(characterPlayer.Inventory, null);
                                characterPlayer.Equipment[firstnull] = item;
                            }
                        }
                        break;

                }
            });

        }

        private void LoginAttempt(NetIncomingMessage message, MessageTemplate template)
        {
            LoginManagerServer LoginManagerServerUser = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginManagerServer>(template.JsonMessage);

            LoginManagerServerUser.SetUniqueID(message.SenderConnection.RemoteUniqueIdentifier);
            Console.WriteLine("Login attempt by: " + LoginManagerServerUser.username);
            NetConnection sender = message.SenderConnection;

            if (SQLManager.CheckIfExistInSQL(LoginManagerServerUser.username))
            {
                //true if logged in
                bool success = LoginManagerServerUser.SetupLogin();
                if (success)
                {
                    Console.WriteLine("Logged in with \"" + LoginManagerServerUser.username + "\" to Database");

                    string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(LoginManagerServerUser.GetCharacter());
                    MessageTemplate TempMessageTemplate = new MessageTemplate(characterString, MessageType.LoginSuccess);

                    //Returns the character to the player
                    NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(TempMessageTemplate));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                    CharacterManager.AddLoginManagerServerToList(LoginManagerServerUser);
                    CharacterManager.AddCharacterToScene(Scene, LoginManagerServerUser);
                }
                else
                {
                    MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                    NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                }

            }
            else
            {
                MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp));
                sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
            }


        }

        private void RegisterUser(LoginManagerServer login)
        {
            //TODO: Move register to its own function
            //TODO: add better character creation later
            //TODO: Change login.username to requrested character name
            login.SetCharacter(new CharacterPlayer(0, 0, login.username, new WeaponItem[ConstatValues.EquipmentLength], new WeaponItem[ConstatValues.BaseInventoryLength]));
            string tempC = Newtonsoft.Json.JsonConvert.SerializeObject(login.GetCharacter());
            SQLManager.AddToSQL(login.username, login.password, tempC);
        }

        private void ConnectionChange(NetIncomingMessage message)
        {
            if (message.SenderConnection.Status == NetConnectionStatus.Connected || message.SenderConnection.Status == NetConnectionStatus.RespondedConnect)
            {
                Debug.WriteLine("New connection!");
            }
            if (message.SenderConnection.Status == NetConnectionStatus.Disconnected)
            {
                OnDisconnected(message.SenderConnection);
            }
            MainScene.ConnectedCount.SetText("Current connections: " + ServerNetworkSceneComponent.GetNetServer().ConnectionsCount);
        }

        private void OnDisconnected(NetConnection sender)
        {
            NetServer server = ServerNetworkSceneComponent.GetNetServer();

            LoginManagerServer login = CharacterManager.GetLoginManagerFromUniqueID(sender.RemoteUniqueIdentifier);
            if (login != null)
            {
                CharacterPlayer characterPlayer = login.GetCharacter();

                //Saves data to SQL database
                string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(characterPlayer);
                SQLManager.UpdateToSQL(login.username, characterString);

                //removes login manager
                CharacterManager.RemoveLoginManagerServerFromListLoginManager(login);
                if (characterPlayer != null)
                {
                    //removes entity
                    CharacterManager.RemoveCharacterFromScene(Scene, characterPlayer._name);
                }
            }

            //removes the connection
            server.Connections.Remove(sender);

            Debug.WriteLine("Disconnected! Connected: " + ServerNetworkSceneComponent.GetNetServer().ConnectionsCount);
            MainScene.ConnectedCount.SetText("Current connections: " + server.ConnectionsCount);
        }

    }
}
