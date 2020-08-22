using Client.Managers;
using GameClient.Types.Item;
using GameServer.General;
using GameServer.Managers.Networking;
using GameServer.Scenes;
using GameServer.Types.Abilities;
using GameServer.Types.Abilities.SharedAbilities;
using GameServer.Types.Components.Components;
using GameServer.Types.Item;
using GameServer.Types.Networking;
using Lidgren.Network;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json.Converters;
using Nez;
using Nez.BitmapFonts;
using Server.Managers;
using Server.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                if (message.MessageType == NetIncomingMessageType.ConnectionApproval)
                {
                    message.SenderConnection.Approve();
                }
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
                CharacterPlayer character = null;
                Entity e = null;
                switch (template.MessageType)
                {
                    case MessageType.Movement:
                        Movement(template, message);
                        break;
                    case MessageType.Login:
                        LoginAttempt(message, template);
                        break;
                    case MessageType.Register:
                        LoginManagerServer login = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginManagerServer>(template.JsonMessage, new StringEnumConverter());
                        RegisterUser(login);
                        break;
                    //send by self made function, not Lidgren
                    case MessageType.Disconnected:
                        //OnDisconnected(message.SenderConnection);
                        break;
                    case MessageType.EquipItem:
                        UnEquipItem(template, message);
                        break;
                    case MessageType.UnEquipItem:
                        UnEquipItem(character, message, template);
                        break;
                    case MessageType.StartChanneling:
                        StartChanneling(template, message);
                        break;
                    case MessageType.Target:
                        TargetPlayer(message, template);
                        break;
                    case MessageType.DamageTarget:
                        DamageTarget(message, template);
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
                Entity e = Scene.FindEntity(LoginManagerServerUser.GetCharacter()._name);
                if (success && e == null)
                {
                    Console.WriteLine("Logged in with \"" + LoginManagerServerUser.username + "\" to Database");
                    string characterString = Newtonsoft.Json.JsonConvert.SerializeObject(LoginManagerServerUser.GetCharacter(), new StringEnumConverter());
                    MessageTemplate TempMessageTemplate = new MessageTemplate(characterString, MessageType.LoginSuccess);

                    //Returns the character to the player
                    NetOutgoingMessage mvmntMessage = ServerNetworkSceneComponent.GetNetServer().CreateMessage(Newtonsoft.Json.JsonConvert.SerializeObject(TempMessageTemplate, new StringEnumConverter()));
                    sender.SendMessage(mvmntMessage, NetDeliveryMethod.ReliableOrdered, 0);
                    MapContainer.AssignLogin(Scene, LoginManagerServerUser);
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
            login.SetCharacter(new CharacterPlayer(0, 0, login.username, new WeaponItem[ConstantValues.EquipmentLength], new WeaponItem[ConstantValues.BaseInventoryLength]) { LastMultiLocation = ConstantValues.DefaultMap });
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
            MessageManager.DisconnectConnection(sender, Scene);
        }

        private void UnEquipItem(CharacterPlayer character, NetIncomingMessage message, MessageTemplate template)
        {
            character = MapContainer.FindCharacterByID(message.SenderConnection.RemoteUniqueIdentifier);
            var inventory = character.GetInventory();
            var equpment = character.GetEquipment();
            int invIndex = Array.FindIndex(inventory, i => i == null);
            int index = -1;
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
        }

        private void UnEquipItem(MessageTemplate template, NetIncomingMessage message)
        {
            int index = -1;
            try
            {
                int.TryParse(template.JsonMessage, out index);
            }
            catch
            { }

            if (index != -1)
            {
                CharacterPlayer character = MapContainer.FindCharacterByID(message.SenderConnection.RemoteUniqueIdentifier);
                WeaponItem newItem = character.GetInventory().ElementAt(index);
                if (newItem != null)
                {
                    int type = (int)newItem.GetEqupmentType();
                    WeaponItem currentItem = character.Equipment[type];

                    if (currentItem == null)
                        character.Inventory[index] = null;
                    else
                        character.Inventory[index] = currentItem;

                    character.Equipment[type] = newItem;
                }
            }
        }

        private void Movement(MessageTemplate template, NetIncomingMessage message)
        {
            Keys[] KeyState = Newtonsoft.Json.JsonConvert.DeserializeObject<Keys[]>(template.JsonMessage, new StringEnumConverter());
            CharacterPlayer character = MapContainer.FindCharacterByID(message.SenderConnection.RemoteUniqueIdentifier);
            bool moved = InputManager.CalculateMovement(character, KeyState, message.SenderConnection.RemoteUniqueIdentifier);
            if (moved)
            {
                Entity entity = Scene.FindEntity(character._name);
                if (entity != null)
                {
                    PlayerComponent playerComponent = entity.GetComponent<PlayerComponent>();
                    if (playerComponent != null)
                        playerComponent.isChanneling = false;
                }
            }
        }

        private void StartChanneling(MessageTemplate template, NetIncomingMessage message)
        {
            ChannelTemplate ct = Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelTemplate>(template.JsonMessage);
            if (ct.ChannelType.Equals(ChannelType.Ability))
            {
                AbilityHead ability = AbilityContainer.GetAbilityByName(ct.ChannelName);
                CharacterPlayer character = MapContainer.FindCharacterByID(message.SenderConnection.RemoteUniqueIdentifier);
                Entity entity = Scene.FindEntity(character._name);
                PlayerComponent pc = entity.GetComponent<PlayerComponent>();
                if (pc != null && !pc.isChanneling)
                {
                    if (ability != null)
                        entity.AddComponent(new DamageChannelingComponent(pc, ability.ChannelTime, ability));
                    else
                        entity.AddComponent(new ChannelingComponent(pc, 4));
                }

            }
        }

        private void TargetPlayer(NetIncomingMessage message, MessageTemplate template)
        {
            CharacterPlayer character = MapContainer.FindCharacterByID(message.SenderConnection.RemoteUniqueIdentifier);
            Entity entity = Scene.FindEntity(character._name);
            var et = Scene.FindEntity(template.JsonMessage);
            PlayerComponent p = entity.GetComponent<PlayerComponent>();
            if (template.JsonMessage != "")
                p.Target = et;
            else
                p.Target = null;
        }

        private void DamageTarget(NetIncomingMessage message, MessageTemplate template)
        {
            CharacterPlayer character = MapContainer.FindCharacterByID(message.SenderConnection.RemoteUniqueIdentifier);
            AbilityHead abi = AbilityContainer.GetAbilityByName(template.JsonMessage);
            Entity entity = Scene.FindEntity(character._name);
            PlayerComponent pcomp = entity.GetComponent<PlayerComponent>();
            if (pcomp != null && abi != null && pcomp.Target != null)
            {
                pcomp.Target.GetComponent<DamageComponent>().DealDamageToEntity(abi.BaseDamage);
            }
        }

    }
}
