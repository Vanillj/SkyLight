using Client.Managers;
using GameClient.Types.Item;
using GameServer.General;
using GameServer.Scenes;
using GameServer.Types.Item;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Converters;
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
            List<MessageTemplate> QueueList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MessageTemplate>>(s, new StringEnumConverter());

            QueueList.ForEach(template =>
            {
                int index = -1;
                switch (template.MessageType)
                {
                    case MessageType.Movement:
                        Keys[] KeyState = Newtonsoft.Json.JsonConvert.DeserializeObject<Keys[]>(template.JsonMessage, new StringEnumConverter());
                        CharacterPlayer c = CharacterManager.GetLoginManagerFromUniqueID(message.SenderConnection.RemoteUniqueIdentifier).GetCharacter();
                        InputManager.CalculateMovement(c, KeyState);
                        break;

                    case MessageType.Login:
                        LoginAttempt(message, template);
                        break;

                    case MessageType.Register:
                        LoginManagerServer login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginManagerServer>(template.JsonMessage, new StringEnumConverter());
                        RegisterUser(login);
                        break;

                    //send by selfmade function, not Lidgren
                    case MessageType.Disconnected:
                        //OnDisconnected(message.SenderConnection);
                        break;
                    case MessageType.EquipItem:
                        try
                        {
                            int.TryParse(template.JsonMessage, out index);
                        }
                        catch
                        { }

                        if (index != -1)
                        {
                            CharacterPlayer characterPlayer = CharacterManager.GetLoginManagerFromUniqueID(message.SenderConnection.RemoteUniqueIdentifier).GetCharacter();
                            WeaponItem newItem = characterPlayer.GetInventory().ElementAt(index);
                            if (newItem != null)
                            {
                                int type = (int)newItem.GetEqupmentType();
                                WeaponItem currentItem = characterPlayer.Equipment[type];

                                if (currentItem == null)
                                    characterPlayer.Inventory[index] = null;
                                else
                                    characterPlayer.Inventory[index] = currentItem;

                                characterPlayer.Equipment[type] = newItem;
                            }
                        }
                        break;
                    case MessageType.UnEquipItem:
                        CharacterPlayer character = CharacterManager.GetLoginManagerFromUniqueID(message.SenderConnection.RemoteUniqueIdentifier).GetCharacter();
                        var inventory = character.GetInventory();
                        var equpment = character.GetEquipment();
                        int invIndex = -1;
                        for (int i = inventory.Length - 1; i >= 0; i--)
                        {
                            if (inventory[i] == null)
                            {
                                invIndex = i;
                            }
                        }
                        try
                        {
                            int.TryParse(template.JsonMessage, out index);
                        }
                        catch (Exception)
                        {

                        }
                        if (invIndex != -1)
                        {
                            inventory[invIndex] = equpment[index];
                            equpment[index] = null;
                        }
                        break;
                }
            });

        }

        private void LoginAttempt(NetIncomingMessage message, MessageTemplate template)
        {
            LoginManagerServer LoginManagerServerUser = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginManagerServer>(template.JsonMessage, new StringEnumConverter());

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

                    string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(LoginManagerServerUser.GetCharacter(), new StringEnumConverter());
                    MessageTemplate TempMessageTemplate = new MessageTemplate(characterString, MessageType.LoginSuccess);

                    //Returns the character to the player
                    NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(TempMessageTemplate, new StringEnumConverter()));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                    CharacterManager.AddLoginManagerServerToList(LoginManagerServerUser);
                    CharacterManager.AddCharacterToScene(Scene, LoginManagerServerUser);
                }
                else
                {
                    MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                    NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp, new StringEnumConverter()));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                }

            }
            else
            {
                MessageTemplate temp = new MessageTemplate("Failure.", MessageType.LoginFailure);
                NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(temp, new StringEnumConverter()));
                sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
            }


        }

        private void RegisterUser(LoginManagerServer login)
        {
            //TODO: Move register to its own function
            //TODO: add better character creation later
            //TODO: Change login.username to requrested character name
            login.SetCharacter(new CharacterPlayer(0, 0, login.username, new WeaponItem[ConstatValues.EquipmentLength], new WeaponItem[ConstatValues.BaseInventoryLength]));
            string tempC = Newtonsoft.Json.JsonConvert.SerializeObject(login.GetCharacter(), new StringEnumConverter());
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
                string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(characterPlayer, new StringEnumConverter());
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
